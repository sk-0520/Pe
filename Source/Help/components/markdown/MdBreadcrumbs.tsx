import { Box, Typography } from "@mui/material";
import { type FC, Fragment } from "react";
import { MdInline } from "./MdInline";

function renderItems(kind: MdBreadcrumbsProps["kind"], items: string[]) {
	switch (kind) {
		case "ui": {
			return (
				<Box
					component="span"
					aria-label="breadcrumb"
					sx={{
						display: "inline-block",
						color: "text.primary",
					}}
				>
					{items.map((a, i) => (
						// biome-ignore lint/suspicious/noArrayIndexKey: a は一意足りえない
						<Fragment key={`${a}-${i}`}>
							{i !== 0 ? (
								<Typography component="span">→</Typography>
							) : null}
							<MdInline kind={kind}>{a}</MdInline>
						</Fragment>
					))}
				</Box>
			);
		}
	}
}

interface MdBreadcrumbsProps {
	items: string;
	separator?: "/";
	kind: "ui";
}

export const MdBreadcrumbs: FC<MdBreadcrumbsProps> = (
	props: MdBreadcrumbsProps,
) => {
	const { kind } = props;
	const separator = props.separator ?? "/";
	const items = props.items
		.split(separator)
		.map((a) => a.trim())
		.filter((a) => a.length);

	return renderItems(kind, items);
};
