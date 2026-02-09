import { Link } from "@mui/material";
import type { FC, ReactNode } from "react";

interface MdAnchorProps {
	href?: string;
	children?: ReactNode;
}

/**
 * マークダウン内のリンク要素のカスタムコンポーネント
 * 外部リンクは新しいタブで開く
 */
export const MdAnchor: FC<MdAnchorProps> = (props) => {
	const { href, children } = props;

	// 外部リンクかどうかを判定
	const isExternalLink = /^https?:\/\//i.test(href || "");

	if (isExternalLink) {
		// 外部リンクの場合は新しいタブで開く
		return (
			<Link href={href} target="_blank" rel="noopener noreferrer">
				{children}
			</Link>
		);
	}

	// 内部リンクの場合は通常のリンク
	return <Link href={href}>{children}</Link>;
};
