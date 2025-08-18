import type { FC } from "react";
import { PageLink } from "@/Help/components/PageLink";
import type { PageKey } from "@/Help/pages";

interface MdLinkProps {
	page: PageKey;
}

export const MdLink: FC<MdLinkProps> = (props: MdLinkProps) => {
	const { page } = props;

	return <PageLink page={page} />;
};
