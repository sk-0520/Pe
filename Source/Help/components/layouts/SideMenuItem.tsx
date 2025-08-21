import { List, ListItemButton, Typography, useTheme } from "@mui/material";
import type { FC, MouseEvent } from "react";
import type { PageElement, PageKey } from "../../page";

interface SideMenuItemProps {
	selectedPageKey: PageKey;
	onSelectPageKey: (pageKey: PageKey) => void;
	page: PageElement;
	nestLevel: number;
}

export const SideMenuItem: FC<SideMenuItemProps> = (
	props: SideMenuItemProps,
) => {
	const { onSelectPageKey, page, nestLevel, selectedPageKey } = props;
	const theme = useTheme();

	const isSelected = selectedPageKey === page.key;

	const handleSelectMenu = (event: MouseEvent) => {
		onSelectPageKey(page.key);
	};

	return (
		<>
			<ListItemButton
				onClick={handleSelectMenu}
				sx={{
					background: isSelected
						? theme.palette.primary.light
						: undefined,
					color: isSelected
						? theme.palette.primary.contrastText
						: undefined,
					"&:hover": {
						background: isSelected
							? theme.palette.primary.main
							: undefined,
					},
				}}
			>
				<Typography
					sx={{
						fontWeight: isSelected ? "bold" : undefined,
						paddingLeft: nestLevel * 1.5,
					}}
				>
					{page.title}
				</Typography>
			</ListItemButton>
			{page.nodes && 0 < page.nodes.length && (
				<List disablePadding>
					{page.nodes.map((a) => (
						<SideMenuItem
							key={a.key}
							selectedPageKey={selectedPageKey}
							onSelectPageKey={onSelectPageKey}
							page={a}
							nestLevel={nestLevel + 1}
						/>
					))}
				</List>
			)}
		</>
	);
};
