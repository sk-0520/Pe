import { Box, TableBody, TableHead, TableRow } from "@mui/material";
import type { FC, MouseEvent } from "react";
import { useWorkIndexes, useWorkTable } from "@/Help/stores/TableStore";
import type { TableBaseProps } from "@/Help/types/table";
import { generateIndexesId } from "@/Help/utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";
import { DatabaseTableSection } from "./DatabaseTableSection";
import { EditorButton, EditorCell, EditorTable } from "./editor";

interface DatabaseTableIndexesProps extends TableBaseProps {}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	const { tableId } = props;

	const { workTable } = useWorkTable(tableId);
	const { workIndexes, updateWorkIndexes } = useWorkIndexes(tableId);

	const handleAddIndex = (event: MouseEvent) => {
		workIndexes.items.push({
			id: generateIndexesId(),
			isUnique: false,
			name: `idx_${workTable.define.tableName}_${workIndexes.items.length + 1}`,
			columns: [],
			columnIds: [],
		});
		updateWorkIndexes({
			...workIndexes,
		});
	};

	return (
		<DatabaseTableSection title="インデックス">
			{
				<EditorTable>
					<TableHead>
						<TableRow>
							<EditorCell>削除</EditorCell>
							<EditorCell>UK</EditorCell>
							<EditorCell>物理名</EditorCell>
							<EditorCell>カラム</EditorCell>
						</TableRow>
					</TableHead>
					<TableBody>
						{workIndexes.items.map((a) => (
							<DatabaseTableIndex
								key={a.id}
								tableId={tableId}
								indexId={a.id}
							/>
						))}
						<TableRow>
							<EditorCell colSpan={11}>
								<Box sx={{ textAlign: "center" }}>
									<EditorButton onClick={handleAddIndex}>
										インデックス追加
									</EditorButton>
								</Box>
							</EditorCell>
						</TableRow>
					</TableBody>
				</EditorTable>
			}
		</DatabaseTableSection>
	);
};
