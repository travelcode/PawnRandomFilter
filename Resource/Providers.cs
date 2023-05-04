namespace Nomadicooer.rimworld.prf
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
        public ProviderBackstories ProviderBackstories => providerBackstories ??= ProviderBackstories.Instance;
        public ProviderSkills ProviderSkills => providerSkills ??= ProviderSkills.Instance;
        public ProviderTraits ProviderTraits => providerTraits ??= ProviderTraits.Instance;
        public ProviderWorkTypes ProviderWorkTypes => providerWorkTypes ??= ProviderWorkTypes.Instance;
    }
}
