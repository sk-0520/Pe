// cSpell:ignore markuplint
import type { Config } from "@markuplint/ml-config";

const config: Config = {
	rules: {
		"required-h1": false,
	},
	excludeFiles: ["*/**/bin"],
	extends: ["markuplint:recommended", "markuplint:recommended-react"],
	parser: {
		".[jt]sx$": "@markuplint/jsx-parser",
	},
	specs: {
		".[jt]sx$": "@markuplint/react-spec",
	},
};

export default config;
