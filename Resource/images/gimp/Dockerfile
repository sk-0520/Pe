FROM ubuntu:24.10

RUN apt-get update \
    && apt-get install --yes \
        gimp \
        gimp-plugin-registry \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
