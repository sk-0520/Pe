import type { PageElement, PageKey } from "../page";

export interface PageProps {
	selectedPageKey: PageKey;
	currentPage: PageElement;
	onSelectPageKey: (pageKey: PageKey) => void;
}
