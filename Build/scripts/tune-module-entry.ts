import path from "node:path";

import { main } from "./tune-module";

const workDirPath = path.resolve(__dirname, "..", "..", "Output", "help");

main(workDirPath);
