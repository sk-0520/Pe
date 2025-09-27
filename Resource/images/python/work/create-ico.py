
import struct
import sys
import glob
import argparse
from dataclasses import dataclass

@dataclass(frozen=True)
class FileInfo:
    path: str
    data: bytes
    width: int
    height: int


def read_binary(path: str) -> bytes:
    with open(path, 'rb') as f:
        return f.read()

def get_png_size(data: bytes) -> tuple[int, int]:
    # PNGヘッダは8バイト、その後IHDRチャンクが続く
    if data[:8] != b'\x89PNG\r\n\x1a\n':
        raise ValueError("PNGファイルではありません")
    # IHDRチャンクは最初の8+4+4=16バイト以降
    width = struct.unpack(">I", data[16:20])[0]
    height = struct.unpack(">I", data[20:24])[0]
    return width, height

def create_ico(png_files: list[str], out_path: str) ->  None:
    # ファイルを一度だけ読み込み、FileInfoで管理
    file_infos: list[FileInfo] = []
    for png in png_files:
        data = read_binary(png)
        width, height = get_png_size(data)
        file_infos.append(FileInfo(path=png, data=data, width=width, height=height))
    # 横幅で昇順ソート
    file_infos.sort(key=lambda x: x.width)

    header = struct.pack('<HHH', 0, 1, len(file_infos))
    dir_entries = b''
    images = []
    offset = 6 + 16 * len(file_infos)
    for fi in file_infos:
        width = fi.width if fi.width < 256 else 0
        height = fi.height if fi.height < 256 else 0
        entry = struct.pack(
            '<BBBBHHII',
            width, height, 0, 0, 0, 0,
            len(fi.data), offset
        )
        dir_entries += entry
        images.append(fi.data)
        offset += len(fi.data)
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

    png_files = glob.glob(f"{in_dir}/*.png")
    create_ico(png_files, out_file)
