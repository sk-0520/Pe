import type { FC } from "react";
import type { VersionInfo } from "../../utils/changelog";

const Setting = {
	firstCommit: "275d6b5f1a41dcb99d56e6448b9249236cdd75c0",
	firstTag: "0.10.0",
} as const;

interface ChangelogVersionInfoProps {
	version: VersionInfo;
	prevVersion?: string;
}

export const ChangelogVersionInfo: FC<ChangelogVersionInfoProps> = (
	props: ChangelogVersionInfoProps,
) => {
	const { version, prevVersion } = props;

	if (!version.isVersion) {
		return version.value;
	}

	return version.value;
};
