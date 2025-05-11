#!/usr/bin/env python

from gimpfu import *

def compress_icon(input_path, output_path):
    print("Input path:", input_path)
    print("Output path:", output_path)

    image = pdb.gimp_file_load(input_path, input_path)
    layer = image.active_layer

    # 60px以上の画像を圧縮
    for layer in image.layers:
        if layer.width >= 60 and layer.height >= 60:
            pdb.gimp_layer_set_compression(layer, TRUE)
    # 画像をICO形式で保存
    pdb.gimp_file_save(image, layer, output_path, output_path)
    pdb.gimp_image_delete(image)

register(
    "python-fu-compress-icon",
    "Compress icon",
    "Compress icon images in ICO format",
    "sk",
    "sk",
    "2025",
    "<Image>/File/Compress Icon",
    "*",
    [
        (PF_STRING, "input_path", "Input ICO file path", ""),
        (PF_STRING, "output_path", "Output ICO file path", "")
    ],
    [],
    compress_icon)

main()
