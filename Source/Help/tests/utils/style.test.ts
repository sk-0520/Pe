import { toCssFontFamily } from "@/Help/utils/style";

describe("toCssFontFamily", () => {
	test.each([
		["", []],
		["a", ["a"]],
		['"a b"', ["a b"]],
		['a,"b c",d', ["a", "b c", "d"]],
	])("期待値: %s, fontFamilies: %p", (expected: string, input: string[]) => {
		expect(toCssFontFamily(input)).toBe(expected);
	});
});
