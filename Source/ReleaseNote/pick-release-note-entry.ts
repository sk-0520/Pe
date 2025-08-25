import path from "node:path";
import { type Input, main } from "./pick-release-note";

const rootDirPath = path.resolve(__dirname, "..", "..");

const input: Input = {
	outputChangelogPath: path.resolve(rootDirPath, "Define", "@changelog.json"),
};

main(input);
