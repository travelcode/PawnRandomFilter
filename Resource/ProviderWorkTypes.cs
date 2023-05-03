using System.Collections.Generic;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    public class ProviderWorkTypes
    {
        private static ProviderWorkTypes? instance;
        private readonly List<WorkTypeDef> works = new List<WorkTypeDef>();

        private ProviderWorkTypes() => works = (List<WorkTypeDef>)DefDatabase<WorkTypeDef>.AllDefs;

        public static ProviderWorkTypes Instance => instance ??= new ProviderWorkTypes();

        public List<WorkTypeDef> Works => this.works;
    }
}
