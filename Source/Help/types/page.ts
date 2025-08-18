import type { PageElement, PageKey } from "@/Help/pages";

export interface PageProps {
	selectedPageKey: PageKey;
	callbackSelectPageKey: (pageKey: PageKey) => void;
	currentPage: PageElement;
}
