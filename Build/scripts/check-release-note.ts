import Changelogs from "../../Define/changelogs";
import { DevelopmentVersionDate } from "../../Source/Help/types/changelog";
import { getElement } from "../../Source/Help/utils/access";

export interface Input {
	rootDirPath: string;
}

export interface Options {
	isRelease: boolean;
}

const Revision = /([0-f][0-f]){,40}/;

export function main(input: Input, options: Options) {
	const changelog = getElement(Changelogs, 0);

	if (!options.isRelease) {
		return;
	}

	if (changelog.date === DevelopmentVersionDate) {
		//throw new Error(JSON.stringify({ date: changelog.date }));
	}

	for (const { type, logs } of changelog.contents) {
		console.log({ type });
		for (const log of logs) {
			if (!log.subject.trim()) {
				throw new Error("subject");
			}

			if (log.revision) {
				if (!Revision.test(log.revision)) {
					throw new Error(
						`${log.subject}: revision: ${log.revision}`,
					);
				}
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
