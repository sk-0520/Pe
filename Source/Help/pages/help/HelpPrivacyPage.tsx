import { default as markdown } from "bundle-text:./HelpPrivacyPage.md";
import type { FC } from "react";
import { HelpMarkdown } from "@/Help/components/HelpMarkdown";
import type { PageProps } from "@/Help/types/page";

export const HelpPrivacyPage: FC<PageProps> = (props: PageProps) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};
