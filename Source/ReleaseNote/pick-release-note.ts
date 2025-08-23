import fs from "node:fs";
import Changelogs from "../../Define/changelogs";
import type { ChangelogVersion } from "../Help/types/changelog";
import { getElement } from "../Help/utils/access";
import { splitVersionInfos } from "../Help/utils/changelog";

interface ChangelogVersionWithPrevVersion extends ChangelogVersion {
	prevVersion: string;
}

export interface Input {
	outputChangelogPath: string;
}

export function main(input: Input) {
	console.debug({ input });

	const latestChangelog = getElement(Changelogs, 0);
	const prevVersion = getElement(
		splitVersionInfos(getElement(Changelogs, 1).version),
		0,
	);

	const changelog: ChangelogVersionWithPrevVersion = {
		...latestChangelog,
		prevVersion: prevVersion.value,
	};

	fs.writeFileSync(input.outputChangelogPath, JSON.stringify(changelog));
}
