namespace BeatGuard_Engine
{
    public abstract class IFingerPrintModulator
    {
        public abstract int[] Modulate(FingerPrint fingerPrint);
    }
}