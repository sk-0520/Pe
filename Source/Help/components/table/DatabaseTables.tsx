import {
	Box,
	Divider,
	MenuItem,
	Select,
	type SelectChangeEvent,
	Stack,
	useTheme,
} from "@mui/material";
import { useAtom } from "jotai";
import {
	type FC,
	type MouseEvent,
	type ReactNode,
	useEffect,
	useState,
} from "react";
import { WorkTablesAtom } from "../../stores/TableStore";
import { getElement } from "../../utils/access";
import { copy } from "../../utils/clipboard";
import {
	convertDefineTable,
	convertTable,
	convertWorkTable,
	splitRawEntities,
	splitRawSection,
	toMarkdown,
	toSql,
	updateRelations,
} from "../../utils/table";
import { DatabaseTable } from "./DatabaseTable";
import { EditorButton } from "./editor";

interface DatabaseTablesProps {
	markdown: string;
}

export const DatabaseTables: FC<DatabaseTablesProps> = (
	props: DatabaseTablesProps,
) => {
	const { markdown } = props;
	const theme = useTheme();
	const [workTables, setWorkTables] = useAtom(WorkTablesAtom);
	const [selectedTableId, setSelectedTableId] = useState<string>("");

	// biome-ignore lint/correctness/useExhaustiveDependencies: 初期化のみ
	useEffect(() => {
		const tables = splitRawEntities(markdown)
			.map((a) => splitRawSection(a))
			//.sort((a, b) => a.table.localeCompare(b.table))
			.map((a) => convertTable(a))
			.map((a) => convertWorkTable(a));

		updateRelations(tables);
		setWorkTables(tables);
		setSelectedTableId(getElement(tables, 0).id);
	}, []);

	// const tables = useMemo(() => {
	// 	console.debug("memo!");

	// 	const tables = splitRawEntities(markdown)
	// 		.map((a) => splitRawSection(a))
	// 		//.sort((a, b) => a.table.localeCompare(b.table))
	// 		.map((a) => convertTable(a))
	// 		.map((a) => convertWorkTable(a));

	// 	updateRelations(tables);

	// 	return tables;
	// }, [markdown]);
	// setWorkTables(tables);

	// useEffect(() => {
	// 	if (tables.length) {
	// 		setSelectedTableId(getValue(tables, 0).id);
	// 	}
	// }, [tables]);

	// useEffect(() => {
	// 	if (workTables.length) {
	// 		setSelectedTableId(getValue(workTables, 0).id);
	// 	}
	// }, [workTables]);

	if (!workTables.length || !selectedTableId) {
		return <>...loading...</>;
	}

	function handleChange(
		event: SelectChangeEvent<string>,
		child: ReactNode,
	): void {
		setSelectedTableId(event.target.value);
	}

	async function handleCopyMarkdownClick(event: MouseEvent): Promise<void> {
		const defineTables = workTables.map((a) => convertDefineTable(a));
		const markdown = toMarkdown(defineTables);
		await copy(markdown);
	}

	async function handleCopySqlClick(event: MouseEvent): Promise<void> {
		const defineTables = workTables.map((a) => convertDefineTable(a));
		const sql = toSql(defineTables);
		await copy(sql);
	}

	return (
		<Box>
			<Box>
				<Select
					fullWidth
					value={selectedTableId}
					onChange={handleChange}
					sx={{
						background: theme.palette.primary.contrastText,
						color: theme.palette.primary.dark,
						fontWeight: "bold",
						"& .MuiOutlinedInput-notchedOutline": {
							borderRadius: 0,
						},
					}}
				>
					{workTables.map((a) => (
						<MenuItem key={a.id} value={a.id}>
							{a.define.tableName}
						</MenuItem>
					))}
				</Select>
			</Box>

			<Divider sx={{ marginBlock: "1em" }} />

			<Box>
				<DatabaseTable tableId={selectedTableId} />
			</Box>

			<Divider sx={{ marginBlock: "1em" }} />

			<Box sx={{ marginTop: "1em" }}>
				<Stack direction="row" spacing={1}>
					<EditorButton size="medium" onClick={handleCopyMarkdownClick}>
						コピー: Markdown
					</EditorButton>
					<EditorButton size="medium" onClick={handleCopySqlClick}>
						コピー: SQL
					</EditorButton>
				</Stack>
			</Box>
			{/*
			<Box
				sx={{
					position: "fixed",
					zIndex: 999999,
					right: "3ch",
					//bottom: 0,
					top: 0,
					height: "25vh",
					width: "40vw",
					overflow: "auto",
					background: "gray",
					fontSize: 10,
					fontFamily: "monospace",
				}}
			>
				<pre>{JSON.stringify(workTables, undefined, 2)}</pre>
			</Box>
			*/}
		</Box>
	);
};
