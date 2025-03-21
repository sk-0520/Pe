登録されているランチャーアイテムをキー入力から起動します。

通知領域のコンテキストメニューからも呼び出せますがキー入力から呼び出すのが設計思想です。

入力された文字列からどうこうするのではなくリスト上で選択されているアイテムを起動します。

* 先頭が <MdInline kind="key">/</MdInline> で始まる場合、以降の文字列を正規表現として扱います
* 非正規表現による検索条件の場合に <MdInline kind="key">\*</MdInline>, <MdInline kind="key">?</MdInline> が入力されている場合にはワイルドカード(制限事項: 行頭・末尾に一致)として解釈されます
* 先頭文字列(正規表現の場合に Pe の解釈としての先頭文字列)が大文字アルファベットの場合に大文字・小文字を区別する検索となります

# ランチャーアイテム

アイテム選択状態で以下の入力により起動します。

| 入力 | 起動内容 |
|---|---|
| <MdInline kind="key">Enter</MdInline> | 通常起動 |
| <MdInline kind="key">Shift + Enter</MdInline> | 指定して実行 |

# 本体コマンド

入力コマンドの先頭が <MdInline kind="key">.</MdInline> のものは Pe 専用コマンドの意味を持ちます。

本体コマンド以外の登録済みコマンドとの区別はアイコン・説明書きで判断してください。

<MdAlert kind="NOTE">
  内部的な制限により本体コマンドは正規表現を使用できません。
</MdAlert>
