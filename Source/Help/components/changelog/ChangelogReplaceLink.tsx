import { Link } from "@mui/material";
import { type FC, Fragment, type ReactNode } from "react";
import { convertTagFromVersion, splitTokens } from "@/Help/utils/changelog";

const IssueLink = "https://github.com/sk-0520/Pe/issues/";
const DiffLink = "https://github.com/sk-0520/Pe/compare/";

interface ChangelogReplaceLinkProps {
	children: ReactNode | string;
	diff?: {
		prev: string;
		current: string;
	};
}

export const ChangelogReplaceLink: FC<ChangelogReplaceLinkProps> = (
	props: ChangelogReplaceLinkProps,
) => {
	const { children, diff } = props;

	if (diff) {
		const prevTag = convertTagFromVersion(diff.prev);
		const currentTag = convertTagFromVersion(diff.current);
		const link = `${DiffLink + prevTag}...${currentTag}`;
		return (
			<Link href={link} target="_blank">
				{children}
			</Link>
		);
	}

	if (typeof children !== "string") {
		throw new Error(`children => ${typeof children}`);
	}

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
					<Link key={i} href={IssueLink + a.value} target="_blank">
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
