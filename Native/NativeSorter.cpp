#include "NativeSorter.h"
#include <algorithm>

extern "C" __declspec(dllexport) void __cdecl TestSort(std::int32_t* values, int length)
{
    if (values == nullptr || length <= 1)
        return;

    std::sort(values, values + length);
}
