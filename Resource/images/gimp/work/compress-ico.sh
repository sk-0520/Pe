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

SCRIPT_NAME=compress-ico.py
SCRIPT_PATH="$(cd "$(dirname "${0}")"; pwd)/${SCRIPT_NAME}"

GIMP_SCRIPT_DIR=~/.config/GIMP/2.10/plug-ins
mkdir -v -p "${GIMP_SCRIPT_DIR}"
cp -v "${SCRIPT_PATH}" "${GIMP_SCRIPT_DIR}"
ls -al "${GIMP_SCRIPT_DIR}"
chmod +x "${GIMP_SCRIPT_DIR}/${SCRIPT_NAME}"

ls -al ${GIMP_SCRIPT_DIR}

#gimp-console -i -b "(process-image \"${INPUT_ICON_PATH}\" \"${OUTPUT_ICON_PATH}\")" -b "(gimp-quit 0)"
gimp -i -b "(python-fu-compress-icon \"${INPUT_ICON_PATH}\" \"${OUTPUT_ICON_PATH}\")" -b '(gimp-quit 0)'

