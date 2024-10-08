import type { ThemeOptions } from "@mui/material/styles";

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
		fontFamily: [
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
		]
			.map((a) => (a.includes(" ") ? `"${a}"` : a))
			.join(","),
	},
};
