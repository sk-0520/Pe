import { Box, Toolbar } from "@mui/material";
import type { FC } from "react";
import { ChangelogVersion } from "../../components/changelog/ChangelogVersion";
import { ChangelogVersionSelector } from "../../components/changelog/ChangelogVersionSelector";
import type { PageProps } from "../../types/page";
import { getChangelogs } from "../../utils/changelog-loader";

const changelogs = getChangelogs();

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
				<ChangelogVersionSelector changelogs={changelogs} />
			</Toolbar>
			<Box>
				{changelogs.map((a, i) => {
					return (
						<ChangelogVersion
							key={a.version}
							{...a}
							prevVersion={changelogs[i + 1]?.version}
						/>
					);
				})}
			</Box>
		</Box>
	);
};
