import { Box, List } from "@mui/material";
import type { FC } from "react";
import { type PageKey, Pages } from "../../page";
import { SideMenuItem } from "./SideMenuItem";

interface SideMenuProps {
	selectedPageKey: PageKey;
	onSelectPageKey: (pageKey: PageKey) => void;
}

export const SideMenu: FC<SideMenuProps> = (props: SideMenuProps) => {
	const { selectedPageKey, onSelectPageKey } = props;

	return (
		<Box sx={{ overflow: "auto" }}>
			<List disablePadding>
				{Pages.map((a) => (
					<SideMenuItem
						key={a.key}
						selectedPageKey={selectedPageKey}
						onSelectPageKey={onSelectPageKey}
						page={a}
						nestLevel={0}
					/>
				))}
			</List>
		</Box>
	);
};
