#!/bin/bash -ue

ARGS=$(getopt -o '' -l "input:,output:" -- "$@") || exit 1
eval "set -- $ARGS"
while [ $# -gt 0 ]; do
  case $1 in
    --input) INPUT_ICON_PATH=$2; shift 2 ;;
    --output) OUTPUT_ICON_PATH=$2; shift 2 ;;
    --) shift; break ;;
  esac
done

echo $INPUT_ICON_PATH
echo $OUTPUT_ICON_PATH
