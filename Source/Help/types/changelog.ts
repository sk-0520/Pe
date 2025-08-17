import z from "zod";

export const ChangelogContentKindsSchema = z.enum([
	"features",
	"fixes",
	"developer",
	"note",
]);
export type ChangelogContentKinds = z.infer<typeof ChangelogContentKindsSchema>;

export const ChangelogContentItemTypesSchema = z.enum([
	"compatibility",
	"notice",
	"nuget",
	"myget",
	"plugin-compatibility",
]);
export type ChangelogContentItemTypes = z.infer<
	typeof ChangelogContentItemTypesSchema
>;

export const ChangelogContentItemSchema = z.object({
	revision: z.string(),
	class: ChangelogContentItemTypesSchema.optional(),
	subject: z.string(),
	comments: z.array(z.string()).optional(),
});
export type ChangelogContentItem = z.infer<typeof ChangelogContentItemSchema>;

export const ChangelogContentSchema = z.object({
	type: ChangelogContentKindsSchema,
	logs: z.array(ChangelogContentItemSchema),
});
export type ChangelogContent = z.infer<typeof ChangelogContentSchema>;

export const ChangelogVersionSchema = z.object({
	date: z.string(),
	version: z.string(),
	group: z.string().optional(),
	contents: z.array(ChangelogContentSchema),
});
export type ChangelogVersion = z.infer<typeof ChangelogVersionSchema>;

export const ChangelogsSchema = z.array(ChangelogVersionSchema);
export type Changelogs = z.infer<typeof ChangelogsSchema>;
