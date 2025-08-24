import { Divider, Link, Typography } from "@mui/material";
import type { FC } from "react";
import json from "../../Define/@changelog.json";
import { ChangelogVersion } from "../Help/components/changelog/ChangelogVersion";
import type {
	ChangelogVersionNumbers,
	ChangelogVersion as ChangelogVersionType,
} from "../Help/types/changelog";

export const ReleaseNote: FC = () => {
	const changelog = json as unknown as ChangelogVersionType & {
		prevVersion: ChangelogVersionNumbers | undefined;
	};
	return (
		<>
			<ChangelogVersion {...changelog} />
			<Divider sx={{ marginBlock: "1em" }} />
			<Typography>
				手動でアップデートする場合は
				<Link
					href="https://github.com/sk-0520/Pe/releases/latest/"
					target="_blank"
				>
					こちら
				</Link>
				からモジュールをダウンロードし、既存 Pe を上書きしてください。
			</Typography>
		</>
	);
};
