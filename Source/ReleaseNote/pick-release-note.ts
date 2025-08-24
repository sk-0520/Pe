import fs from "node:fs";
import Changelogs from "../../Define/changelogs";
import type {
	ChangelogVersion,
	ChangelogVersionNumber,
} from "../Help/types/changelog";
import { getElement } from "../Help/utils/access";
import { getLastVersion } from "../Help/utils/changelog";

interface ChangelogVersionWithPrevVersion extends ChangelogVersion {
	prevVersion: ChangelogVersionNumber;
}

export interface Input {
	outputChangelogPath: string;
}

export function main(input: Input) {
	console.debug({ input });

	const latestChangelog = getElement(Changelogs, 0);
	const prevVersion = getElement(Changelogs, 1);

	const changelog: ChangelogVersionWithPrevVersion = {
		...latestChangelog,
		prevVersion: getLastVersion(prevVersion.version),
	};

	fs.writeFileSync(input.outputChangelogPath, JSON.stringify(changelog));
}
