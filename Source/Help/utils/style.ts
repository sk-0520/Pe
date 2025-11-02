import type { CSSProperties } from "react";

export function toCssFontFamily(
	fontFamilies: ReadonlyArray<string>,
): CSSProperties["fontFamily"] {
	return fontFamilies.map((a) => (a.includes(" ") ? `"${a}"` : a)).join(",");
}

export const MonospaceFontFamily = toCssFontFamily(["Consolas", "monospace"]);
