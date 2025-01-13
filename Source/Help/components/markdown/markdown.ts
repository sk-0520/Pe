import type { MarkdownToJSX } from "markdown-to-jsx";
import type { ReactNode } from "react";

export type MarkdownRule = (
	//next: () => ReactChild,
	node: MarkdownToJSX.ParserResult,
	renderChildren: MarkdownToJSX.RuleOutput,
	state: MarkdownToJSX.State,
) => ReactNode | undefined;
