using RimWorld;
using System.Collections.Generic;
using Verse;
namespace Nomadicooer.rimworld.crp
{
    public class ProviderTraits
    {
        private static ProviderTraits? instance;
        private readonly List<TraitDegreeDataRecord> allTraits = new List<TraitDegreeDataRecord>();
        public ProviderTraits()
        {
            IEnumerable<TraitDef> traitDefs = DefDatabase<TraitDef>.AllDefs;
            foreach (var traitDef in traitDefs)
            {
                List<TraitDegreeData> degreeDatas = traitDef.degreeDatas;
                int count = degreeDatas.Count;
                if (count <= 0)
                {
                    TraitDegreeDataRecord item = new TraitDegreeDataRecord(traitDef);
                    this.allTraits.Add(item);
                    continue;
                }
                for (int i = 0; i < count; i++)
                {
                    TraitDegreeData traitDegreeData = degreeDatas[i];
                    allTraits.Add(new TraitDegreeDataRecord(traitDef, traitDegreeData));
                }
            }
        }

        public static ProviderTraits Instance => instance ??= new ProviderTraits();
        public List<TraitDegreeDataRecord> AllTraits => this.allTraits;
    }
}
