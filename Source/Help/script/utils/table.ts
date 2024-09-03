import { splitLines, trim } from "./string";

export const CommonCreatedColumnNames: ReadonlyArray<string> = [
	"CreatedTimestamp",
	"CreatedAccount",
	"CreatedProgramName",
	"CreatedProgramVersion",
];

export const CommonUpdatedColumnNames: ReadonlyArray<string> = [
	"UpdatedTimestamp",
	"UpdatedAccount",
	"UpdatedProgramName",
	"UpdatedProgramVersion",
	"UpdatedCount",
];

export const CommonColumnNames: ReadonlyArray<string> = [
	...CommonCreatedColumnNames,
	...CommonUpdatedColumnNames,
];

export const Sqlite3BasicTypes = ["integer", "real", "text", "blob"] as const;
export const Sqlite3AffinityTypes = [
	"datetime", // sqlite3 的には数値っぽいから違うんやけどね
	"boolean",
] as const;
export type Sqlite3BasicType = (typeof Sqlite3BasicTypes)[number];
export type Sqlite3AffinityType = (typeof Sqlite3BasicTypes)[number];
export const Sqlite3Types = [
	...Sqlite3BasicTypes,
	...Sqlite3AffinityTypes,
] as const;
export type Sqlite3Type = (typeof Sqlite3Types)[number];

export const SqliteTypeMap = new Map<Sqlite3Type, Sqlite3BasicType>([
	// 通常
	["integer", "integer"],
	["real", "real"],
	["text", "text"],
	["blob", "blob"],
	// 意味だけ
	["datetime", "text"],
	["boolean", "integer"],
]) as ReadonlyMap<Sqlite3Type, Sqlite3BasicType>;

export const ClrTypeFullNames = [
	"System.String",
	"System.Int64",
	"System.Decimal",
	"System.Byte[]",
	"System.Boolean",
	"System.Single",
	"System.Double",
	"System.Guid",
	"System.DateTime",
	"System.Version",
	"System.TimeSpan",
] as const;
export type ClrTypeFullName = (typeof ClrTypeFullNames)[number];

export const ClrTypeMap = new Map<ClrTypeFullName, string>([
	["System.String", "string"],
	["System.Int64", "long"],
	["System.Decimal", "decimal"],
	["System.Byte[]", "byte[]"],
	["System.Boolean", "bool"],
	["System.Single", "float"],
	["System.Double", "double"],
	["System.Guid", "Guid"],
	["System.DateTime", "DateTime"],
	["System.Version", "Version"],
	["System.TimeSpan", "TimeSpan"],
]);

export const ClrMap = new Map<Sqlite3Type, ReadonlyArray<ClrTypeFullName>>([
	["integer", ["System.Int64"]],
	["real", ["System.Decimal", "System.Single", "System.Double"]],
	[
		"text",
		["System.String", "System.Guid", "System.Version", "System.TimeSpan"],
	],
	["blob", ["System.Byte[]"]],
	["datetime", ["System.DateTime", "System.String"]],
	["boolean", ["System.Boolean", "System.Int64"]],
]) as ReadonlyMap<Sqlite3Type, ReadonlyArray<string>>;

const NoneIndex = "*NONE*";

const LayoutColumnIndex = {
	primaryKey: 0,
	notNull: 1,
	foreignKey: 2,
	logicalName: 3,
	physicalName: 4,
	logicalType: 5,
	clrType: 6,
	check: 7,
	comment: 8,
} as const;
const LayoutColumnLength = Object.keys(LayoutColumnIndex).length;

const IndexDefinedIndex = {
	uniqueKey: 0,
	name: 1,
	columnNames: 2,
};
const IndexDefinedLength = Object.keys(IndexDefinedIndex).length;

interface RawSection {
	table: string;
	layout: string[];
	index: string[];
}

export interface ForeignKey {
	table: string;
	column: string;
}

