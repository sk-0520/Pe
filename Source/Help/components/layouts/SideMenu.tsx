import { Box, List } from "@mui/material";
import type { FC } from "react";
import { type PageKey, Pages } from "@/Help/pages";
import { SideMenuItem } from "./SideMenuItem";

interface SideMenuProps {
	selectedPageKey: PageKey;
	handleSelectPageKey: (pageKey: PageKey) => void;
}

export const SideMenu: FC<SideMenuProps> = (props: SideMenuProps) => {
	const { selectedPageKey, handleSelectPageKey } = props;

	return (
		<Box sx={{ overflow: "auto" }}>
			<List disablePadding>
				{Pages.map((a) => (
					<SideMenuItem
						key={a.key}
						selectedPageKey={selectedPageKey}
						callbackSelectPageKey={handleSelectPageKey}
						page={a}
						nestLevel={0}
					/>
				))}
			</List>
		</Box>
	);
};
