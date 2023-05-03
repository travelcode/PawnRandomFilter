using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    public class ProviderSkills
    {
        private static ProviderSkills? instance;
        private ProviderSkills() { }
        private IEnumerable<SkillDef> skills = new List<SkillDef>();
        public IEnumerable<SkillDef> Skills
        {
            get
            {
                if (skills.Any())
                {
                    skills = DefDatabase<SkillDef>.AllDefs;
                }
                return skills;
            }
        }

        public static ProviderSkills Instance => instance ??= new ProviderSkills();
    }
}
