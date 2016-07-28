using System;

namespace Voron
{
    [Flags]
    public enum PageFlags : byte
    {
        Single = 1,
        Overflow = 2,
        VariableSizeTreePage = 4,
        FixedSizeTreePage = 8,
        PrefixTreePage = 16,
        RawData = 32,
        Reserved2 = 64,
        Reserved3 = 128,
    }
}
