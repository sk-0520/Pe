# ベースイメージは軽量なDebian系（例: ubuntu:22.04）
FROM ubuntu:22.04

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        icoutils \
        ca-certificates \
    && apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /work

# エントリポイント例（スクリプトで渡す場合は適宜変更）
# 以下はSVG→PNG→ICO一括生成例
# COPY ./icon.svg /work/icon.svg
# RUN for s in 16 32 48 64 128; do \
#       inkscape -o icon_${s}.png -w $s -h $s icon.svg; \
#     done && \
#     png2ico icon.ico icon_16.png icon_32.png icon_48.png icon_64.png icon_128.png

# 必要に応じてCMDやENTRYPOINTを追加
