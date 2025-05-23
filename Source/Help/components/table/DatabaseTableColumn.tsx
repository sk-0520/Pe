import DeleteIcon from "@mui/icons-material/Delete";
import { Box, IconButton, MenuItem, TableRow, Typography } from "@mui/material";
import { useAtomValue } from "jotai";
import {
	type BaseSyntheticEvent,
	type FC,
	type MouseEvent,
	useEffect,
} from "react";
import { Controller, useForm } from "react-hook-form";
import {
	WorkTablesAtom,
	useWorkColumn,
	useWorkColumns,
} from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import { getValue } from "../../utils/access";
import {
	Sqlite3AffinityTypes,
	Sqlite3BasicTypes,
	type Sqlite3Type,
	SqliteTypeMap,
} from "../../utils/sqlite";
import {
	ClrMap,
	type ClrTypeFullName,
	ClrTypeFullNames,
	ClrTypeMap,
	CommonCreatedColumnNames,
	CommonUpdatedColumnNames,
	type ForeignKey,
	type WorkColumn,
	type WorkForeignKey,
	type WorkTable,
	isCommonColumnName,
} from "../../utils/table";
import { ListGroupHeader } from "../ListGroupHeader";
import {
	EditorCell,
	EditorCheckbox,
	EditorSelect,
	EditorTextField,
} from "./editor";

interface InputValues {
	isPrimary: boolean;
	notNull: boolean;
	foreignKey: string;
	logicalName: string;
	logicalType: Sqlite3Type;
	physicalName: string;
	clrType: ClrTypeFullName;
	comment: string;
}

interface ForeignKeyItem<Type extends "Table" | "Column", Data> {
	type: Type;
	data: Data;
}
type ForeignKeyTableItem = ForeignKeyItem<"Table", WorkTable>;
type ForeignKeyColumnItem = ForeignKeyItem<"Column", WorkColumn> & {
	table: WorkTable;
};

interface Sqlite3Item<Type extends "Title" | "Type"> {
	type: Type;
	display: string;
}
type Sqlite3TitleItem = Sqlite3Item<"Title">;
type Sqlite3TypeItem = Sqlite3Item<"Type"> & {
	data: Sqlite3Type;
};

const Sqlite3Items: ReadonlyArray<Sqlite3TitleItem | Sqlite3TypeItem> = [
	{ type: "Title", display: "SQLite3" } satisfies Sqlite3TitleItem,
	...Sqlite3BasicTypes.map(
		(a) => ({ type: "Type", display: a, data: a }) satisfies Sqlite3TypeItem,
	),
	{ type: "Title", display: "affinity" } satisfies Sqlite3TitleItem,
	...Sqlite3AffinityTypes.map(
		(a) => ({ type: "Type", display: a, data: a }) satisfies Sqlite3TypeItem,
	),
] as const;

interface DatabaseTableColumnProps extends TableBaseProps {
	columnId: string;
}

