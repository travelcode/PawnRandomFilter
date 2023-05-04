using RimWorld;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    public class TraitDegreeDataRecord
    {
        private readonly TraitDef traitDef;
        private readonly TraitDegreeData traitDegreeData;
        private readonly int degree;
        public TraitDegreeDataRecord(TraitDef traitDef)
        {
            this.traitDef = traitDef;
            degree = 0;
            Trait trait = new Trait(traitDef, 0, forced: true);
            traitDegreeData = trait.CurrentData;
            traitDegreeData.commonality = traitDef.GetGenderSpecificCommonality(Gender.Male);
        }
        public TraitDegreeDataRecord(TraitDef traitDef, TraitDegreeData traitDegreeData)
        {
            this.traitDef = traitDef;
            this.traitDegreeData = traitDegreeData;
            degree = traitDegreeData.degree;
        }

        public TraitDef TraitDef => traitDef;

        public TraitDegreeData TraitDegreeData => traitDegreeData;

        public int Degree => degree;
    }
}
