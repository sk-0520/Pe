import type { ThemeOptions } from "@mui/material/styles";
import { toCssFontFamily } from "@/Help/utils/style";

export const AppTheme: ThemeOptions = {
	palette: {
		mode: "light",
		primary: {
			main: "#795548",
		},
		secondary: {
			main: "#86aab7",
		},
	},
	typography: {
		button: {
			textTransform: "none",
		},
		fontFamily: toCssFontFamily([
			// cSpell:disable
			"Verdana",
			"Skia-Regular_Condensed",
			"Tahoma",
			"Meiryo UI",
			"メイリオ",
			"Meiryo",
			"Osaka",
			"YuGothic",
			"Yu Gothic",
			"sans-serif",
			// cSpell:enable
		]),
	},
};
