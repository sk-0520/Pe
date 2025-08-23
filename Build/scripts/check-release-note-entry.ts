import path from "node:path";
import { type Input, main, type Options } from "./check-release-note";

const rootDirPath = path.resolve(__dirname, "..", "..");

const input: Input = {
	rootDirPath: rootDirPath,
};

const options: Options = {
	isRelease: true,
};

main(input, options);
