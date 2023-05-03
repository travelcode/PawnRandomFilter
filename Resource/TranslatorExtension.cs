using System;
using Verse;
using Verse.Noise;

namespace Nomadicooer.rimworld.crp
{
    internal static class TranslatorExtension
    {
        private const string prefix = "nc.rw.crp.";
        private const string prefixBtnKey = prefix + "btn.";
        private const string prefixLabelKey = prefix + "label.";
        private const string prefixTooltipKey = prefix + "tooltip.";
        private const string prefixEnumKey = prefix + "enum.";
        internal static string Text(this string key)
        {
            string normalKey = prefix + key;
            return normalKey.TranslateWithBackup(key);
        }
        internal static string ButtonText(this string key)
        {
            string btnKey = prefixBtnKey + key;
            return btnKey.TranslateWithBackup(key);
        }
        internal static string LabelText(this string key)
        {
            string labelKey = prefixLabelKey + key;
            return labelKey.TranslateWithBackup(key);
        }
        internal static string TooltipText(this string key)
        {
            string tooltipKey = prefixTooltipKey + key;
            return tooltipKey.TranslateWithBackup(key);
        }
        internal static string EnumText(this string key)
        {
            string enumKey = prefixEnumKey + key;
            return enumKey.TranslateWithBackup(key);
        }
        internal static string Translate(this Enum @enum)
        {
            Type type = @enum.GetType();
            string enumKey = type.Name + "." + @enum.ToString();
            string prefxKey = prefixEnumKey + enumKey;
            return prefxKey.TranslateWithBackup(enumKey);
        }
    }
}
