import { Link, Typography } from "@mui/material";
import type { FC, MouseEvent } from "react";
import { type PageKey, Pages } from "../pages";
import { useSideMenuStore } from "../stores/SideMenuStore";
import { getPage, makeUrl } from "../utils/page";

interface PageLinkProps {
	page: PageKey;
}

export const PageLink: FC<PageLinkProps> = (props: PageLinkProps) => {
	const { page } = props;

	const setPageKey = useSideMenuStore((a) => a.setPageKey);

	function handleLinkClick(ev: MouseEvent, pageKey: PageKey, url: URL): void {
		ev.preventDefault();
		setPageKey(pageKey);
		history.pushState({}, "", url);
	}

	try {
		const url = makeUrl(page);
		const pageElement = getPage(page, Pages);

		return (
			<Link
				href={url.href}
				onClick={(ev) => handleLinkClick(ev, page, url)}
			>
				{pageElement.title}
			</Link>
		);
	} catch (_ex: unknown) {
		return <Typography color="error">{page}</Typography>;
	}
};
