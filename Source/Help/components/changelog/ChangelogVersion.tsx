import {
	Box,
	List,
	ListItem,
	Stack,
	Typography,
	useTheme,
} from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";
import { splitVersionInfos } from "../../utils/changelog";
import { ChangelogContent } from "./ChangelogContent";
import { ChangelogVersionInfo } from "./ChangelogVersionInfo";

interface ChangelogVersionProps extends changelog.ChangelogVersion {
	prevVersion?: string;
}

export const ChangelogVersion: FC<ChangelogVersionProps> = (
	props: ChangelogVersionProps,
) => {
	const { date, contents, version, prevVersion } = props;
	const theme = useTheme();

	const versionInfos = splitVersionInfos(version);

	return (
		<Box id={version}>
			<Typography
				variant="h1"
				sx={{
					padding: "0.2ex 0.5ex",
					marginBlock: "1rem",
					background: theme.palette.primary.light,
					color: theme.palette.primary.contrastText,
					fontSize: "18pt",
					fontWeight: "bold",
					lineHeight: "1.5em",
				}}
			>
				<Typography
					component="time"
					dateTime={date}
					sx={{ fontSize: "18pt", fontWeight: "bold", lineHeight: "1.5em" }}
				>
					{date}
				</Typography>
				,
				<List component={Stack} direction="row" sx={{ display: "inline" }}>
					{versionInfos.map((a) => (
						<ListItem key={a.value} sx={{ display: "inline" }}>
							<ChangelogVersionInfo version={a} />
						</ListItem>
					))}
				</List>
			</Typography>

			{contents.map((a, i) => (
				// biome-ignore lint/suspicious/noArrayIndexKey: キーがねぇ
				<ChangelogContent key={i} {...a} />
			))}
		</Box>
	);
};
