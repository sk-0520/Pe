import {
	Box,
	Checkbox,
	ListItem,
	ListSubheader,
	MenuItem,
	Select,
	TableRow,
	TextField,
} from "@mui/material";
import { useAtom, useAtomValue } from "jotai";
import {
	type BaseSyntheticEvent,
	type FC,
	Fragment,
	memo,
	useState,
} from "react";
import { Controller, useForm } from "react-hook-form";
import { WorkTablesAtom, useWorkColumn } from "../../stores/TableStore";
import type { TableBaseProps, TableDefineProps } from "../../types/table";
import type {
	ForeignKey,
	TableColumn,
	WorkForeignKey,
} from "../../utils/table";
import {
	EditorButton,
	EditorCell,
	EditorCheckbox,
	EditorSelect,
	EditorTextField,
} from "./editor";

const DatabaseTypeMap = new Map([
	// 通常
	["integer", "integer"],
	["real", "real"],
	["text", "text"],
	["blob", "blob"],
	// 意味だけ
	["datetime", "text"],
	["boolean", "integer"],
]) as ReadonlyMap<string, string>;

const ClrMap = new Map([
	["integer", ["System.Int64"]],
	["real", ["System.Decimal", "System.Single", "System.Double"]],
	[
		"text",
		["System.String", "System.Guid", "System.Version", "System.TimeSpan"],
	],
	["blob", ["System.Byte[]"]],
	["datetime", ["System.DateTime", "System.String"]],
	["boolean", ["System.Boolean", "System.Int64"]],
]) as ReadonlyMap<string, ReadonlyArray<string>>;

// const CommonColumns: ReadonlyArray<string> = [
// 	"CreatedTimestamp",
// 	"CreatedAccount",
// 	"CreatedProgramName",
// 	"CreatedProgramVersion",
// 	"UpdatedTimestamp",
// 	"UpdatedAccount",
// 	"UpdatedProgramName",
// 	"UpdatedProgramVersion",
// 	"UpdatedCount",
// ];

interface InputValues {
	isPrimary: boolean;
	notNull: boolean;
	logicalName: string;
	logicalType: string;
	physicalName: string;
	cliType: string;
	checkConstraints: string;
	comment: string;
}

interface DatabaseTableColumnProps extends TableBaseProps {
	columnId: string;
	columnsLastUpdateTimestamp: number;
}

export const DatabaseTableColumn: FC<DatabaseTableColumnProps> = memo(
	function DatabaseTableColumn(props: DatabaseTableColumnProps) {
		const { tableId, columnId } = props;
		const { workColumn, updateWorkColumn } = useWorkColumn(tableId, columnId);
		const {
			id,
			isPrimary,
			notNull,
			foreignKeyId,
			logical,
			physicalName,
			cliType,
			checkConstraints,
			comment,
		} = workColumn;

		const workTables = useAtomValue(WorkTablesAtom);

		const foreignTables = workTables.filter((a) => a.id !== tableId);

		const foreignTable = foreignKeyId
			? foreignTables.find((a) => a.id === foreignKeyId.tableId)
			: undefined;
		const foreignColumn =
			foreignTable && foreignKeyId
				? foreignTable.columns.items.find((a) => a.id === foreignKeyId.columnId)
				: undefined;

		const { control, handleSubmit } = useForm<InputValues>({
			mode: "onBlur",
			reValidateMode: "onChange",
			defaultValues: {
				isPrimary: isPrimary,
				notNull: notNull,
				logicalName: logical.name,
				logicalType: logical.type,
				physicalName: physicalName,
				cliType: cliType,
				checkConstraints: checkConstraints,
				comment: comment,
			},
		});

		function handleInput(
			data: InputValues,
			event?: BaseSyntheticEvent<object>,
		): void {
			let foreignKey: ForeignKey | undefined = undefined;
			let foreignKeyId: WorkForeignKey | undefined = undefined;
			if (foreignTable && foreignColumn) {
				foreignKey = {
					table: foreignTable.define.tableName,
					column: foreignColumn.physicalName,
				};
				foreignKeyId = {
					tableId: foreignTable.id,
					columnId: foreignColumn.id,
				};
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
				cliType: data.cliType,
				checkConstraints: data.checkConstraints,
				comment: data.comment,
			});
		}

		return (
			<TableRow>
				<EditorCell>
					<Controller
						name="isPrimary"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorCheckbox {...field} onBlur={handleSubmit(handleInput)} />
						)}
					/>
				</EditorCell>
				<EditorCell>
					<Controller
						name="notNull"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorCheckbox {...field} onBlur={handleSubmit(handleInput)} />
						)}
					/>
				</EditorCell>
				<EditorCell>
					<EditorButton>
						{foreignTable && foreignColumn
							? `${foreignTable.define.tableName}.${foreignColumn.physicalName}`
							: "未選択"}
					</EditorButton>
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
								<ListSubheader>SQLite3</ListSubheader>
								<MenuItem value="integer">integer</MenuItem>
								<MenuItem value="real">real</MenuItem>
								<MenuItem value="text">text</MenuItem>
								<MenuItem value="blob">blob</MenuItem>
								<ListSubheader>affinity</ListSubheader>
								<MenuItem value="datetime">datetime</MenuItem>
								<MenuItem value="boolean">boolean</MenuItem>
							</EditorSelect>
						)}
					/>
				</EditorCell>
				<EditorCell>
					<EditorTextField />
				</EditorCell>
				<EditorCell>
					<Controller
						name="cliType"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorSelect {...field} onBlur={handleSubmit(handleInput)}>
								<MenuItem value="System.String">string</MenuItem>
								<MenuItem value="System.Int64">long</MenuItem>
								<MenuItem value="System.Decimal">decimal</MenuItem>
								<MenuItem value="System.Byte[]">byte[]</MenuItem>
								<MenuItem value="System.Boolean">bool</MenuItem>
								<MenuItem value="System.Single">float</MenuItem>
								<MenuItem value="System.Double">double</MenuItem>
								<MenuItem value="System.Guid">Guid</MenuItem>
								<MenuItem value="System.DateTime">DateTime</MenuItem>
								<MenuItem value="System.Version">Version</MenuItem>
								<MenuItem value="System.TimeSpan">TimeSpan</MenuItem>
							</EditorSelect>
						)}
					/>
				</EditorCell>
				<EditorCell>
					<Controller
						name="checkConstraints"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
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
				<EditorCell>delete</EditorCell>
			</TableRow>
		);
	},
);
