import path from "node:path";
import { globSync } from "glob";

import * as validate_josn from "./validate-json";

const rootDir = path.resolve(__dirname, "..", "..");

const appSettingsDirPath = path.resolve(
	rootDir,
	"Source",
	"Pe",
	"Pe.Main",
	"etc",
);
const jsons: string[] = [
	...globSync("*appsettings*.json", {
		cwd: appSettingsDirPath,
	}).map((a) => path.resolve(appSettingsDirPath, a)),
];

validate_josn.main(jsons);
