import argparse
from PIL import Image

def convert(input_path: str, output_path: str) -> None:
    print(f"{input_path=}, {output_path=}")
    source_icon = Image.open(input_path).convert('RGBA')

    #output_image = Image.new(mode="RGBA")

    print(f"{source_icon=}")
    image_sizes = source_icon.info['sizes']
    #source_icon.
    for image_size in image_sizes:
        print(f"{image_size=}")
        print(f"{image_size[0]}")

    source_icon.save(output_path)

    pass


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--input')
    parser.add_argument('--output')
    args = parser.parse_args()

    convert(args.input, args.output)