export interface TableColumn {
	isPrimary: boolean;
	notNull: boolean;
	foreignKey: ForeignKey | undefined;
	logical: {
		name: string;
		type: Sqlite3Type;
	};
	physicalName: string;
	clrType: ClrTypeFullName;
	checkConstraints: string;
	comment: string;
}

export interface TableIndex {
	isUnique: boolean;
	name: string;
	columns: string[];
}

export interface TableDefine {
	name: string;
	columns: TableColumn[];
	indexes: TableIndex[];
}

export function splitRawEntities(markdown: string): string[] {
	if (!markdown) {
		return [];
	}

	return markdown.split(/^___$/gm).map((a) => a.trim());
}

const RawSectionHeaderRegex = {
	name: /^#\s(?<TABLE>\w+)$/,
	data: /^##\s(?:layout|index)$/gm,
} as const;

export function splitRawSection(rawEntity: string): RawSection {
	const section: RawSection = {
		table: "",
		layout: [],
		index: [],
	};

	const lines = splitLines(rawEntity);

	if (!lines.length) {
		throw new Error("table");
	}

	let lastIndex = 0;
	const tableLines = lines;
	for (const line of tableLines) {
		try {
			if (!line.trim()) {
				continue;
			}

			const result = line.match(RawSectionHeaderRegex.name);
			if (!result || !result.groups) {
				throw new Error("table");
			}

			section.table = result.groups.TABLE;
			break;
		} finally {
			lastIndex++;
		}
	}

	const dataLines = lines.splice(lastIndex);
	const data = dataLines
		.map((a) => a.trim())
		.filter((a) => a)
		.join("\n");

	const blocks = data.split(RawSectionHeaderRegex.data).filter((a) => a);
	if (blocks.length !== 2) {
		throw new Error("layout/index");
	}

	const layout = blocks[0].trim();
	const index = blocks[1].trim();

	section.layout = splitLines(layout);
	if (index !== NoneIndex) {
		section.index = splitLines(index);
	}

	return section;
}

function toCells(s: string): string[] {
	return trim(s, new Set("|"))
		.split("|")
		.map((a) => a.trim());
}

function isTrue(s: string): boolean {
	return s === "x";
}

export function convertColumns(lines: string[]): TableColumn[] {
	if (lines.length < 3) {
		throw new Error("table markdown");
	}

	const headers = toCells(lines[0]);

	if (headers.length !== LayoutColumnLength) {
		throw new Error("column length");
	}

	const result: TableColumn[] = [];

	const columnLines = lines.splice(2);
	for (const columnLine of columnLines) {
		const columns = toCells(columnLine);
		if (columns.length !== LayoutColumnLength) {
			throw new Error("data length");
		}

		const primaryKey = columns[LayoutColumnIndex.primaryKey];
		const notNull = columns[LayoutColumnIndex.notNull];
		const foreignKey = columns[LayoutColumnIndex.foreignKey];
		const logicalName = columns[LayoutColumnIndex.logicalName];
		const physicalName = columns[LayoutColumnIndex.physicalName];
		const logicalType = columns[LayoutColumnIndex.logicalType] as Sqlite3Type; //TODO: 値チェック
		const clrType = columns[LayoutColumnIndex.clrType] as ClrTypeFullName; //TODO: 値チェック
		const check = columns[LayoutColumnIndex.check];
		const comment = columns[LayoutColumnIndex.comment];

		const foreignKeys = foreignKey
			.trim()
			.split(".", 2)
			.map((a) => a.trim())
			.filter((a) => a);

		const column: TableColumn = {
			isPrimary: isTrue(primaryKey),
			notNull: isTrue(notNull),
			foreignKey:
				foreignKeys.length === 2
					? {
							table: foreignKeys[0],
							column: foreignKeys[1],
						}
					: undefined,
			logical: {
				name: logicalName,
				type: logicalType,
			},
			physicalName: physicalName,
			clrType: clrType,
			checkConstraints: check,
			comment: comment,
		};

		result.push(column);
	}

	return result;
}

