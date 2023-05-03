using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    internal static class TooltipUtility
    {
        private readonly static string[] GoodColor = new string[] {
            "#FFFFFF",//白色
            "#2E8B57",//海洋绿
            "#4682B4",//钢蓝色
            "#FF1493",//深紫色
            "#FF8C00",//橙黑色
        };
        private readonly static string[] BadColor = new string[] {
            "#FFFFFF",//白色
            "#D3D3D3",//亮灰色
            "#DC143C",//深红色
            "#FF0000",//红色
            "#8B0000",//暗红色
        };
        private static void Title(StringBuilder sb, string title)
        {
            sb.AppendLine(StringColor.Teal.GetColorMessage(title.Translate()));
        }
        public static string Backstory(BackstoryDef backstory)
        {
            StringBuilder sb = new StringBuilder();
            //背景基本信息
            BackstoryBasic(sb, backstory);
            SpawnCategories(sb, backstory.spawnCategories);
            SkillGains(sb, backstory.skillGains);
            DisableWorks(sb, backstory.DisabledWorkTypes);
            DisabledWorkGiver(sb, backstory.DisabledWorkGivers);
            Trait(sb, "DisallowedTraits", backstory.disallowedTraits);
            Trait(sb, "forcedTraits", backstory.forcedTraits);
            Possessions(sb, backstory.possessions);
            IsPlayerColonyChildBackstory(sb, backstory.IsPlayerColonyChildBackstory);
            Shuffleable(sb, backstory.shuffleable);
            string result = sb.ToString();
            sb.Clear();
            return result;
        }
        private static void IsPlayerColonyChildBackstory(StringBuilder sb, bool isPlayerColonyChildBackstory)
        {
            Title(sb, "IsPlayerColonyChildBackstory");
            sb.AppendLine(isPlayerColonyChildBackstory ? "Yes".Translate() : "No".Translate());
        }
        private static void Possessions(StringBuilder sb, List<BackstoryThingDefCountClass> possessions)
        {
            if (possessions == null || possessions.Count <= 0)
            {
                return;
            }
            Title(sb, "Possessions");
            foreach (var possession in possessions)
            {
                sb.Append(possession.key.label.Translate()).Append('+').AppendLine(possession.value.ToString());
            }
        }
        private static void Shuffleable(StringBuilder sb, bool shuffleable)
        {
            Title(sb, "Shuffleable");
            sb.AppendLine(shuffleable ? "Yes".Translate() : "No".Translate());
        }
        private static void DisabledWorkGiver(StringBuilder sb, IEnumerable<WorkGiverDef> disabledWorkGivers)
        {
            if (disabledWorkGivers == null || !disabledWorkGivers.Any())
            {
                return;
            }
            Title(sb, "DisabledWorkGivers");
            foreach (var giver in disabledWorkGivers)
            {
                sb.AppendLine(giver.label.Translate());
            }
        }
        private static void Trait(StringBuilder sb, string titleKey, List<BackstoryTrait> disallowedTraits)
        {
            if (disallowedTraits == null || disallowedTraits.Count <= 0)
            {
                return;
            }
            Title(sb, titleKey);
            foreach (var trait in disallowedTraits)
            {
                List<TraitDegreeData> degreeDatas = trait.def.degreeDatas;
                if (trait.degree >= 0 && trait.degree <= degreeDatas.Count)
                {
                    sb.AppendLine(trait.def.degreeDatas[trait.degree].label);
                    continue;
                }
                foreach (var degreeData in degreeDatas)
                {
                    sb.AppendLine(degreeData.label);
                }


            }
        }
        private static void DisableWorks(StringBuilder sb, List<WorkTypeDef> disabledWorkTypes)
        {
            if (disabledWorkTypes == null || disabledWorkTypes.Count <= 0)
            {
                return;
            }
            Title(sb, "DisableWorks");
            foreach (var work in disabledWorkTypes)
            {
                sb.AppendLine(work.labelShort.Translate());
            }
        }
        private static void SkillGains(StringBuilder sb, Dictionary<SkillDef, int> skillGains)
        {
            if (skillGains == null || skillGains.Count <= 0)
            {
                return;
            }
            Title(sb, "SkillGains");
            foreach (var skill in skillGains)
            {
                string label = skill.Key.label.Translate();
                string valueString = skill.Value.ToString();
                string sign = skill.Value > 0 ? "+" : string.Empty;
                string full = label + " " + sign + valueString;
                LevelColor(sb, full, skill.Value);
            }
        }
        private static void LevelColor(StringBuilder sb, string text, int Level)
        {
            if (Level > 4)
            {
                Level = 4;
            }
            if (Level < -4)
            {
                Level = -4;
            }
            if (Level >= 0)
            {
                text = StringColor.FromColor(GoodColor[Level]).GetColorMessage(text);
            }
            else
            {
                text = StringColor.FromColor(BadColor[Math.Abs(Level)]).GetColorMessage(text);
            }
            sb.AppendLine(text);
        }
        private static void SpawnCategories(StringBuilder sb, List<string> spawnCategories)
        {
            if (spawnCategories == null || spawnCategories.Count == 0)
            {
                return;
            }
            Title(sb, "SpawnCategories");
            foreach (var spawCategory in spawnCategories)
            {
                sb.AppendLine(spawCategory.Translate());
            }

        }
        private static void BackstoryBasic(StringBuilder sb, BackstoryDef backstory)
        {
            //标题
            Title(sb, backstory.title);
            //短名称
            sb.AppendLine(backstory.titleShort);
            //描述
            string desc = backstory.baseDesc;
            Description(sb, desc);
        }
        private static void Description(StringBuilder sb, string desc)
        {
            string colorString = StringColor.Darkorange.GetColorMessage("Nomadicooer".Translate());
            string baseDesc = Regex.Replace(desc, @"(\[\w*\])|(\{\w*\})", colorString);
            sb.AppendLine(baseDesc);
        }
        public static string Traits(TraitDegreeDataRecord trait)
        {
            StringBuilder sb = new StringBuilder();
            TraitBasic(sb, trait);
            TraitValues(sb, trait);
            return sb.ToString();
        }
        private static void TraitValues(StringBuilder sb, TraitDegreeDataRecord trait)
        {
            TraitDegreeData degreeData = trait.TraitDegreeData;
            TraitDef traitDef = trait.TraitDef;
            ConflictingTraite(sb, traitDef);
            ConflictingPassions(sb, traitDef);
            ForcedPassions(sb, traitDef);
            DisableWorks(sb, traitDef.disabledWorkTypes);
            AllowedMeditationFocusTypes(sb, degreeData);
            AppendStrings(sb, traitDef.exclusionTags, "exclusionTags");
        }
        private static void AllowedMeditationFocusTypes(StringBuilder sb, TraitDegreeData degreeData)
        {
            List<MeditationFocusDef> types = degreeData.allowedMeditationFocusTypes;
            if (types == null || types.Count <= 0)
            {
                return;
            }
            Title(sb, "AllowedMeditationFocusTypes");
            foreach (var type in types)
            {
                sb.AppendLine(type.label);
            }
        }
        private static void ForcedPassions(StringBuilder sb, TraitDef traitDef)
        {
            AppendPassions(sb, "forcedPassions", traitDef.forcedPassions);
        }
        private static void AppendStrings(StringBuilder sb, List<string> strings, string titleKey)
        {
            if (strings == null || strings.Count == 0)
            {
                return;
            }
            Title(sb, titleKey);
            foreach (var item in strings)
            {
                sb.AppendLine(item);
            }
        }
        private static void ConflictingPassions(StringBuilder sb, TraitDef traitDef)
        {
            AppendPassions(sb, "conflictingPassions", traitDef.conflictingPassions);
        }
        private static void AppendPassions(StringBuilder sb, string titileKey, List<SkillDef> passions)
        {
            if (passions == null || passions.Count <= 0)
            {
                return;
            }
            Title(sb, titileKey);
            foreach (var item in passions)
            {
                sb.AppendLine(item.label);
            }
        }
        private static void ConflictingTraite(StringBuilder sb, TraitDef traitDef)
        {
            List<TraitDef> traits = traitDef.conflictingTraits;
            if (traits == null || traits.Count <= 0)
            {
                return;
            }
            Title(sb, "conflictingTraits");
            foreach (var item in traits)
            {
                List<TraitDegreeData> degreeDatas = item.degreeDatas;
                if (degreeDatas.Count <= 0)
                {
                    sb.AppendLine(item.label);
                    continue;
                }
                foreach (var traitDegree in degreeDatas)
                {
                    sb.AppendLine(traitDegree.label);
                }
            }
        }
        private static void TraitBasic(StringBuilder sb, TraitDegreeDataRecord trait)
        {
            Title(sb, trait.TraitDegreeData.label);
            Description(sb, trait.TraitDegreeData.description);
        }
    }
}
