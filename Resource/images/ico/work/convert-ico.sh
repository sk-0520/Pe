#!/bin/bash -ue

ARGS=$(getopt -o '' -l "dir:,output:" -- "$@") || exit 1
eval "set -- $ARGS"
while [ $# -gt 0 ]; do
	case $1 in
		--dir) INPUT_DIR_PATH=$2; shift 2 ;;
		--output) OUTPUT_ICON_PATH=$2; shift 2 ;;
		--) shift; break ;;
	esac
done

echo INPUT_DIR_PATH
echo "${INPUT_DIR_PATH}"
ls "${INPUT_DIR_PATH}"
echo OUTPUT_ICON_PATH
echo "${OUTPUT_ICON_PATH}"

pushd "${INPUT_DIR_PATH}"
	icotool -c -o "${OUTPUT_ICON_PATH}" *.png
popd
