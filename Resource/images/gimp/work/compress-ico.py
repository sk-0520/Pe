#!/usr/bin/env python

from gimpfu import *

def compress_icon(input_path, output_path):
    # GIMPで画像を開く
    image = pdb.gimp_file_load(input_path, input_path)
    layer = image.active_layer

    # 60px以上の画像を圧縮
    for layer in image.layers:
        if layer.width >= 60 and layer.height >= 60:
            pdb.gimp_layer_set_compression(layer, TRUE)  # 圧縮を有効にする

    # 画像をICO形式で保存
    pdb.gimp_file_save(image, layer, output_path, output_path)
    pdb.gimp_image_delete(image)  # メモリを解放

register(
    "python_fu_compress_icon",
    "Compress icon",
    "Compress icon images in ICO format",
    "Your Name",
    "Your Name",
    "2023",
    "<Image>/File/Compress Icon",
    "*",
    [
        (PF_STRING, "input_path", "Input ICO file path", ""),
        (PF_STRING, "output_path", "Output ICO file path", "")
    ],
    [],
    compress_icon)

main()
