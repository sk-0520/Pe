#include "../Pe.Library/text.h"
#include "component.h"

//void create_component_core(COMPONENT_BASE* result, COMPONENT_TYPE component_type, const TEXT* custom_class_name)
//{
//    WNDCLASS wnd_class /*= {
//    }*/;
//}

WINDOW_COMPONENT create_window_component(const WINDOW_COMPONENT_OPTIONS* options)
{
    WINDOW_COMPONENT component;
    set_memory(&component, 0, sizeof(component));

    //WNDCLASS wnd_class = {
    //    0
    //};

    return component;
}
