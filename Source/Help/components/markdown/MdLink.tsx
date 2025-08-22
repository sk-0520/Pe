import type { FC } from "react";
import { PageLink } from "../../components/PageLink";
import type { PageKey } from "../../page";

interface MdLinkProps {
	page: PageKey;
}

export const MdLink: FC<MdLinkProps> = (props: MdLinkProps) => {
	const { page } = props;

	return <PageLink page={page} />;
};
