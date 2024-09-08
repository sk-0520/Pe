import {
	Box,
	Button,
	Checkbox,
	FormControlLabel,
	Stack,
	TextField,
} from "@mui/material";
import type { FC } from "react";
import { Controller, useForm } from "react-hook-form";
import type { PageProps } from "../../types/page";

interface SearchInput {
	query: string;
	/** 大文字と小文字を区別する */
	case: boolean;
	/** 正規表現 */
	regex: boolean;
	/** 開発ドキュメントを含める */
	dev: boolean;
}

export const HelpSearchPage: FC<PageProps> = (props: PageProps) => {
	const { control, handleSubmit } = useForm<SearchInput>({
		defaultValues: {
			query: "",
			case: false,
			regex: false,
			dev: false,
		},
	});

	function onSubmit(data: SearchInput) {
		console.debug(data);
	}

	return (
		<form onSubmit={handleSubmit(onSubmit)}>
			<Box>
				<Controller
					control={control}
					name="query"
					rules={{
						required: true,
					}}
					render={({ field, formState: { errors } }) => (
						<TextField
							{...field}
							label="検索文言"
							fullWidth
							error={!!errors.query}
						/>
					)}
				/>
				<Stack direction="row">
					<Controller
						control={control}
						name="case"
						render={({ field, formState: { errors } }) => (
							<FormControlLabel
								control={<Checkbox {...field} checked={field.value} />}
								label="大文字と小文字を区別する"
							/>
						)}
					/>
					<Controller
						control={control}
						name="regex"
						render={({ field, formState: { errors } }) => (
							<FormControlLabel
								control={<Checkbox {...field} checked={field.value} />}
								label="正規表現を使用する"
							/>
						)}
					/>
					<Controller
						control={control}
						name="dev"
						render={({ field, formState: { errors } }) => (
							<FormControlLabel
								control={<Checkbox {...field} checked={field.value} />}
								label="開発ドキュメントを含める"
							/>
						)}
					/>
				</Stack>
				<Box>
					<Button variant="contained" type="submit">
						検索
					</Button>
				</Box>
			</Box>
		</form>
	);
};
