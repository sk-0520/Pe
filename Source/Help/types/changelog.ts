type Decimal = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";
type DateYear =
	`20${Exclude<Decimal, "0" | "4" | "5" | "6" | "7" | "8" | "9">}${Decimal}`;
type DateMonth = `0${Exclude<Decimal, "0">}` | `1${"0" | "1" | "2"}`;
type DateDay =
	| `0${Exclude<Decimal, "0">}`
	| `${"1" | "2"}${Decimal}`
	| `3${"0" | "1"}`;
type VersionDate = `${DateYear}/${DateMonth}/${DateDay}`;
export const DevelopmentVersionDate = "YYYY/MM/DD";
export type ChangelogDate =
	| typeof DevelopmentVersionDate
	| VersionDate
	| Array<VersionDate>;

type ChangelogVersionNumberOld =
	| `0.${Exclude<Decimal, "8" | "9">}${Decimal}.${Decimal}`
	| `0.8${Extract<Decimal, "0" | "1" | "2" | "3">}.${Decimal}`;
type ChangelogVersionNumber84 =
	| `0.8${Extract<Decimal, "4" | "5" | "6" | "7" | "8" | "9">}.${Decimal}${Decimal}${Decimal}`
	| `0.9${Decimal}.${Decimal}${Decimal}${Decimal}`;
export type ChangelogVersionNumber =
	| ChangelogVersionNumberOld
	| ChangelogVersionNumber84
	| `${ChangelogVersionNumber84}+`;
export type ChangelogVersionNumbers =
	| ChangelogVersionNumber
	| Array<ChangelogVersionNumber>;

export const ChangelogContentKinds = [
	"note",
	"features",
	"fixes",
	"developer",
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
	version: ChangelogVersionNumbers;
	group?: string;
	contents: ChangelogContent[];
}
