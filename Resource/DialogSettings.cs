using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    public class DialogSettings : Window
    {
        //用于保存对话框滚动条的位置
        private Vector2 scrollPosition = Vector2.zero;
        private readonly static string SaveButtonText = "Save".Translate();
        private readonly static string RestButtonText = "Rest".Translate();
        private readonly static FilterSettings result = FilterSettings.Instance;
        private readonly ListViewSelector<BackstoryDef> childhoodBackstoriesListViewSelector;
        private readonly ListViewSelector<BackstoryDef> adulthoodBackstoriesListViewSelector;
        private readonly ListViewSelector<TraitDegreeDataRecord> traitsListViewSelector;
        public override Vector2 InitialSize => UIWidgets.DialogInitSize;
        public DialogSettings()
        {
            //添加关闭按钮
            doCloseButton = true;
            //添加关闭X按钮
            doCloseX = true;
            //设置点击窗体外关闭,不然会发生重绘现象
            closeOnClickedOutside = true;
            //当其它窗口打开的时候变灰
            grayOutIfOtherDialogOpen = false;
            //设置标题
            optionalTitle = "DialogSettings".Text();
            childhoodBackstoriesListViewSelector = GetChildhoodBackstoriesListViewSelector();
            adulthoodBackstoriesListViewSelector =
                GetAdulthoodBackSotoriesListViewSelector();
            traitsListViewSelector = GetTraitsListViewSelector();
        }
        public override void DoWindowContents(Rect inRect)
        {
            CreateSaveButton(inRect);
            CreateRestButton(inRect);
            scrollPosition = GUI.BeginScrollView(UIWidgets.ScrollViewPosition, scrollPosition, UIWidgets.ScrollViewContentPostion, false, false);
            UIWidgets.Begin();
            DrawMisc();
            DrawSkills();
            DrawChildhoodBackstories();
            DrawAdulthoodBackstories();
            DrawTraits();
            GUI.EndScrollView();
            UIWidgets.End();
        }
        private static void CreateRestButton(Rect inRect)
        {
            float height = inRect.height - FooterRowHeight/2-StandardMargin/2;
            bool r = Widgets.ButtonText(new Rect((inRect.width / 4f) * 3f - CloseButSize.x / 2f, height, CloseButSize.x, CloseButSize.y), RestButtonText);
            if (r)
            {
                result.RestSettings();
                result.Save();
            }
        }
        private static void CreateSaveButton(Rect inRect)
        {
            float height=inRect.height- FooterRowHeight/2-StandardMargin/2;
            bool r = Widgets.ButtonText(new Rect(inRect.width / 4f - CloseButSize.x / 2f, height, CloseButSize.x, CloseButSize.y), SaveButtonText);
            if (r)
            {
                result.Save();
                UIWidgets.ShowMessage<DialogSettings>("SaveSucceed".Translate());
            }
        }
        private void DrawTraits()
        {
            UIWidgets.DrawGroupTitle("Traits");
            result.Traits = UIWidgets.ListViewSelector(traitsListViewSelector);
        }
        private void DrawAdulthoodBackstories()
        {
            UIWidgets.DrawGroupTitle("AdulthoodBackstories");
            result.AdulthoodBackstories = UIWidgets.ListViewSelector(adulthoodBackstoriesListViewSelector);
        }
        private void DrawChildhoodBackstories()
        {
            UIWidgets.DrawGroupTitle("ChildHoodBackstories");
            result.ChildHoodBackstories = UIWidgets.ListViewSelector(childhoodBackstoriesListViewSelector);
        }
        private static void DrawSkills()
        {
            UIWidgets.DrawGroupTitle("Skills");
           // result.ShootingRange = UIWidgets.IntRangeSlider("Shooting", result.ShootingRange, 0, 20);
            SkillInfo skillInfo = new SkillInfo(result.ShootingRange, result.ShootingPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Shooting", skillInfo);
            result.ShootingRange = skillInfo.intRange;
            result.ShootingPassion = skillInfo.passion;
            //result.MeleeRange = UIWidgets.IntRangeSlider("Melee", result.MeleeRange, 0, 20);
            skillInfo = new SkillInfo(result.MeleeRange, result.MeleePassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Melee", skillInfo);
            result.MeleeRange = skillInfo.intRange;
            result.MeleePassion = skillInfo.passion;
            //result.ConstructionRange = UIWidgets.IntRangeSlider("Construction", result.ConstructionRange, 0, 20);
            skillInfo = new SkillInfo(result.ConstructionRange, result.ConstructionPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Construction", skillInfo);
            result.ConstructionRange = skillInfo.intRange;
            result.ConstructionPassion = skillInfo.passion;
            //result.MiningRange = UIWidgets.IntRangeSlider("Mining", result.MiningRange, 0, 20);
            skillInfo = new SkillInfo(result.MiningRange, result.MiningPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Mining", skillInfo);
            result.MiningRange = skillInfo.intRange;
            result.MiningPassion = skillInfo.passion;
            //result.CookingRange = UIWidgets.IntRangeSlider("Cooking", result.CookingRange, 0, 20);
            skillInfo = new SkillInfo(result.CookingRange, result.CookingPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Cooking", skillInfo);
            result.CookingRange = skillInfo.intRange;
            result.CookingPassion = skillInfo.passion;
            //result.PlantsRange = UIWidgets.IntRangeSlider("Plants", result.PlantsRange, 0, 20);
            skillInfo = new SkillInfo(result.PlantsRange, result.PlantsPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Plants", skillInfo);
            result.PlantsRange = skillInfo.intRange;
            result.PlantsPassion = skillInfo.passion;
            //result.AnimalsRange = UIWidgets.IntRangeSlider("Animals", result.AnimalsRange, 0, 20);
            skillInfo = new SkillInfo(result.AnimalsRange, result.AnimalsPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Animals", skillInfo);
            result.AnimalsRange = skillInfo.intRange;
            result.AnimalsPassion = skillInfo.passion;
            //result.CraftingRange = UIWidgets.IntRangeSlider("Crafting", result.CraftingRange, 0, 20);
            skillInfo = new SkillInfo(result.CraftingRange, result.CraftingPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Crafting", skillInfo);
            result.CraftingRange = skillInfo.intRange;
            result.CraftingPassion = skillInfo.passion;
            //result.ArtisticRange = UIWidgets.IntRangeSlider("Artistic", result.ArtisticRange, 0, 20);
            skillInfo = new SkillInfo(result.ArtisticRange, result.ArtisticPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Artistic", skillInfo);
            result.ArtisticRange = skillInfo.intRange;
            result.ArtisticPassion = skillInfo.passion;
            //result.MedicalRange = UIWidgets.IntRangeSlider("Medical", result.MedicalRange, 0, 20);
            skillInfo = new SkillInfo(result.MedicalRange, result.MedicalPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Medical", skillInfo);
            result.MedicalRange = skillInfo.intRange;
            result.MedicalPassion = skillInfo.passion;
            //result.SocialRange = UIWidgets.IntRangeSlider("Social", result.SocialRange, 0, 20);
            skillInfo = new SkillInfo(result.SocialRange, result.SocialPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Social", skillInfo);
            result.SocialRange = skillInfo.intRange;
            result.SocialPassion = skillInfo.passion;
            //result.IntellectualRange = UIWidgets.IntRangeSlider("Intellectual", result.IntellectualRange, 0, 20);
            skillInfo = new SkillInfo(result.IntellectualRange, result.IntellectualPassion);
            skillInfo = UIWidgets.SkillIntRangeSlider("Intellectual", skillInfo);
            result.IntellectualRange = skillInfo.intRange;
            result.IntellectualPassion = skillInfo.passion;

        }

        private static void DrawMisc()
        {
            //杂项
            UIWidgets.DrawGroupTitle("Misc");
            //筛选模式
            result.FilterMode = UIWidgets.RadioButtonGroup(result.FilterMode);
            //允许筛选的最大次数
            if (result.FilterMode == FilterMode.OneInMillion)
            {
                result.MaxTimes = UIWidgets.IntField("MaxTimes", result.MaxTimes, 100);
            }
            //匹配模式
            result.MatchMode = UIWidgets.RadioButtonGroup(result.MatchMode);
            //性别选择
            result.Gender = UIWidgets.RadioButtonGroup(result.Gender);
            //有无禁用工作状态
            if (result.FilterMode == FilterMode.OneInMillion)
            {
                result.DisableWorkState = UIWidgets.RadioButtonGroup(result.DisableWorkState);
            }
            //有无健康问题
            result.HealthState = UIWidgets.RadioButtonGroup(result.HealthState);
            //有无社会关系
            result.RelationshipState = UIWidgets.RadioButtonGroup(result.RelationshipState);
            //年龄范围
            result.AgeRange = UIWidgets.IntRangeSlider("Age", result.AgeRange, 0, 150);
        }
        private static ListViewSelector<BackstoryDef> GetChildhoodBackstoriesListViewSelector()
        {
            List<BackstoryDef> srcItems = Providers.Instance.ProviderBackstories.ChildhoodBackstories;
            List<BackstoryDef> dstItems = result.ChildHoodBackstories;
            ListViewSelector<BackstoryDef> selector
                = new ListViewSelector<BackstoryDef>(srcItems, dstItems, (backsotry) => backsotry.title, (backstory) => TooltipUtility.Backstory(backstory));
            return selector;
        }
        private static ListViewSelector<BackstoryDef> GetAdulthoodBackSotoriesListViewSelector()
        {
            List<BackstoryDef> srcItems = Providers.Instance.ProviderBackstories.AdulthoodBackstories;
            List<BackstoryDef> dstItems = result.AdulthoodBackstories;
            ListViewSelector<BackstoryDef> selector
                = new ListViewSelector<BackstoryDef>(srcItems, dstItems, (backsotry) => backsotry.title, (backstory) => TooltipUtility.Backstory(backstory));
            return selector;
        }
        private static ListViewSelector<TraitDegreeDataRecord> GetTraitsListViewSelector()
        {
            List<TraitDegreeDataRecord> srcItems = Providers.Instance.ProviderTraits.AllTraits;
            List<TraitDegreeDataRecord> dstItems = result.Traits;
            ListViewSelector<TraitDegreeDataRecord> selector
                = new ListViewSelector<TraitDegreeDataRecord>(srcItems, dstItems, (trait) => trait.TraitDegreeData.label, (trait) => TooltipUtility.Traits(trait));
            return selector;
        }
    }
}
