using RimWorld;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    internal class SkillInfo
    {
        public IntRange intRange = IntRange.zero;
        public Passion passion=Passion.None;

        public SkillInfo(IntRange intRange, Passion passion)
        {
            this.intRange = intRange;
            this.passion = passion;
        }
    }
}
