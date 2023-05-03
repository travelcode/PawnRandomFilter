using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    public class ProviderBackstories
    {
        private static ProviderBackstories? instance;
        private readonly List<BackstoryDef> childhoodBackstories = new List<BackstoryDef>();
        private readonly List<BackstoryDef> adulthoodBackstories = new List<BackstoryDef>();
        private ProviderBackstories() => InitStory();
        private void InitStory()
        {
            IEnumerable<BackstoryDef> backstories = DefDatabase<BackstoryDef>.AllDefs;
            foreach (BackstoryDef backstory in backstories)
            {
                switch (backstory.slot)
                {
                    case BackstorySlot.Childhood:
                        childhoodBackstories.Add(backstory);
                        break;
                    case BackstorySlot.Adulthood:
                        adulthoodBackstories.Add(backstory);
                        break;
                }
            }
        }
        public static ProviderBackstories Instance => instance ??= new ProviderBackstories();
        public List<BackstoryDef> ChildhoodBackstories => this.childhoodBackstories;
        public List<BackstoryDef> AdulthoodBackstories => this.adulthoodBackstories;
    }
}
