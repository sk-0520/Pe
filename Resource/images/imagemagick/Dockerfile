FROM dpokidov/imagemagick

RUN apt-get update \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

RUN ln -s /usr/bin/python2 /usr/bin/python