export function convertIndexes(lines: string[]): TableIndex[] {
	if (lines.length < 3) {
		throw new Error("table markdown");
	}

	const headers = toCells(lines[0]);

	if (headers.length !== IndexDefinedLength) {
		throw new Error("column length");
	}

	const result: TableIndex[] = [];

	const columnLines = lines.splice(2);
	for (const columnLine of columnLines) {
		const columns = toCells(columnLine);
		if (columns.length !== IndexDefinedLength) {
			throw new Error("data length");
		}

		const uniqueKey = columns[IndexDefinedIndex.uniqueKey];
		const name = columns[IndexDefinedIndex.name];
		const rawColumnNames = columns[IndexDefinedIndex.columnNames];

		const columnNames = rawColumnNames.split(",").map((a) => a.trim());
		if (!columnNames.length) {
			throw new Error("empty column");
		}

		const index: TableIndex = {
			isUnique: isTrue(uniqueKey),
			name: name,
			columns: columnNames,
		};

		result.push(index);
	}

	return result;
}

export function convertTable(section: RawSection): TableDefine {
	return {
		name: section.table,
		columns: convertColumns(section.layout),
		indexes: section.index.length ? convertIndexes(section.index) : [],
	};
}

export interface WorkUpdateState {
	lastUpdateTimestamp: number;
}

export interface WorkDefine extends WorkUpdateState {
	id: string;
	tableName: string;
}

export interface WorkForeignKey {
	tableId: string;
	columnId: string;
}

export interface WorkColumn extends TableColumn, WorkUpdateState {
	/** カラム編集ID。 */
	id: string;
	foreignKeyId: WorkForeignKey | undefined;
}

export interface WorkColumns extends WorkUpdateState {
	id: string;
	items: WorkColumn[];
}

export interface WorkIndex extends TableIndex {
	id: string;
}

export interface WorkIndexes extends WorkUpdateState {
	id: string;
	items: WorkIndex[];
}

export interface WorkTable extends WorkUpdateState {
	id: string;
	define: WorkDefine;
	columns: WorkColumns;
	indexes: WorkIndexes;
}

export function generateTimestamp(): number {
	return new Date().getTime();
}

function generateId() {
	return crypto.randomUUID();
}

export function generateDefineId() {
	return generateId();
}

export function generateTableId() {
	return generateId();
}

export function generateColumnsId() {
	return generateId();
}

export function generateColumnId() {
	return generateId();
}

export function generateIndexesId() {
	return generateId();
}

export function generateIndexId() {
	return generateId();
}

export function convertWorkTable(tableDefine: TableDefine): WorkTable {
	return {
		id: generateDefineId(),
		lastUpdateTimestamp: 0,
		define: {
			id: generateTableId(),
			lastUpdateTimestamp: 0,
			tableName: tableDefine.name,
		},
		columns: {
			id: generateColumnsId(),
			lastUpdateTimestamp: 0,
			items: tableDefine.columns.map((a) => ({
				...a,
				id: generateColumnId(),
				lastUpdateTimestamp: 0,
				foreignKeyId: undefined,
			})),
		},
		indexes: {
			id: generateIndexesId(),
			lastUpdateTimestamp: 0,
			items: tableDefine.indexes.map((a) => ({
				...a,
				id: generateIndexId(),
				lastUpdateTimestamp: 0,
			})),
		},
	};
}

export function updateRelations(workTables: WorkTable[]): void {
	for (const workTable of workTables) {
		for (const workColumn of workTable.columns.items) {
			const foreignKey = workColumn.foreignKey;
			if (foreignKey) {
				const targetTable = workTables.find(
					(a) => a.define.tableName === foreignKey.table,
				);
				if (!targetTable) {
					continue;
				}
				const targetColumn = targetTable.columns.items.find(
					(a) => a.physicalName === foreignKey.column,
				);
				if (!targetColumn) {
					continue;
				}
				workColumn.foreignKeyId = {
					tableId: targetTable.id,
					columnId: targetColumn.id,
				};
			}
		}
	}
}
