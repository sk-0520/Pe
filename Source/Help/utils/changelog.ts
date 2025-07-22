import { getElement } from "./access";

export type Kind = "text" | "issue" | "url";

export interface Token {
	kind: Kind;
	value: string;
}

const IssueRegex = /(^#(?<ISSUE>\d+))/;
const UrlRegex = /^(?<URL>(https?:\/\/[\w?=&./\-;#~%]+(?![\w?&./;#~%"=-]*>)))/;

export function splitTokens(s: string): Token[] {
	const buffer: Token[] = [];

	let currentIndex = 0;
	while (currentIndex < s.length) {
		const work = s.substring(currentIndex);

		const issueMatch = work.match(IssueRegex);
		if (issueMatch?.groups && "ISSUE" in issueMatch.groups) {
			buffer.push({
				kind: "issue",
				value: issueMatch.groups.ISSUE,
			});
			currentIndex +=
				issueMatch.groups.ISSUE.length + 1 /* #の分を追加 */;
			continue;
		}

		const urlMatch = work.match(UrlRegex);
		if (urlMatch?.groups && "URL" in urlMatch.groups) {
			buffer.push({
				kind: "url",
				value: urlMatch.groups.URL,
			});
			currentIndex += urlMatch.groups.URL.length;
			continue;
		}

		buffer.push({
			kind: "text",
			value: work.substring(0, 1),
		});

		currentIndex += 1;
	}

	const result: Token[] = [];
	for (let i = 0; i < buffer.length; i++) {
		if (i) {
			const work = getElement(result, result.length - 1);
			// @ts-expect-error ts(2345)
			if (work.kind === "text" && buffer[i].kind === "text") {
				// @ts-expect-error ts(2345)
				work.value += buffer[i].value;
			} else {
				// @ts-expect-error ts(2345)
				result.push(buffer[i]);
			}
		} else {
			// @ts-expect-error ts(2345)
			result.push(buffer[i]);
		}
	}

	return result;
}

export function selectDateTime(rawDate: string): string {
	const dateItems = rawDate
		.split(",")
		.map((a) => a.trim())
		.filter((a) => a.length)
		.filter((a) => /\d{4}\/\d{2}\/\d{2}/.test(a));

	const dateItem = dateItems.pop();
	if (dateItem) {
		const y = dateItem.substring(0, 4);
		const m = dateItem.substring(5, 7);
		const d = dateItem.substring(8, 10);
		return `${y}-${m}-${d}T00:00:00+09:00`;
	}
	return new Date().toISOString();
}

export interface VersionInfo {
	value: string;
	isVersion: boolean;
}

/**
 * バージョン正規表現。
 *
 * 末尾 + はどうにも正しいタグなんよ。かなしみ。
 */
const VersionRegex = /^(?<VERSION>\d+\.\d+\.\d+(\+?))$/;

export function splitVersionInfos(rawVersion: string): VersionInfo[] {
	if (!rawVersion.length) {
		return [];
	}

	const result: VersionInfo[] = [];

	const rawVersions = rawVersion
		.split(",")
		.map((a) => a.trim())
		.filter((a) => a.length);

	for (const s of rawVersions) {
		const execResult = VersionRegex.exec(s);
		if (execResult?.groups && "VERSION" in execResult.groups) {
			result.push({
				value: execResult.groups.VERSION,
				isVersion: true,
			});
		} else {
			result.push({
				value: s,
				isVersion: false,
			});
		}
	}

	return result;
}

const TagVesion = {
	minimum: "0.84.000",
	maximum: "0.99.095",
	regex: /^(?<MAJOR>\d+)\.(?<MINOR>\d+)\.(?<BUILD>\d+)$/,
} as const;

export function convertTagFromVersion(version: string): string {
	if (TagVesion.minimum <= version && version <= TagVesion.maximum) {
		const result = TagVesion.regex.exec(version);
		if (result?.groups) {
			const major = result.groups.MAJOR;
			const minor = result.groups.MINOR;
			const build = Number.parseInt(result.groups.BUILD ?? "", 10);
			return `${major}.${minor}.${build}`;
		}
	}

	return version;
}