export const DatabaseTableColumn: FC<DatabaseTableColumnProps> = (
	props: DatabaseTableColumnProps,
) => {
	const { tableId, columnId } = props;
	const { workColumns, updateWorkColumns } = useWorkColumns(tableId);
	const { workColumn, updateWorkColumn } = useWorkColumn(tableId, columnId);
	const {
		id,
		isPrimary,
		notNull,
		foreignKeyId,
		logical,
		physicalName,
		clrType,
		comment,
	} = workColumn;

	const workTables = useAtomValue(WorkTablesAtom);

	const foreignTables = workTables.filter((a) => a.id !== tableId);
	const foreignTableColumns = foreignTables.flatMap((a) => [
		{
			type: "Table",
			data: a,
		} satisfies ForeignKeyTableItem,
		...a.columns.items
			.filter((b) => !isCommonColumnName(b.physicalName))
			.map(
				(b) =>
					({
						type: "Column",
						data: b,
						table: a,
					}) satisfies ForeignKeyColumnItem,
			),
	]);

	const foreignTable = foreignKeyId
		? foreignTables.find((a) => a.id === foreignKeyId.tableId)
		: undefined;
	const foreignColumn =
		foreignTable && foreignKeyId
			? foreignTable.columns.items.find((a) => a.id === foreignKeyId.columnId)
			: undefined;

	const { control, handleSubmit, watch, setValue } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			isPrimary: isPrimary,
			notNull: notNull,
			foreignKey:
				foreignTable && foreignColumn
					? `${foreignTable.id}.${foreignColumn.id}`
					: "",
			logicalName: logical.name,
			logicalType: logical.type,
			physicalName: physicalName,
			clrType: clrType,
			comment: comment,
		},
	});

	const isCommonCreatedColumn = CommonCreatedColumnNames.includes(
		watch("physicalName"),
	);
	const isCommonUpdatedColumn = CommonUpdatedColumnNames.includes(
		watch("physicalName"),
	);
	const isCommonColumn = isCommonCreatedColumn || isCommonUpdatedColumn;

	// この辺データ構造全くわからんわ
	const physicalType = getValue(SqliteTypeMap, watch("logicalType"));
	const selectableClrTypes = getValue(ClrMap, watch("logicalType"));
	console.debug(selectableClrTypes);

	useEffect(() => {
		if (!selectableClrTypes.includes(clrType)) {
			const value = selectableClrTypes[0];
			setValue("clrType", value as ClrTypeFullName);
		}
	}, [selectableClrTypes, clrType, setValue]);

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object>,
	): void {
		let foreignKey: ForeignKey | undefined = undefined;
		let foreignKeyId: WorkForeignKey | undefined = undefined;
		if (data.foreignKey) {
			const [foreignKeyTableId, foreignKeyColumnId] =
				data.foreignKey.split(".");
			foreignKeyId = {
				// @ts-expect-error ts(2345)
				tableId: foreignKeyTableId,
				// @ts-expect-error ts(2345)
				columnId: foreignKeyColumnId,
			};
			const foreignKeyTable = foreignTables.find(
				(a) => a.id === foreignKeyTableId,
			);
			if (foreignKeyTable) {
				const foreignKeyColumn = foreignKeyTable.columns.items.find(
					(a) => a.id === foreignKeyColumnId,
				);
				if (foreignKeyColumn) {
					foreignKey = {
						table: foreignKeyTable.define.tableName,
						column: foreignKeyColumn.logical.name,
					};
				}
			}
			if (!foreignKey) {
				foreignKeyId = undefined;
			}
		}

		updateWorkColumn({
			id: id,
			isPrimary: data.isPrimary,
			notNull: data.notNull,
			foreignKey: foreignKey,
			foreignKeyId: foreignKeyId,
			logical: {
				name: data.logicalName,
				type: data.logicalType,
			},
			physicalName: data.physicalName,
			clrType: data.clrType,
			comment: data.comment,
		});
	}

	function handleRemove(event: MouseEvent): void {
		const index = workColumns.items.findIndex((a) => a.id === columnId);
		if (index === -1) {
			throw new Error();
		}
		const newItems = [...workColumns.items];
		newItems.splice(index, 1);
		updateWorkColumns({
			...workColumns,
			items: newItems,
		});
	}

	return (
		<TableRow
			sx={{
				opacity: isCommonColumn ? 0.4 : undefined,
			}}
		>
			<EditorCell>
				<IconButton onClick={handleRemove}>
					<DeleteIcon />
				</IconButton>
			</EditorCell>
			<EditorCell>
				<Controller
					name="isPrimary"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox
							checked={field.value}
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="notNull"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox
							checked={field.value}
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Box>
					<Controller
						name="foreignKey"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorSelect
								{...field}
								sx={{ fontSize: "80%" }}
								onBlur={handleSubmit(handleInput)}
							>
								<MenuItem value="">{"未設定"}</MenuItem>

								{foreignTableColumns.map((a) => {
									switch (a.type) {
										case "Table":
											return (
												<ListGroupHeader key={a.data.id}>
													{a.data.define.tableName}
												</ListGroupHeader>
											);

										case "Column":
											return (
												<MenuItem
													key={`${a.table.id}.${a.data.id}`}
													value={`${a.table.id}.${a.data.id}`}
												>
													{a.data.physicalName}
												</MenuItem>
											);
									}
								})}
							</EditorSelect>
						)}
					/>
				</Box>
			</EditorCell>
			<EditorCell>
				<Controller
					name="logicalName"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="physicalName"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="logicalType"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorSelect {...field} onBlur={handleSubmit(handleInput)}>
							{Sqlite3Items.map((a) => {
								switch (a.type) {
									case "Title":
										return (
											<ListGroupHeader key={`${a.type}.${a.display}`}>
												{a.display}
											</ListGroupHeader>
										);

									case "Type":
										return (
											<MenuItem key={a.data} value={a.data}>
												{a.display}
											</MenuItem>
										);
								}
							})}
						</EditorSelect>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Typography>{physicalType}</Typography>
			</EditorCell>
			<EditorCell>
				<Controller
					name="clrType"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorSelect {...field} onBlur={handleSubmit(handleInput)}>
							{ClrTypeFullNames.map((a) => {
								return (
									<MenuItem
										key={a}
										value={a}
										disabled={!selectableClrTypes.includes(a)}
									>
										{ClrTypeMap.get(a)}
									</MenuItem>
								);
							})}
						</EditorSelect>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="comment"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
		</TableRow>
	);
};
