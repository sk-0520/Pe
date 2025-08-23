import DifferenceIcon from "@mui/icons-material/Difference";
import {
	Box,
	List,
	ListItem,
	Stack,
	type SxProps,
	styled,
	type Theme,
	Typography,
	useTheme,
} from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";
import { splitVersionInfos, toDateLabel } from "../../utils/changelog";
import { ChangelogContent } from "./ChangelogContent";
import { ChangelogReplaceLink } from "./ChangelogReplaceLink";

const FirstCommit = "275d6b5f1a41dcb99d56e6448b9249236cdd75c0";

const StyledVersionListItem = styled(ListItem)({
	display: "inline",
	margin: 0,
	padding: 0,
});

const HeaderStyle: SxProps<Theme> = {
	fontSize: "18pt",
	fontWeight: "bold",
	lineHeight: "1.5em",
};

interface ChangelogVersionProps extends changelog.ChangelogVersion {
	prevVersion?: string;
}

export const ChangelogVersion: FC<ChangelogVersionProps> = (
	props: ChangelogVersionProps,
) => {
	const { date, contents, version, prevVersion } = props;
	const theme = useTheme();

	const versionInfos = splitVersionInfos(version);
	const versionCommit = versionInfos.findLast((a) => a.isVersion)?.value;
	const prevVersionCommit = prevVersion
		? (splitVersionInfos(prevVersion).findLast((a) => a.isVersion)?.value ??
			FirstCommit)
		: FirstCommit;

	return (
		<Box id={version}>
			<Typography
				variant="h1"
				sx={{
					padding: "0.2ex 0.5ex",
					marginBlock: "1rem",
					background: theme.palette.primary.light,
					color: theme.palette.primary.contrastText,
					...HeaderStyle,
				}}
			>
				<Typography
					component="time"
					dateTime={toDateLabel(date)}
					sx={HeaderStyle}
				>
					{toDateLabel(date)}
				</Typography>
				:
				<List
					component={Stack}
					direction="row"
					sx={{ display: "inline", marginLeft: "0.5ch" }}
				>
					{versionInfos.map((a, i) => (
						<StyledVersionListItem
							key={a.value}
							sx={
								i
									? {
											"&::before": {
												content: "'-'",
											},
										}
									: undefined
							}
						>
							{a.value}
						</StyledVersionListItem>
					))}
					{versionCommit !== undefined && (
						<StyledVersionListItem sx={{ marginLeft: "1ch" }}>
							<ChangelogReplaceLink
								diff={{
									prev: prevVersionCommit,
									current: versionCommit,
								}}
							>
								<DifferenceIcon sx={{ color: "white" }} />
							</ChangelogReplaceLink>
						</StyledVersionListItem>
					)}
				</List>
			</Typography>

			{contents.map((a, i) => (
				// biome-ignore lint/suspicious/noArrayIndexKey: キーがねぇ
				<ChangelogContent key={i} {...a} />
			))}
		</Box>
	);
};
