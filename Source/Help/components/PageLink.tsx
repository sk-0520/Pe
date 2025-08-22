import { Link, Typography } from "@mui/material";
import { useAtom } from "jotai";
import type { FC, MouseEvent } from "react";
import { type PageKey, Pages } from "../page";
import { SelectedPageKeyAtom } from "../stores/SideMenuStore";
import { getPage, makeUrl } from "../utils/page";

interface PageLinkProps {
	page: PageKey;
}

export const PageLink: FC<PageLinkProps> = (props: PageLinkProps) => {
	const { page } = props;

	const [_, setSelectedPageKey] = useAtom(SelectedPageKeyAtom);

	const handleLinkClick = (ev: MouseEvent, pageKey: PageKey, url: URL) => {
		ev.preventDefault();
		setSelectedPageKey(pageKey);
		history.pushState({}, "", url);
	};

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
