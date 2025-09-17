import {
	type ChangelogVersion,
	DevelopmentVersionDate,
} from "../../Source/Help/types/changelog";

export interface Input {
	changelog: ChangelogVersion;
}
export interface Options {
	isRelease: boolean;
}

const Revision = /^([0-f]){40}$/;

export function main(input: Input, options: Options) {
	if (!options.isRelease) {
		return;
	}

	const changelog = input.changelog;

	if (changelog.date === DevelopmentVersionDate) {
		throw new Error(JSON.stringify({ date: changelog.date }));
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

	const changelogTypes = changelog.contents.map((a) => a.type);
	const sortedChangelogTypes = changelogTypes.toSorted((a, b) => {
		const order = {
			note: 0,
			features: 1,
			fixes: 2,
			developer: 3,
		};
		return order[a] - order[b];
	});
	if (changelogTypes.toString() !== sortedChangelogTypes.toString()) {
		throw new Error(
			`${changelogTypes.toString()} !== ${sortedChangelogTypes.toString()}`,
		);
	}
}
