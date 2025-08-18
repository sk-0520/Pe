import {
	Button,
	type ButtonProps,
	Checkbox,
	type CheckboxProps,
	Select,
	type SelectProps,
	TableCell,
	type TableCellProps,
	type TableProps,
	TextField,
	type TextFieldProps,
} from "@mui/material";
import { StickyTable } from "@/Help/components/StickyTable";

export function EditorTable(props: TableProps) {
	return <StickyTable stickyHeader size="small" {...props} />;
}

export function EditorCell(props: TableCellProps) {
	return <TableCell size="small" {...props} />;
}

export function EditorCheckbox(props: CheckboxProps) {
	return <Checkbox size="small" {...props} />;
}

export function EditorTextField(props: TextFieldProps) {
	return <TextField size="small" variant="standard" fullWidth {...props} />;
}

export function EditorSelect(props: SelectProps) {
	return <Select size="small" variant="standard" fullWidth {...props} />;
}

export function EditorButton(props: ButtonProps) {
	return <Button variant="outlined" size="small" {...props} />;
}
