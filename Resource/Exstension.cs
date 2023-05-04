using Verse;

namespace Nomadicooer.rimworld.prf
{
    public static class Exstension
    {
        public static bool InRange(this IntRange range, int value)
        {
            return value >= range.min && value <= range.max;
        }
    }
}
