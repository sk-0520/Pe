import { main } from "../../../Build/scripts/check-release-note";

describe("check-release-note", () => {
	test("development", () => {
		expect(() => {
			main(
				{
					changelog: {
						date: "YYYY/MM/DD",
						version: "0.00.0",
						contents: [],
					},
				},
				{
					isRelease: false,
				},
			);
		}).not.toThrow();
	});

	test("date", () => {
		expect(() => {
			main(
				{
					changelog: {
						date: "YYYY/MM/DD",
						version: "0.00.0",
						contents: [],
					},
				},
				{
					isRelease: true,
				},
			);
		}).toThrow(/\s*{\s*"date":\s*.*}/);
	});

	describe("subject", () => {
		test("single", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "",
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow("subject");
		});

		test("next", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
										},
										{
											subject: "",
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow("subject");
		});
	});

	describe("revision", () => {
		test("no hash", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz",
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow(
				"subject: revision: zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz",
			);
		});

		test("long hash", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"ffffffffffffffffffffffffffffffffffffffffaa",
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow(
				"subject: revision: ffffffffffffffffffffffffffffffffffffffffaa",
			);
		});

		test("short hash", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"ffffffffffffffffffffffffffffffffffffff",
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow(
				"subject: revision: ffffffffffffffffffffffffffffffffffffff",
			);
		});

		test("hash", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"ffffffffffffffffffffffffffffffffffffffff",
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).not.toThrow();
		});
	});

	describe("comments", () => {
		test("undefined", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"ffffffffffffffffffffffffffffffffffffffff",
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).not.toThrow();
		});

		test("single", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"ffffffffffffffffffffffffffffffffffffffff",
											comments: [""],
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow("subject: comments");
		});

		test("1,2,3", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"ffffffffffffffffffffffffffffffffffffffff",
											comments: ["1", "2", "3"],
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).not.toThrow();
		});

		test("1,3", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [
										{
											subject: "subject",
											revision:
												"ffffffffffffffffffffffffffffffffffffffff",
											comments: ["1", " ", "3"],
										},
									],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow("subject: comments");
		});
	});

	describe("type", () => {
		test("full", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "note",
									logs: [],
								},
								{
									type: "features",
									logs: [],
								},
								{
									type: "fixes",
									logs: [],
								},
								{
									type: "developer",
									logs: [],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).not.toThrow();
		});

		test("order features", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "features",
									logs: [],
								},
								{
									type: "note",
									logs: [],
								},

								{
									type: "fixes",
									logs: [],
								},
								{
									type: "developer",
									logs: [],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow();
		});

		test("order fixes", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "fixes",
									logs: [],
								},
								{
									type: "note",
									logs: [],
								},
								{
									type: "features",
									logs: [],
								},
								{
									type: "developer",
									logs: [],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow();
		});

		test("order developer", () => {
			expect(() => {
				main(
					{
						changelog: {
							date: "2010/01/01",
							version: "0.00.0",
							contents: [
								{
									type: "developer",
									logs: [],
								},
								{
									type: "note",
									logs: [],
								},
								{
									type: "features",
									logs: [],
								},
								{
									type: "fixes",
									logs: [],
								},
							],
						},
					},
					{
						isRelease: true,
					},
				);
			}).toThrow();
		});
	});
});
