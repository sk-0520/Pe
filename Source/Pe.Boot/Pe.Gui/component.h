#pragma once
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

typedef struct tag_GUI_COMPONENT_LIBRARY
{
    const HANDLE handle;
    const GUI_ROOT_RESOURCE* root;
    const COMPONENT_TYPE type;
    const WNDCLASS wnd_class;
} GUI_COMPONENT_LIBRARY;

//typedef struct tag_COMPONENT_BASE
//{
//    GUI_COMPONENT_LIBRARY library;
//} COMPONENT_BASE;

typedef struct tag_WINDOW_COMPONENT
{
    GUI_COMPONENT_LIBRARY library;
} WINDOW_COMPONENT;

typedef enum tag_WINDOW_COMPONENT_STYLE
{

} WINDOW_COMPONENT_STYLE;

typedef struct tag_WINDOW_COMPONENT_OPTIONS
{
    HINSTANCE hInstance;
    const WNDPROC procedure;
    const TEXT* window_class_name;
    const TEXT* title;
    WINDOW_COMPONENT_STYLE style;
    bool visible;
} WINDOW_COMPONENT_OPTIONS;

WINDOW_COMPONENT create_window_component(const WINDOW_COMPONENT_OPTIONS* options);
