import { Box, Toolbar } from "@mui/material";
import type { FC } from "react";
import Changelogs from "../../../../Define/changelogs";
import { ChangelogVersion } from "../../components/changelog/ChangelogVersion";
import { ChangelogVersionSelector } from "../../components/changelog/ChangelogVersionSelector";
import type { PageProps } from "../../types/page";

export const HelpChangelogPage: FC<PageProps> = (props: PageProps) => {
	return (
		<Box>
			<Toolbar
				sx={{
					position: "fixed",
					top: 0,
					right: 0,
					zIndex: (theme) => theme.zIndex.drawer + 2,
				}}
			>
				<ChangelogVersionSelector changelogs={Changelogs} />
			</Toolbar>
			<Box>
				{Changelogs.map((a, i) => {
					return (
						<ChangelogVersion
							key={a.version}
							{...a}
							prevVersion={Changelogs[i + 1]?.version}
						/>
					);
				})}
			</Box>
		</Box>
	);
};
