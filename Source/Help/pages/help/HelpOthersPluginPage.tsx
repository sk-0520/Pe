import { default as markdown } from "bundle-text:./HelpOthersPluginPage.md";
import type { FC } from "react";
import { HelpMarkdown } from "@/Help/components/HelpMarkdown";
import type { PageProps } from "@/Help/types/page";

export const HelpOthersPluginPage: FC<PageProps> = (props: PageProps) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};
