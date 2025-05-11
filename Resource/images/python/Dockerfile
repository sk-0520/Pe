FROM python:3.13

ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* \
    && pip install Pillow

