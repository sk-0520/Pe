﻿#include <windows.h>
#include <shlwapi.h>

#include "debug.h"
#include "text.h"


static TEXT_PARSED_INT32_RESULT create_failed_integer_parse_result()
{
    TEXT_PARSED_INT32_RESULT result = {
        .success = false,
    };

    return result;
}

static TEXT_PARSED_INT64_RESULT create_failed_long_parse_result()
{
    TEXT_PARSED_INT64_RESULT result = {
        .success = false,
    };

    return result;
}

TEXT_PARSED_INT32_RESULT parse_integer_from_text(const TEXT* input, bool support_hex)
{
    if (!is_enabled_text(input)) {
        return create_failed_integer_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT32_RESULT result;
#pragma warning(pop)
    result.success = StrToIntEx(input->value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;
}

TEXT_PARSED_INT64_RESULT parse_long_from_text(const TEXT* input, bool support_hex)
{
    if (!is_enabled_text(input)) {
        return create_failed_long_parse_result();
    }

#pragma warning(push)
#pragma warning(disable:6001)
    TEXT_PARSED_INT64_RESULT result;
#pragma warning(pop)
    result.success = StrToInt64Ex(input->value, support_hex ? STIF_SUPPORT_HEX : STIF_DEFAULT, &result.value);

    return result;

}

#ifdef _UNICODE

bool is_enabled_multibyte_character_result(const MULTIBYTE_CHARACTER_RESULT* mbcr)
{
    if (!mbcr) {
        return false;
    }

    if (!mbcr->buffer) {
        return false;
    }

    return true;
}

MULTIBYTE_CHARACTER_RESULT RC_HEAP_FUNC(convert_to_multibyte_character, const TEXT* input, MULTIBYTE_CHARACTER_TYPE mbc_type)
{
    DWORD flags = 0;
    CHAR default_char = '?';

    int mc_length1 = WideCharToMultiByte(mbc_type, flags, input->value, (int)input->length, NULL, 0, &default_char, NULL);
    if (!mc_length1) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    uint8_t* buffer = RC_HEAP_CALL(allocate_memory, mc_length1 * sizeof(uint8_t) + sizeof(uint8_t), true);
    int mc_length2 = WideCharToMultiByte(mbc_type, flags, input->value, (int)input->length, (LPSTR)buffer, mc_length1, &default_char, NULL);
    if (!mc_length2) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    if (mc_length1 != mc_length2) {
        MULTIBYTE_CHARACTER_RESULT error = {
            .buffer = NULL,
            .length = 0,
        };
        return error;
    }

    MULTIBYTE_CHARACTER_RESULT result = {
        .buffer = buffer,
        .length = mc_length2,
    };
    return result;
}

bool RC_HEAP_FUNC(free_multibyte_character_result, MULTIBYTE_CHARACTER_RESULT* mbcr)
{
    if (!mbcr) {
        return false;
    }
    if (!mbcr->buffer) {
        return false;
    }

    bool result = RC_HEAP_CALL(free_memory, mbcr->buffer);

    mbcr->buffer = NULL;
    mbcr->length = 0;

    return result;
}

TEXT RC_HEAP_FUNC(make_text_from_multibyte, const uint8_t* input, size_t length, MULTIBYTE_CHARACTER_TYPE mbc_type)
{
    DWORD flags = 0;
    int wc_length1 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, NULL, 0);
    if (!wc_length1) {
        return create_invalid_text();
    }

    TCHAR* buffer = RC_HEAP_CALL(allocate_string, wc_length1);
    int wc_length2 = MultiByteToWideChar(mbc_type, flags, (CHAR*)input, (int)length, buffer, wc_length1);
    if (!wc_length2) {
        RC_HEAP_CALL(free_string, buffer);
        return create_invalid_text();
    }
    if (wc_length1 != wc_length2) {
        RC_HEAP_CALL(free_string, buffer);
        return create_invalid_text();
    }

    return wrap_text_with_length(buffer, wc_length2, true);
}


#endif // UNICODE