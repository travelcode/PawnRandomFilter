using RimWorld;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    public class TraitDegreeDataRecord
    {
        private readonly TraitDef traitDef;
        private readonly TraitDegreeData traitDegreeData;
        private readonly int degree;
        public TraitDegreeDataRecord(TraitDef traitDef)
        {
            this.traitDef = traitDef;
            this.degree = 0;
            Trait trait = new Trait(traitDef, 0, forced: true);
            this.traitDegreeData = trait.CurrentData;
            this.traitDegreeData.commonality = traitDef.GetGenderSpecificCommonality(Gender.Male);
        }
        public TraitDegreeDataRecord(TraitDef traitDef, TraitDegreeData traitDegreeData)
        {
            this.traitDef = traitDef;
            this.traitDegreeData = traitDegreeData;
            this.degree = traitDegreeData.degree;
        }

        public TraitDef TraitDef => this.traitDef;

        public TraitDegreeData TraitDegreeData => this.traitDegreeData;

        public int Degree => this.degree;
    }
}
