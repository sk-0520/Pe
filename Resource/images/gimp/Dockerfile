FROM ubuntu:25.10

RUN apt update \
    && apt install --yes wget fuse libfuse2

WORKDIR /opt

RUN wget -O gimp3.AppImage "https://download.gimp.org/gimp/v3.0/linux/GIMP-3.0.4-x86_64.AppImage" && \
    chmod +x gimp3.AppImage
CMD ["./gimp3.AppImage"]
