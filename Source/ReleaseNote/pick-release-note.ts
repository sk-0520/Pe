import fs from "node:fs";
import type { ChangelogVersion } from "../Help/types/changelog";
import { getValue } from "../Help/utils/access";
import { splitVersionInfos } from "../Help/utils/changelog";

export interface Input {
	changelogsPath: string;
	outputChangelogPath: string;
}

export function main(input: Input) {
	console.debug({ input });

	const changelogsJson = fs.readFileSync(input.changelogsPath).toString();
	const changelogs = JSON.parse(changelogsJson);
	const changelog = getValue(changelogs, 0) as object & {
		prevVersion: string | undefined;
	};

	changelog.prevVersion = splitVersionInfos(
		(getValue(changelogs, 1) as ChangelogVersion).version,
	)[0]?.value;

	fs.writeFileSync(input.outputChangelogPath, JSON.stringify(changelog));
}
