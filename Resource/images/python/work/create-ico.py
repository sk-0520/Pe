
import struct
import sys
import glob
import argparse
from natsort import natsorted

def get_png_size(data):
    # PNGヘッダは8バイト、その後IHDRチャンクが続く
    if data[:8] != b'\x89PNG\r\n\x1a\n':
        raise ValueError("PNGファイルではありません")
    # IHDRチャンクは最初の8+4+4=16バイト以降
    width = struct.unpack(">I", data[16:20])[0]
    height = struct.unpack(">I", data[20:24])[0]
    return width, height

def create_ico(png_files, out_path):
    # ICOヘッダ
    header = struct.pack('<HHH', 0, 1, len(png_files))
    dir_entries = b''
    images = []
    offset = 6 + 16 * len(png_files)
    for png in png_files:
        with open(png, 'rb') as f:
            data = f.read()
        try:
            width, height = get_png_size(data)
        except Exception as e:
            print(f"{png} のサイズ取得に失敗: {e}")
            continue
        width = width if width < 256 else 0
        height = height if height < 256 else 0
        entry = struct.pack(
            '<BBBBHHII',
            width, height, 0, 0, 0, 0,
            len(data), offset
        )
        dir_entries += entry
        images.append(data)
        offset += len(data)
    with open(out_path, 'wb') as f:
        f.write(header)
        f.write(dir_entries)
        for img in images:
            f.write(img)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="複数の PNG 画像から ICO ファイルを生成")
    parser.add_argument("--dir", required=True, help="PNG 画像を含む入力ディレクトリ")
    parser.add_argument("--output", required=True, help="出力 ICO ファイルのパス")
    args = parser.parse_args()

    in_dir = args.dir
    out_file = args.output

    print(f"ディレクトリパス: {in_dir}")
    print(f"出力アイコンパス: {out_file}")

    png_files = natsorted(glob.glob(f"{in_dir}/*.png"))
    create_ico(png_files, out_file)
