import { Link } from "@mui/material";
import type { FC, ReactNode } from "react";
import { PageLink } from "../../components/PageLink";
import type { PageKey } from "../../page";

interface MdLinkLocalProps {
	page: PageKey;
}

interface MdLinkExternalProps {
	href: string;
	children: ReactNode;
}

type MdLinkProps = MdLinkLocalProps | MdLinkExternalProps;

export const MdLink: FC<MdLinkProps> = (props: MdLinkProps) => {
	if ("page" in props) {
		const { page } = props;
		return <PageLink page={page} />;
	}

	const { href, children } = props;
	return (
		<Link target={href} href={href}>
			{children}
		</Link>
	);
};
