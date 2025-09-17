import { Link } from "@mui/material";
import { type FC, Fragment, type ReactNode } from "react";
import {
	convertTagFromVersion,
	splitTokens,
	type Token,
} from "../../utils/changelog";

const IssueLink = "https://github.com/sk-0520/Pe/issues/";
const DiffLink = "https://github.com/sk-0520/Pe/compare/";

interface ChangelogReplaceLinkCoreProps {
	token: Token;
}
const ChangelogReplaceLinkCore: FC<ChangelogReplaceLinkCoreProps> = (
	props: ChangelogReplaceLinkCoreProps,
) => {
	const { token } = props;

	switch (token.kind) {
		case "text":
			return <Fragment>{token.value}</Fragment>;
		case "issue":
			return (
				<Link href={IssueLink + token.value} target="_blank">
					#{token.value}
				</Link>
			);
		case "url":
			return (
				<Link href={token.value} target="_blank">
					{token.value}
				</Link>
			);
	}
};

export interface ChangelogReplaceLinkProps {
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
		// biome-ignore lint/suspicious/noArrayIndexKey: key がねぇ
		return <ChangelogReplaceLinkCore key={i} token={a} />;
	});
};
