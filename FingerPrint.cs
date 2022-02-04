using System;

namespace BeatGuard_Engine
{
    public class FingerPrint
    {
        public Guid Value { get; }

        public FingerPrint(Guid value)
        {
            Value = value;
        }
    }
}