namespace Nomadicooer.rimworld.crp
{
    public class Providers
    {
        private static Providers? instance;
        private Providers() { }
        private ProviderBackstories? providerBackstories;
        private ProviderSkills? providerSkills;
        private ProviderTraits? providerTraits;
        private ProviderWorkTypes? providerWorkTypes;
        public static Providers Instance => instance ??= new Providers();
        public ProviderBackstories ProviderBackstories => this.providerBackstories ??= ProviderBackstories.Instance;
        public ProviderSkills ProviderSkills => this.providerSkills ??= ProviderSkills.Instance;
        public ProviderTraits ProviderTraits => this.providerTraits ??= ProviderTraits.Instance;
        public ProviderWorkTypes ProviderWorkTypes => this.providerWorkTypes ??= ProviderWorkTypes.Instance;
    }
}
