type Number = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";
type DateYear =
	`20${Exclude<Number, "0" | "4" | "5" | "6" | "7" | "8" | "9">}${Number}`;
type DateMonth = `0${Exclude<Number, "0">}` | `1${"0" | "1" | "2"}`;
type DateDay =
	| `0${Exclude<Number, "0">}`
	| `${"1" | "2"}${Number}`
	| `3${"0" | "1"}`;
type VersionDate = `${DateYear}/${DateMonth}/${DateDay}`;
export const DevelopmentVersionDate = "YYYY/MM/DD";
export type ChangelogDate =
	| typeof DevelopmentVersionDate
	| VersionDate
	| Array<VersionDate>;

export type DevelopmentVersionNumber = `${string}+`;
export type ChangelogVersionNumber = DevelopmentVersionNumber | string;
export type ChangelogVersionNumbers =
	| ChangelogVersionNumber
	| Array<ChangelogVersionNumber>;

export const ChangelogContentKinds = [
	"features",
	"fixes",
	"developer",
	"note",
] as const;

export type ChangelogContentKind = (typeof ChangelogContentKinds)[number];

export const ChangelogContentItemTypes = [
	"compatibility",
	"notice",
	"nuget",
	"myget",
	"plugin-compatibility",
] as const;

export type ChangelogContentItemType =
	(typeof ChangelogContentItemTypes)[number];

export interface ChangelogContentItem {
	revision?: string;
	class?: ChangelogContentItemType;
	subject: string;
	comments?: string[];
}

export interface ChangelogContent {
	type: ChangelogContentKind;
	logs: ChangelogContentItem[];
}

export interface ChangelogVersion {
	date: ChangelogDate;
	version: string;
	group?: string;
	contents: ChangelogContent[];
}

export type ChangelogVersions = ChangelogVersion[];
