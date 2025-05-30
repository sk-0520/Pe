import {
	type Token,
	type VersionInfo,
	convertTagFromVersion,
	splitTokens,
	splitVersionInfos,
} from "../../utils/changelog";

describe("splitTokens", () => {
	test("empty", () => {
		const actual = splitTokens("");
		expect(actual).toHaveLength(0);
	});

	test("text", () => {
		const actual = splitTokens("abc def ghi");
		expect(actual).toHaveLength(1);
		expect(actual[0]).toStrictEqual({
			kind: "text",
			value: "abc def ghi",
		});
	});

	test.each([["#0"], ["#1"], ["#12"], ["#123"]])("issue: %s", (data) => {
		const actual = splitTokens(data);
		expect(actual).toHaveLength(1);
		expect(actual[0]).toHaveProperty("kind", "issue");
		expect(actual[0]).toHaveProperty("value", data.substring(1));
	});

	test.each([
		["http://localhost"],
		["https://localhost"],
		["https://localhost/path"],
		["https://localhost/path?q"],
		["https://localhost/path?q=v"],
		["https://localhost/path?q=v&q=v"],
		["https://localhost/path?q=v&q=v#fragment"],
	])("url: %s", (data) => {
		const actual = splitTokens(data);
		expect(actual).toHaveLength(1);
		expect(actual[0]).toHaveProperty("kind", "url");
		expect(actual[0]).toHaveProperty("value", data);
	});

	test.each([
		[
			[
				{ kind: "text", value: "abc " },
				{ kind: "issue", value: "123" },
				{ kind: "text", value: " " },
				{ kind: "url", value: "http://localhost" },
			] satisfies Token[],
			"abc #123 http://localhost",
		],
		[
			[
				{ kind: "text", value: "#" },
				{ kind: "url", value: "http://localhost#4" },
			] satisfies Token[],
			"#http://localhost#4",
		],
		[
			[
				{ kind: "url", value: "http://localhost" },
				{ kind: "text", value: " text" },
			] satisfies Token[],
			"http://localhost text",
		],
		[
			[
				{ kind: "issue", value: "1" },
				{ kind: "text", value: " text1 " },
				{ kind: "url", value: "http://localhost/1" },
				{ kind: "text", value: " " },
				{ kind: "issue", value: "2" },
				{ kind: "text", value: " text2 " },
				{ kind: "url", value: "http://localhost/2" },
				{ kind: "text", value: " " },
				{ kind: "issue", value: "3" },
				{ kind: "text", value: " text3 " },
				{ kind: "url", value: "http://localhost/3" },
			] satisfies Token[],
			"#1 text1 http://localhost/1 #2 text2 http://localhost/2 #3 text3 http://localhost/3",
		],
	])("tokens: 期待値: [%o], 入力: [%s]", (expected: Token[], input: string) => {
		const actual = splitTokens(input);
		expect(actual).toEqual(expected);
	});

	test.each([
		[[] satisfies VersionInfo[], ""],
		[[{ value: "a", isVersion: false }] satisfies VersionInfo[], "a"],
		[
			[{ value: "1.2.3.4", isVersion: false }] satisfies VersionInfo[],
			"1.2.3.4",
		],
		[[{ value: "1.2.3", isVersion: true }] satisfies VersionInfo[], "1.2.3"],
		[
			[
				{ value: "1.2.3", isVersion: true },
				{ value: "4.5.6", isVersion: true },
			] satisfies VersionInfo[],
			"1.2.3,4.5.6",
		],
		[
			[
				{ value: "1.2.3", isVersion: true },
				{ value: "4.5.6", isVersion: true },
			] satisfies VersionInfo[],
			"1.2.3, 4.5.6",
		],
		[
			[
				{ value: "1.2.3", isVersion: true },
				{ value: "4.5.6", isVersion: true },
			] satisfies VersionInfo[],
			"1.2.3 ,4.5.6",
		],
		[
			[
				{ value: "1.2.3", isVersion: true },
				{ value: "4.5.6", isVersion: true },
				{ value: "version", isVersion: false },
				{ value: "7.8.9", isVersion: true },
			] satisfies VersionInfo[],
			"1.2.3 , 4.5.6 , version,7.8.9",
		],
	])(
		"tokens: 期待値: [%o], 入力: [%s]",
		(expected: VersionInfo[], input: string) => {
			const actual = splitVersionInfos(input);
			expect(actual).toEqual(expected);
		},
	);

	test.each([
		["", ""],
		["0.0.0", "0.0.0"],
		["0.00.000", "0.00.000"],
		["0.83.0", "0.83.0"],
		["0.84.0", "0.84.0"],
		["0.84.00", "0.84.00"],
		["0.84.0", "0.84.000"],
		["0.99.096", "0.99.096"],
		["0.100.0", "0.100.0"],
		["ABC", "ABC"],
		["A.B.C", "A.B.C"],
		["1.2.3.4", "1.2.3.4"],
	])(
		"convertTagFromVersion: 期待値: [%s], 入力: [%s]",
		(expected: string, input: string) => {
			const actual = convertTagFromVersion(input);
			expect(actual).toEqual(expected);
		},
	);
});
