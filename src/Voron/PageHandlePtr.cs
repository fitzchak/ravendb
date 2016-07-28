using System.Runtime.CompilerServices;

namespace Voron
{
    public struct PageHandlePtr
    {
        public readonly Page Value;
        public readonly bool IsWritable;

        public PageHandlePtr(Page value, bool isWritable)
        {
            this.Value = value;
            this.IsWritable = isWritable;
        }

        public long PageNumber
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return IsValid ? Value.PageNumber : -1; }
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Value != null; }
        }
    }
}
