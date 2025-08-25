import type {
	ChangelogDate,
	ChangelogVersionNumber,
	ChangelogVersionNumbers,
} from "../types/changelog";
import { getElement } from "./access";

export type Kind = "text" | "issue" | "url";

export interface Token {
	kind: Kind;
	value: string;
}

const IssueRegex = /(^#(?<ISSUE>\d+))/;
const UrlRegex = /^(?<URL>(https?:\/\/[\w?=&./\-;#~%]+(?![\w?&./;#~%"=-]*>)))/;

export function toDateLabel(date: ChangelogDate): string {
	if (Array.isArray(date)) {
		return date.join(", ");
	}

	return date;
}

export function toVersionLabel(version: ChangelogVersionNumbers): string {
	if (Array.isArray(version)) {
		return version.join("-");
	}

	return version;
}

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

export function getFirstVersion(
	version: ChangelogVersionNumbers,
): ChangelogVersionNumber {
	if (Array.isArray(version)) {
		return getElement(version, 0);
	}

	return version;
}

export function getLastVersion(
	version: ChangelogVersionNumbers,
): ChangelogVersionNumber {
	if (Array.isArray(version)) {
		return getElement(version, version.length - 1);
	}

	return version;
}

const TagVersion = {
	minimum: "0.84.000",
	maximum: "0.99.095",
	regex: /^(?<MAJOR>\d+)\.(?<MINOR>\d+)\.(?<BUILD>\d+)$/,
} as const;

export function convertTagFromVersion(version: string): string {
	if (TagVersion.minimum <= version && version <= TagVersion.maximum) {
		const result = TagVersion.regex.exec(version);
		if (result?.groups) {
			const major = result.groups.MAJOR;
			const minor = result.groups.MINOR;
			const build = Number.parseInt(result.groups.BUILD ?? "", 10);
			return `${major}.${minor}.${build}`;
		}
	}

	return version;
}

export function toHtmlId(version: ChangelogVersionNumbers): string {
	if (Array.isArray(version)) {
		return version.join("-");
	}

	return version;
}

export function splitVersion(
	version: ChangelogVersionNumbers,
): ChangelogVersionNumber[] {
	if (Array.isArray(version)) {
		return version;
	}

	return [version];
}
