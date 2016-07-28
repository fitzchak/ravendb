using System.Runtime.InteropServices;

namespace Voron.Data.RawData
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct RawDataSmallPageHeader
    {
        [FieldOffset(0)]
        public long PageNumber;

        [FieldOffset(8)]
        public ushort NumberOfEntries;

        [FieldOffset(10)]
        public ushort NextAllocation;

        [FieldOffset(12)]
        public PageFlags Flags;

        [FieldOffset(13)]
        public RawDataPageFlags RawDataFlags;

        [FieldOffset(14)]
        public ushort PageNumberInSection;
    }
}
