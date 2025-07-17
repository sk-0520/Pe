import { styled, Table, type TableProps } from "@mui/material";

export const StickyTable = styled((props: TableProps) => {
	const { sx, stickyHeader } = props;

	return (
		<Table
			sx={{
				...(sx ?? {}),
				"> thead th": stickyHeader
					? (theme) => ({
							top: theme.mixins.toolbar.minHeight,
						})
					: undefined,
			}}
			{...props}
		/>
	);
})();
