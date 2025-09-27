import struct
import sys
import glob

def make_ico(png_files, out_path):
    # ICOヘッダ
    header = struct.pack('<HHH', 0, 1, len(png_files))
    dir_entries = b''
    images = []
    offset = 6 + 16 * len(png_files)
    for png in png_files:
        with open(png, 'rb') as f:
            data = f.read()
        # PNGサイズ抽出
        # ここではファイル名等からサイズ推定（厳密にはPNGヘッダ解析も可）
        # 例: icon_16.png → 16x16
        fname = png.split('/')[-1]
        sz = int(''.join([c for c in fname if c.isdigit()]))
        width = sz if sz < 256 else 0
        height = sz if sz < 256 else 0
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
    in_dir = sys.argv[1]  # xxx/dir
    out_file = sys.argv[2]  # xxx/output.ico
    png_files = sorted(glob.glob(f"{in_dir}/*.png"))
    make_ico(png_files, out_file)
    print(f"ICO generated: {out_file}")
