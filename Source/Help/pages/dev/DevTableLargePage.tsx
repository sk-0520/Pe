import { default as markdown } from "bundle-text:../../../../Define/table-file.md";
import type { FC } from "react";
import { DatabaseTables } from "@/Help/components/table/DatabaseTables";
import type { PageProps } from "@/Help/types/page";

export const DevTableLargePage: FC<PageProps> = (props: PageProps) => {
	return <DatabaseTables markdown={markdown} />;
};
