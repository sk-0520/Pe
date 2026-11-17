// cSpell:ignore markuplint
import type { Config } from "@markuplint/ml-config";

const config: Config = {
	rules: {
		"require-h1": false,
		"no-duplicate-h1": false,
	},
	excludeFiles: ["*/**/bin"],
	extends: ["markuplint:recommended", "markuplint:recommended-react"],
	parser: {
		".[jt]sx$": "@markuplint/jsx-parser",
	},
	specs: {
		".[jt]sx$": "@markuplint/react-spec",
	},
	overrides: {
		// React の onSubmit などはインライン HTML 属性ではなく合成イベントの props であり、
		// インラインハンドラ(XSS/CSP)の脅威モデルに該当しないため JSX/TSX に限り無効化する
		"./Source/**/*.{jsx,tsx}": {
			rules: {
				"security/no-event-handler-attr": false,
			},
		},
	},
	// 既定値 "reset" では override が設定全体を置換してしまうため "merge" を明示する
	overrideMode: "merge",
};

export default config;
