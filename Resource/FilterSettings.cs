using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    public class FilterSettings
    {
        private FilterSettings() { }
        private static FilterSettings? instance;
        private readonly Random random = new Random();
        public static FilterSettings Instance => instance ??= new FilterSettings();
        //最大随机次数
        private int maxTimes = 1000;
        //性别
        private Gender gender = Gender.None;
        //匹配模式
        private FilterMode filterMode = FilterMode.OneInMillion;
        //匹配模式
        private MatchMode matchMode = MatchMode.Any;
        //是否启用禁用工作状态
        private DisableWorkState disableWorkstate = DisableWorkState.Nothing;
        //是否具有健康状况
        private HealthState healthState = HealthState.Nothing;
        //是否具有关系状态
        private RelationshipState relationshipState = RelationshipState.Any;
        //年龄范围
        private IntRange ageRange = new IntRange(0, 100);
        //射击等级
        private IntRange shootingRange = new IntRange(0, 20);
        //格斗等级
        private IntRange meleeRange = new IntRange(0, 20);
        //建造等级
        private IntRange constructionRange = new IntRange(0, 20);
        //采矿等级
        private IntRange miningRange = new IntRange(0, 20);
        //烹饪等级
        private IntRange cookingRange = new IntRange(0, 20);
        //种植等级
        private IntRange plantsRange = new IntRange(0, 20);
        //驯兽等级
        private IntRange animalsRange = new IntRange(0, 20);
        //手工等级
        private IntRange craftingRange = new IntRange(0, 20);
        //艺术等级
        private IntRange artisticRange = new IntRange(0, 20);
        //医疗等级
        private IntRange medicalRange = new IntRange(0, 20);
        //射击等级
        private IntRange socialRange = new IntRange(0, 20);
        //智识等级
        private IntRange intellectualRange = new IntRange(0, 20);
        //幼年背景故事
        private List<BackstoryDef> childHoodBackstories = new List<BackstoryDef>();
        //成年背景故事
        private List<BackstoryDef> adulthoodBackstories = new List<BackstoryDef>();
        //特性
        private List<TraitDegreeDataRecord> traits = new List<TraitDegreeDataRecord>();
        public int MaxTimes { get => this.maxTimes; set => this.maxTimes = value; }
        public Gender Gender { get => this.gender; set => this.gender = value; }
        public IntRange AgeRange { get => this.ageRange; set => this.ageRange = value; }
        public IntRange ShootingRange { get => this.shootingRange; set => this.shootingRange = value; }
        public IntRange MeleeRange { get => this.meleeRange; set => this.meleeRange = value; }
        public IntRange ConstructionRange { get => this.constructionRange; set => this.constructionRange = value; }
        public IntRange MiningRange { get => this.miningRange; set => this.miningRange = value; }
        public IntRange CookingRange { get => this.cookingRange; set => this.cookingRange = value; }
        public IntRange PlantsRange { get => this.plantsRange; set => this.plantsRange = value; }
        public IntRange AnimalsRange { get => this.animalsRange; set => this.animalsRange = value; }
        public IntRange CraftingRange { get => this.craftingRange; set => this.craftingRange = value; }
        public IntRange ArtisticRange { get => this.artisticRange; set => this.artisticRange = value; }
        public IntRange MedicalRange { get => this.medicalRange; set => this.medicalRange = value; }
        public IntRange SocialRange { get => this.socialRange; set => this.socialRange = value; }
        public IntRange IntellectualRange { get => this.intellectualRange; set => this.intellectualRange = value; }
        public List<BackstoryDef> ChildHoodBackstories { get => this.childHoodBackstories; set => this.childHoodBackstories = value; }
        public List<BackstoryDef> AdulthoodBackstories { get => this.adulthoodBackstories; set => this.adulthoodBackstories = value; }
        public List<TraitDegreeDataRecord> Traits { get => this.traits; set => this.traits = value; }
        public MatchMode MatchMode { get => this.matchMode; set => this.matchMode = value; }
        public FilterMode FilterMode { get => this.filterMode; set => this.filterMode = value; }
        internal DisableWorkState DisableWorkstate { get => this.disableWorkstate; set => this.disableWorkstate = value; }
        public HealthState HealthState { get => this.healthState; set => this.healthState = value; }
        public RelationshipState RelationshipState { get => this.relationshipState; set => this.relationshipState = value; }
        public Pawn RsetPawn(Pawn pawn)
        {
            RsetGender(pawn);
            RsetAge(pawn);
            RsetSkills(pawn);
            RsetTrait(pawn);
            RestDisableWork(pawn);
            RestHealth(pawn);
            return pawn;
        }

        private void RestHealth(Pawn pawn)
        {
            if (this.healthState == HealthState.Nothing)
            {
                pawn.health.RemoveAllHediffs();
            }
        }

        private void RestDisableWork(Pawn pawn)
        {
            List<WorkTypeDef> workTypes = pawn.GetDisabledWorkTypes();
            if (workTypes == null)
            {
                return;
            }
            if (this.disableWorkstate == DisableWorkState.Nothing)
            {
                workTypes.Clear();
            }
        }

        private void RsetTrait(Pawn pawn)
        {
            if (this.traits.Count == 0)
            {
                return;
            }
            RestTraitAny(pawn);
            RestTraitAll(pawn);
        }

        private void RestTraitAll(Pawn pawn)
        {
            if (this.matchMode != MatchMode.All)
            {
                return;
            }
            TraitSet traitSet = pawn.story.traits;
            if (this.traits.Count < traitSet.allTraits.Count)
            {
                for (int i = 0; i < this.traits.Count; i++)
                {
                    traitSet.allTraits.RemoveLast();
                }
            }
            foreach (var item in this.traits)
            {
                traitSet.allTraits.Add(new Trait(item.TraitDef, item.Degree, true));
            }
        }

        private void RestTraitAny(Pawn pawn)
        {
            if (this.matchMode != MatchMode.Any)
            {
                return;
            }
            TraitSet traitSet = pawn.story.traits;
            List<TraitDegreeDataRecord> traitsSetting = this.traits;
            foreach (var item in traitsSetting)
            {
                if (traitSet.HasTrait(item.TraitDef))
                {
                    return;
                }
            }
            if (traitSet.allTraits.Count > 0)
            {
                traitSet.allTraits.RemoveLast();
            }
            int index = random.Next(traitsSetting.Count);
            TraitDegreeDataRecord record = traitsSetting[index];
            traitSet.GainTrait(new Trait(record.TraitDef, record.Degree, true), true);
        }

        private void RsetSkills(Pawn pawn)
        {
            List<SkillRecord>? skills = pawn.skills?.skills;
            if (skills == null)
            {
                return;
            }
            foreach (var skillRecord in skills)
            {
                int level = skillRecord.GetLevel(true);
                string skillName = skillRecord.def.defName;
                if (skillName == SkillName.Shooting.ToString())
                {
                    SetSkill(skillRecord, level, this.shootingRange);
                }
                else if (skillName == SkillName.Melee.ToString())
                {
                    SetSkill(skillRecord, level, this.MedicalRange);
                }
                else if (skillName == SkillName.Construction.ToString())
                {
                    SetSkill(skillRecord, level, this.constructionRange);
                }
                else if (skillName == SkillName.Mining.ToString())
                {
                    SetSkill(skillRecord, level, this.MiningRange);
                }
                else if (skillName == SkillName.Cooking.ToString())
                {
                    SetSkill(skillRecord, level, this.CookingRange);
                }
                else if (skillName == SkillName.Plants.ToString())
                {
                    SetSkill(skillRecord, level, this.PlantsRange);
                }
                else if (skillName == SkillName.Animals.ToString())
                {
                    SetSkill(skillRecord, level, this.AnimalsRange);
                }
                else if (skillName == SkillName.Crafting.ToString())
                {
                    SetSkill(skillRecord, level, this.CraftingRange);
                }
                else if (skillName == SkillName.Artistic.ToString())
                {
                    SetSkill(skillRecord, level, this.ArtisticRange);
                }
                else if (skillName == SkillName.Medicine.ToString())
                {
                    SetSkill(skillRecord, level, this.MedicalRange);
                }
                else if (skillName == SkillName.Social.ToString())
                {
                    SetSkill(skillRecord, level, this.SocialRange);
                }
                else if (skillName == SkillName.Intellectual.ToString())
                {
                    SetSkill(skillRecord, level, this.IntellectualRange);
                }

            }
        }

        private void SetSkill(SkillRecord skillRecord, int level, IntRange range)
        {
            if (!range.InRange(level))
            {
                skillRecord.Level = random.Next(range.min, range.max);
            }
        }

        private void RsetAge(Pawn pawn)
        {
            int age = pawn.ageTracker.AgeBiologicalYears;
            bool r = this.ageRange.InRange(age);
            if (r) return;
            int ageYear = random.Next(this.ageRange.min, this.ageRange.max);
            pawn.ageTracker.AgeBiologicalTicks = ageYear * 3600000;
        }

        private void RsetGender(Pawn pawn)
        {
            if (this.gender == Gender.None) return;
            pawn.gender = this.Gender;
        }

        public bool Match(Pawn pawn)
        {
            if (!MatchGender(pawn)) return false;
            if (!MatchAge(pawn)) return false;
            if (!MatchDisableWork(pawn)) return false;
            if (!MatchHealth(pawn)) return false;
            if (!MatchRelation(pawn)) return false;
            if (!MathchSkills(pawn)) return false;
            if (!MathchBackstory(pawn)) return false;
            if (!MathchTrait(pawn)) return false;
            return true;
        }
        private bool MatchHealth(Pawn pawn)
        {
            if (healthState == HealthState.Any) return true;
            int? count = pawn.health?.hediffSet?.hediffs?.Count;
            count = count == null ? 0 : count;
            if (count == 0 && healthState == HealthState.Nothing) return true;
            else if (count > 0 && healthState == HealthState.Exsit) return true;
            return false;
        }
        private bool MatchRelation(Pawn pawn)
        {
            if (relationshipState == RelationshipState.Any) return true;
            bool r = pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe;
            if (!r && relationshipState == RelationshipState.Nothing) return true;
            if (r && relationshipState == RelationshipState.Exsit) return true;
            return false;
        }
        private bool MatchDisableWork(Pawn pawn)
        {
            if (disableWorkstate == DisableWorkState.Any) return true;
            List<WorkTypeDef> workTypes = pawn.GetDisabledWorkTypes();
            bool hasDisableWork = workTypes != null && workTypes.Count > 0;
            if (hasDisableWork && disableWorkstate == DisableWorkState.Exsit)
            {
                return true;
            }
            else if (!hasDisableWork && disableWorkstate == DisableWorkState.Nothing)
            {
                return true;
            }
            return false;
        }
        private bool MathchTrait(Pawn pawn)
        {
            if (this.matchMode == MatchMode.All)
            {
                return MatchTraitWithAll(pawn);
            }
            return MatchTraitWithAny(pawn);
        }
        private bool MatchTraitWithAll(Pawn pawn)
        {
            if (traits.Count <= 0)
            {
                return true;
            }
            TraitSet traitSet = pawn.story.traits;
            foreach (var trait in this.traits)
            {
                bool r = traitSet.HasTrait(trait.TraitDef, trait.Degree);
                if (!r) return false;

            }
            return true;
        }
        private bool MatchTraitWithAny(Pawn pawn)
        {
            if (traits.Count <= 0)
            {
                return true;
            }
            TraitSet traitSet = pawn.story.traits;
            foreach (var trait in this.traits)
            {
                bool r = traitSet.HasTrait(trait.TraitDef, trait.Degree);
                if (r) return true;

            }
            return false;
        }
        private bool MathchBackstory(Pawn pawn)
        {
            if (!MatchChildBackstory(pawn)) return false;
            if (!MatchAdultBackstory(pawn)) return false;
            return true;
        }
        private bool MatchAdultBackstory(Pawn pawn)
        {
            if (adulthoodBackstories == null || adulthoodBackstories.Count <= 0) return true;
            BackstoryDef adulthood = pawn.story.Adulthood;
            if (adulthood == null) return false;
            if (adulthoodBackstories.Exists((story) => story.defName == adulthood.defName)) return true;
            return false;
        }
        private bool MatchChildBackstory(Pawn pawn)
        {
            if (childHoodBackstories == null || childHoodBackstories.Count <= 0) return true;
            BackstoryDef childHood = pawn.story.Childhood;
            if (childHood == null) return false;
            if (childHoodBackstories.Exists((story) => story.defName == childHood.defName)) return true;
            return false;
        }
        private bool MathchSkills(Pawn pawn)
        {
            List<SkillRecord>? skills = pawn.skills?.skills;
            if (skills == null) return false;
            foreach (var skill in skills)
            {
                int level = skill.GetLevel(true);
                string skillName = skill.def.defName;
                if (!IsSkillInRange(level, skillName))
                {
                    return false;
                }

            }
            return true;
        }
        private bool IsSkillInRange(int level, string skillName)
        {
            bool r;
            if (skillName == SkillName.Shooting.ToString())
            {
                r = shootingRange.InRange(level);
            }
            else if (skillName == SkillName.Melee.ToString())
            {
                r = meleeRange.InRange(level);
            }
            else if (skillName == SkillName.Construction.ToString())
            {
                r = constructionRange.InRange(level);
            }
            else if (skillName == SkillName.Mining.ToString())
            {
                r = miningRange.InRange(level);
            }
            else if (skillName == SkillName.Cooking.ToString())
            {
                r = cookingRange.InRange(level);
            }
            else if (skillName == SkillName.Plants.ToString())
            {
                r = plantsRange.InRange(level);
            }
            else if (skillName == SkillName.Animals.ToString())
            {
                r = animalsRange.InRange(level);
            }
            else if (skillName == SkillName.Crafting.ToString())
            {
                r = craftingRange.InRange(level);
            }
            else if (skillName == SkillName.Artistic.ToString())
            {
                r = artisticRange.InRange(level);
            }
            else if (skillName == SkillName.Medicine.ToString())
            {
                r = medicalRange.InRange(level);
            }
            else if (skillName == SkillName.Social.ToString())
            {
                r = socialRange.InRange(level);
            }
            else if (skillName == SkillName.Intellectual.ToString())
            {
                r = intellectualRange.InRange(level);
            }
            else
            {
                r = false;
            }
            return r;
        }
        private bool MatchGender(Pawn pawn)
        {
            if (this.gender == Gender.None)
            {
                return true;
            }
            return this.gender == pawn.gender;
        }
        private bool MatchAge(Pawn pawn)
        {
            return ageRange.InRange(pawn.ageTracker.AgeBiologicalYears);
        }
    }
}
