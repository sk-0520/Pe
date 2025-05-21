import fs from "node:fs";
import path from "node:path";
import jsonschema from "jsonschema";

export async function main(targetJsons: ReadonlyArray<string>): Promise<void> {
	for (const targetJson of targetJsons) {
		const json = JSON.parse(fs.readFileSync(targetJson).toString());
		if (!("$schema" in json)) {
			throw Error("$schema");
		}
		if (typeof json.$schema !== "string") {
			throw Error("$schema");
		}

		const schemaPath = path.resolve(path.dirname(targetJson), json.$schema);
		const schema = JSON.parse(fs.readFileSync(schemaPath).toString());

		jsonschema.validate(json, schema, {
			throwAll: true,
		});
	}
}
