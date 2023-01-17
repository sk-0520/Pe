﻿#pragma once
#include "gui.h"

typedef enum tag_COMPONENT_TYPE
{
    COMPONENT_TYPE_WINDOW,
    COMPONENT_TYPE_CUSTOM,

    COMPONENT_TYPE_BUTTON,
    COMPONENT_TYPE_COMBOBOX,
    COMPONENT_TYPE_EDIT,
    COMPONENT_TYPE_LISTBOX,
    COMPONENT_TYPE_LABEL,

    COMPONENT_TYPE_PROGRESS,
    COMPONENT_TYPE_STATUSBAR,
    COMPONENT_TYPE_TOOLBAR,
    COMPONENT_TYPE_TOOLTIP,
    COMPONENT_TYPE_SPIN,

} COMPONENT_TYPE;

#define GUI_COMPONENT_LIBRARY \
struct { \
    const COMPONENT_TYPE type; \
    const HANDLE handle; \
    const GUI_ROOT_RESOURCE* root; \
} library

typedef struct tag_GUI_BASE_COMPONENT
{
    GUI_COMPONENT_LIBRARY;
} GUI_BASE_COMPONENT;

void create_component();
