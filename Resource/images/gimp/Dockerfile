# python2 が必要なのです
FROM ubuntu:20.04

ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update \
    && apt-get install --yes \
        gimp \
        gimp-plugin-registry \
        python2 \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
