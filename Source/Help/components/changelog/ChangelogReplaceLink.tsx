import { Link } from "@mui/material";
import { type FC, Fragment } from "react";
import { splitTokens } from "../../utils/changelog";

const issueLink = "https://github.com/sk-0520/Pe/issues/";

interface ChangelogReplaceLinkProps {
	children: string;
}

export const ChangelogReplaceLink: FC<ChangelogReplaceLinkProps> = (
	props: ChangelogReplaceLinkProps,
) => {
	const { children } = props;

	const tokens = splitTokens(children);

	if (!tokens) {
		return children;
	}

	return tokens.map((a, i) => {
		switch (a.kind) {
			case "text":
				// biome-ignore lint/suspicious/noArrayIndexKey: key がねぇ
				return <Fragment key={i}>{a.value}</Fragment>;
			case "issue":
				return (
					// biome-ignore lint/suspicious/noArrayIndexKey: key がねぇ
					<Link key={i} href={issueLink + a.value} target="_blank">
						#{a.value}
					</Link>
				);
			case "url":
				return (
					// biome-ignore lint/suspicious/noArrayIndexKey: key がねぇ
					<Link key={i} href={a.value} target="_blank">
						{a.value}
					</Link>
				);
		}
	});
};
