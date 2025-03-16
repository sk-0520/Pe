import AnnouncementOutlinedIcon from "@mui/icons-material/AnnouncementOutlined";
import LightbulbOutlinedIcon from "@mui/icons-material/LightbulbOutlined";
import { Alert, AlertTitle, styled } from "@mui/material";
import type { FC, ReactNode } from "react";

const AlertKinds = ["NOTE", "TIP", "IMPORTANT", "WARNING", "CAUTION"] as const;
type AlertKind = (typeof AlertKinds)[number];

const AlertDisplays: { [key in AlertKind]: string } = {
	NOTE: "ノート",
	TIP: "TIPS",
	IMPORTANT: "重要",
	WARNING: "警告",
	CAUTION: "注意",
};

const StyledAlert = styled(Alert)({
	marginBlock: "1em",
	fontSize: "1rem",

	// ".MuiAlert-message": {
	// 	fontSize: "1rem",
	// },
});

interface MdAlertProps {
	kind: AlertKind;
	children: ReactNode;
}

export const MdAlert: FC<MdAlertProps> = (props: MdAlertProps) => {
	const { kind, children } = props;

	const alertChildren = (
		<>
			<AlertTitle sx={{ fontWeight: "bold" }}>{AlertDisplays[kind]}</AlertTitle>
			{children}
		</>
	);

	switch (kind) {
		case "NOTE":
			return <Alert severity="info">{alertChildren}</Alert>;

		case "TIP":
			return (
				<StyledAlert
					severity="info"
					icon={<LightbulbOutlinedIcon htmlColor="green" fontSize="inherit" />}
					sx={{
						background: "#CBFFD3",
					}}
				>
					{alertChildren}
				</StyledAlert>
			);

		case "IMPORTANT":
			return (
				<StyledAlert
					severity="info"
					icon={
						<AnnouncementOutlinedIcon htmlColor="purple" fontSize="inherit" />
					}
					sx={{
						background: "#DCC2FF",
					}}
				>
					{alertChildren}
				</StyledAlert>
			);

		case "WARNING":
			return <StyledAlert severity="warning">{alertChildren}</StyledAlert>;

		case "CAUTION":
			return <StyledAlert severity="error">{alertChildren}</StyledAlert>;
	}
};
