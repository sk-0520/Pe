import { default as markdown } from "bundle-text:./HelpLauncherPage.md";
import type { FC } from "react";
import { HelpMarkdown } from "@/Help/components/HelpMarkdown";
import type { PageProps } from "@/Help/types/page";

export const HelpLauncherPage: FC<PageProps> = (props: PageProps) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};
