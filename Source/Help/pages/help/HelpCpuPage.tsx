import { default as markdown } from "bundle-text:./HelpCpuPage.md";
import type { FC } from "react";
import { HelpMarkdown } from "@/Help/components/HelpMarkdown";
import type { PageProps } from "@/Help/types/page";

export const HelpCpuPage: FC<PageProps> = (props: PageProps) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};
