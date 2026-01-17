import { parseArgs } from "node:util";
import { type Input, main } from "./pick-release-note";

const parsed = parseArgs({
	allowPositionals: true,
	args: process.argv.slice(2),
	options: {
		outputChangelogPath: {
			type: "string",
		},
	},
});

if (!parsed.values.outputChangelogPath) {
	throw new Error("outputChangelogPath is required");
}

const input: Input = {
	outputChangelogPath: parsed.values.outputChangelogPath,
};

main(input);
