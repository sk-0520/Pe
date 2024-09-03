ちょっとしたメモ書きなんかをデスクトップに貼り付ける付箋機能です。

各種入力は自動保存されます。

# フォーマット

## 種別

<dl>
  <dt>テキスト</dt>
  <dd>ノートの内容はプレーンテキストとして扱われます。</dd>

  <dt>リッチテキスト</dt>
  <dd>ノートの内容は RTF として扱われます。</dd>
</dl>

## 位置

<dl>
  <dt>絶対座標</dt>
  <dd>位置・サイズが絶対値として扱われます。通常のウィンドウみたいなイメージです。</dd>
  
  <dt>相対座標</dt>
  <dd>位置・サイズが相対値として扱われます。ディスプレイのサイズ変更時にもある程度レイアウトを維持したままになります。</dd>
</dl>

## 前景

基本前景色を指定します。

## 背景

基本背景色を指定します。

# 状態

## 固定

ノート操作 UI 以外からの操作を受け付けないようにします。

## 最小化

最小化状態を切り替えます。

## 最前面

最前面状態を切り替えます。

## 右端で折り返す

自動折り返し状態を切り替えます。

種別が *テキスト* の時のみ有効です。

## 自動的に隠す方法

一定時間経過時にノートの内容を隠す方法を指定します。

## タイトルバー位置

タイトルバー位置を指定します。

# 添付ファイル

添付ファイルはノートに対してファイルをD&Dすることで有効になります。

添付ファイルは Windows のショートカットのような扱いで物理ファイルへのリンク扱いとなります(内部的には取り込みがあるのですが実装がつらいので永久に後回し)。

表示されているアイテムをクリックすると対象のファイルを開き、削除等はコンテキストメニュー(右クリック)で処理します。

## 開く

対象ファイルを開きます。

## プロパティ

対象ファイルのプロパティを表示します。

## 削除

サブメニューを選択することで対象ファイルに対する添付を解除します。

# 操作

## コピー

ノートの内容をクリップボードにコピーします。

## 検索

検索処理を行います。

> [!WARNING]
> 前検索は諸々の都合でできません。

## 閉じる

ノートを非表示にします。

## 削除

ノートを削除します。

> [!CAUTION]
> 削除したノートは復旧できません。