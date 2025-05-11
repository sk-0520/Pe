#!/bin/bash -ue

ARGS=$(getopt -o '' -l "svg:,output:" -- "$@") || exit 1
eval "set -- $ARGS"
echo "ARGS -> $ARGS"
while [ $# -gt 0 ]; do
echo "-> $1"
  case $1 in
    --svg) SOURCE_SVG_PATH=$2; shift 2 ;;
    --output) OUTPUT_DIR_PATH=$2; shift 2 ;;
    --) shift; break ;;
  esac
done

ICON_SIZE_LIST=(
	16  18  20  24  28
	32  40
	48  56  60  64
	72  80  84  96  112  128  160
	192 224
	256 320 384 448 512
)

for ICON_SIZE in "${ICON_SIZE_LIST[@]}"; do
    inkscape \
        --export-dpi=96 \
        --export-width="${ICON_SIZE}" \
        --export-height="${ICON_SIZE}" \
        --export-overwrite \
        --export-type=png \
        --export-filename="${OUTPUT_DIR_PATH}/${ICON_SIZE}.png" \
        "${SOURCE_SVG_PATH}"
done
