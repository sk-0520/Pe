#!/bin/bash -ue

ARGS=$(getopt -o '' -l "input:,output:" -- "$@") || exit 1
eval "set -- $ARGS"
while [ $# -gt 0 ]; do
  case $1 in
    --input) INPUT_DIR_PATH=$2; shift 2 ;;
    --output) OUTPUT_ICON_PATH=$2; shift 2 ;;
    --) shift; break ;;
  esac
done

ICONS=$(find "${INPUT_DIR_PATH}" -maxdepth 1 -name "*.png" | sort --version-sort | tr '\n' ' ')
# shellcheck disable=SC2086
convert ${ICONS} "${OUTPUT_ICON_PATH}"


