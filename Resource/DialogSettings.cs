using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
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
            grayOutIfOtherDialogOpen = true;
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


        private void CreateRestButton(Rect inRect)
        {
            float height = inRect.height - FooterRowHeight/2-StandardMargin/2;
            bool r = Widgets.ButtonText(new Rect((inRect.width / 4f) * 3f - CloseButSize.x / 2f, height, CloseButSize.x, CloseButSize.y), RestButtonText);
            if (r)
            {
                result.Rest();
            }
        }
        public override void PreClose()
        {
            result.Save();
        }
        private void CreateSaveButton(Rect inRect)
        {
            float height=inRect.height- FooterRowHeight/2-StandardMargin/2;
            bool r = Widgets.ButtonText(new Rect(inRect.width / 4f - CloseButSize.x / 2f, height, CloseButSize.x, CloseButSize.y), SaveButtonText);
            if (r)
            {
                result.Save();
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
            result.ShootingRange = UIWidgets.IntRangeSlider("Shooting", result.ShootingRange, 0, 20);
            result.MeleeRange = UIWidgets.IntRangeSlider("Melee", result.MeleeRange, 0, 20);
            result.ConstructionRange = UIWidgets.IntRangeSlider("Construction", result.ConstructionRange, 0, 20);
            result.MiningRange = UIWidgets.IntRangeSlider("Mining", result.MiningRange, 0, 20);
            result.CookingRange = UIWidgets.IntRangeSlider("Cooking", result.CookingRange, 0, 20);
            result.PlantsRange = UIWidgets.IntRangeSlider("Plants", result.PlantsRange, 0, 20);
            result.AnimalsRange = UIWidgets.IntRangeSlider("Animals", result.AnimalsRange, 0, 20);
            result.CraftingRange = UIWidgets.IntRangeSlider("Crafting", result.CraftingRange, 0, 20);
            result.ArtisticRange = UIWidgets.IntRangeSlider("Artistic", result.ArtisticRange, 0, 20);
            result.MedicalRange = UIWidgets.IntRangeSlider("Medical", result.MedicalRange, 0, 20);
            result.SocialRange = UIWidgets.IntRangeSlider("Social", result.SocialRange, 0, 20);
            result.IntellectualRange = UIWidgets.IntRangeSlider("Intellectual", result.IntellectualRange, 0, 20);
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
