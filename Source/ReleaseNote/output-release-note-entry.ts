import { parseArgs } from "node:util";
import { type Input, main } from "./output-release-note";

const parsed = parseArgs({
	allowPositionals: true,
	args: process.argv.slice(2),
	options: {
		inputFilePath: {
			type: "string",
		},
		outputFilePath: {
			type: "string",
		},
	},
});

if (!parsed.values.inputFilePath) {
	throw new Error("inputFilePath is required");
}
if (!parsed.values.outputFilePath) {
	throw new Error("outputFilePath is required");
}

const input: Input = {
	inputFilePath: parsed.values.inputFilePath,
	outputFilePath: parsed.values.outputFilePath,
};

main(input);
