import fs from "node:fs";
import { ChangelogVersionSchema } from "../../Source/Help/types/changelog";
import { getElement } from "../../Source/Help/utils/access";

export interface Input {
	rootDirPath: string;
	changelogsJsonPath: string;
}

export interface Options {
	isRelease: boolean;
}

export function main(input: Input, options: Options) {
	const changelogsJson = fs.readFileSync(input.changelogsJsonPath).toString();
	const changelogs = JSON.parse(changelogsJson);
	const changelog = ChangelogVersionSchema.parse(getElement(changelogs, 0));

	if (!options.isRelease) {
		return;
	}

	for (const { type, logs } of changelog.contents) {
		console.log({ type });
		for (const log of logs) {
			if (!log.subject.trim()) {
				throw new Error("subject");
			}

			if (log.comments) {
				const hasError = log.comments
					.map((a) => a.trim())
					.filter((a) => !a).length;
				if (hasError) {
					throw new Error(`${log.subject}: comments`);
				}
			}
		}
	}
}
