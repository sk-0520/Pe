import Changelogs from "../../Define/changelogs";
import { getElement } from "../../Source/Help/utils/access";
import { type Input, main, type Options } from "./check-release-note";

const input: Input = {
	changelog: getElement(Changelogs, 0),
};

const options: Options = {
	isRelease: true,
};

main(input, options);
