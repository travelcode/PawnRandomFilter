using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    public class FilterSettings
    {
        private FilterSettings()
        {
        }
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
        private DisableWorkState disableWorkState = DisableWorkState.Nothing;
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
        public int MaxTimes { get => maxTimes; set => maxTimes = value; }
        public Gender Gender { get => gender; set => gender = value; }
        public IntRange AgeRange { get => ageRange; set => ageRange = value; }
        public IntRange ShootingRange
        {
            get => shootingRange;
            set
            {
                CheckSkill(value);
                shootingRange = value;
            }
        }

        public IntRange MeleeRange
        {
            get => meleeRange; set
            {
                CheckSkill(value);
                meleeRange = value;
            }
        }
        public IntRange ConstructionRange
        {
            get => constructionRange; set
            {
                CheckSkill(value);
                constructionRange = value;
            }
        }
        public IntRange MiningRange
        {
            get => miningRange; set
            {
                CheckSkill(value);
                miningRange = value;
            }
        }
        public IntRange CookingRange
        {
            get => cookingRange; set
            {
                CheckSkill(value);
                cookingRange = value;
            }
        }
        public IntRange PlantsRange
        {
            get => plantsRange; set
            {
                CheckSkill(value);
                plantsRange = value;
            }
        }
        public IntRange AnimalsRange
        {
            get => animalsRange; set
            {
                CheckSkill(value);
                animalsRange = value;
            }
        }
        public IntRange CraftingRange
        {
            get => craftingRange; set
            {
                CheckSkill(value);
                craftingRange = value;
            }
        }
        public IntRange ArtisticRange
        {
            get => artisticRange; set
            {
                CheckSkill(value);
                artisticRange = value;
            }
        }
        public IntRange MedicalRange
        {
            get => medicalRange; set
            {
                CheckSkill(value);
                medicalRange = value;
            }
        }
        public IntRange SocialRange
        {
            get => socialRange; set
            {
                CheckSkill(value);
                socialRange = value;
            }
        }
        public IntRange IntellectualRange
        {
            get => intellectualRange; set
            {
                CheckSkill(value);
                intellectualRange = value;
            }
        }
        public List<BackstoryDef> ChildHoodBackstories
        {
            get => childHoodBackstories; set
            {
                CheckBackstories(value);
                childHoodBackstories = value;
            }
        }

        private void CheckBackstories(List<BackstoryDef> value)
        {
            List<BackstoryDef> backstories = value;
            if (filterMode != FilterMode.OneInMillion)
            {
                return;
            }
            if (disableWorkState != DisableWorkState.Nothing)
            {
                return;
            }
            foreach (var backstory in backstories)
            {
                if (backstory.DisabledWorkTypes.Count <= 0)
                {
                    continue;
                }
                string text = "warn.backstoryHasDisableWorktype".Translate();
                text = text.Replace("{backstory}", backstory.title);
                text = StringColor.OrangeRed.GetColorMessage(text);
                UIWidgets.ShowMessage<DialogSettings>(text);
            }
        }

        public List<BackstoryDef> AdulthoodBackstories
        {
            get => adulthoodBackstories; set
            {
                CheckBackstories(value);
                adulthoodBackstories = value;
            }
        }
        public List<TraitDegreeDataRecord> Traits
        {
            get => traits; set
            {
                CheckTraits(value);
                traits = value;
            }
        }

        private  void CheckTraits(List<TraitDegreeDataRecord> value)
        {
            if (filterMode != FilterMode.OneInMillion)
            {
                return;
            }
            if (disableWorkState != DisableWorkState.Nothing)
            {
                return;
            }
            foreach (var tddr in value)
            {
                if (tddr.TraitDef.disabledWorkTypes.Count <= 0)
                {
                    continue;
                }
                string text = "warn.TraitHasDisableWorkType".Translate();
                text = text.Replace("{trait}", tddr.TraitDef.label.Translate());
                text = StringColor.OrangeRed.GetColorMessage(text);
                UIWidgets.ShowMessage<DialogSettings>(text);
            }
        }

        public MatchMode MatchMode { get => matchMode; set => matchMode = value; }
        public FilterMode FilterMode { get => filterMode; set => filterMode = value; }
        internal DisableWorkState DisableWorkState { get => disableWorkState; set => disableWorkState = value; }
        public HealthState HealthState { get => healthState; set => healthState = value; }
        public RelationshipState RelationshipState { get => relationshipState; set => relationshipState = value; }
        public Pawn RsetPawn(Pawn pawn)
        {
            RestBackstory(pawn);
            RsetGender(pawn);
            RsetAge(pawn);
            RsetSkills(pawn);
            RsetTrait(pawn);
            RestDisableWork(pawn);
            RestHealth(pawn);
            RestRelations(pawn);
            return pawn;
        }
        private static void RestRelations(Pawn pawn)
        {
            pawn.relations.ClearAllRelations();
        }

        private void RestBackstory(Pawn pawn)
        {
            RestChildStory(pawn);
            RestAdultStory(pawn);
        }
        private void RestChildStory(Pawn pawn)
        {
            List<BackstoryDef> stories = childHoodBackstories;
            if (stories.Count <= 0) return;
            BackstoryDef childStory = pawn.story.Childhood;
            if (childStory == null) return;
            if (stories.Exists((story) => story.defName == childStory.defName)) return;
            int index = random.Next(stories.Count);
            pawn.story.Childhood = stories[index];
        }
        private void RestAdultStory(Pawn pawn)
        {
            List<BackstoryDef> stories = adulthoodBackstories;
            if (stories.Count <= 0) return;
            BackstoryDef pawnStory = pawn.story.Childhood;
            if (pawnStory == null) return;
            if (stories.Exists((story) => story.defName == pawnStory.defName)) return;
            int index = random.Next(stories.Count);
            pawn.story.Adulthood = stories[index];
        }
        private void RestHealth(Pawn pawn)
        {
            if (healthState == HealthState.Nothing)
            {
                pawn.health.RemoveAllHediffs();
            }
        }
        private static void RestDisableWork(Pawn pawn)
        {
            List<BackstoryDef> stories = pawn.story.AllBackstories;
            foreach (var story in stories)
            {
                story.workDisables = WorkTags.None;
            }
        }
        private void RsetTrait(Pawn pawn)
        {
            if (traits.Count == 0)
            {
                return;
            }
            RestTraitAny(pawn);
            RestTraitAll(pawn);
        }
        private void RestTraitAll(Pawn pawn)
        {
            if (matchMode != MatchMode.All)
            {
                return;
            }
            TraitSet traitSet = pawn.story.traits;
            traitSet.allTraits.Clear();
            foreach (var item in traits)
            {
                traitSet.allTraits.Add(new Trait(item.TraitDef, item.Degree, true));
            }
        }
        private void RestTraitAny(Pawn pawn)
        {
            if (matchMode != MatchMode.Any)
            {
                return;
            }
            TraitSet traitSet = pawn.story.traits;
            List<TraitDegreeDataRecord> traitsSetting = traits;
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
                    SetSkill(skillRecord, level, shootingRange);
                }
                else if (skillName == SkillName.Melee.ToString())
                {
                    SetSkill(skillRecord, level, MedicalRange);
                }
                else if (skillName == SkillName.Construction.ToString())
                {
                    SetSkill(skillRecord, level, constructionRange);
                }
                else if (skillName == SkillName.Mining.ToString())
                {
                    SetSkill(skillRecord, level, MiningRange);
                }
                else if (skillName == SkillName.Cooking.ToString())
                {
                    SetSkill(skillRecord, level, CookingRange);
                }
                else if (skillName == SkillName.Plants.ToString())
                {
                    SetSkill(skillRecord, level, PlantsRange);
                }
                else if (skillName == SkillName.Animals.ToString())
                {
                    SetSkill(skillRecord, level, AnimalsRange);
                }
                else if (skillName == SkillName.Crafting.ToString())
                {
                    SetSkill(skillRecord, level, CraftingRange);
                }
                else if (skillName == SkillName.Artistic.ToString())
                {
                    SetSkill(skillRecord, level, ArtisticRange);
                }
                else if (skillName == SkillName.Medicine.ToString())
                {
                    SetSkill(skillRecord, level, MedicalRange);
                }
                else if (skillName == SkillName.Social.ToString())
                {
                    SetSkill(skillRecord, level, SocialRange);
                }
                else if (skillName == SkillName.Intellectual.ToString())
                {
                    SetSkill(skillRecord, level, IntellectualRange);
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
            bool r = ageRange.InRange(age);
            if (r) return;
            int ageYear = random.Next(ageRange.min, ageRange.max);
            pawn.ageTracker.AgeBiologicalTicks = ageYear * 3600000;
        }
        private void RsetGender(Pawn pawn)
        {
            if (gender == Gender.None) return;
            pawn.gender = Gender;
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
            if (disableWorkState == DisableWorkState.Any) return true;
            List<WorkTypeDef> workTypes = pawn.GetDisabledWorkTypes();
            bool hasDisableWork = workTypes != null && workTypes.Count > 0;
            if (hasDisableWork && disableWorkState == DisableWorkState.Exsit)
            {
                return true;
            }
            else if (!hasDisableWork && disableWorkState == DisableWorkState.Nothing)
            {
                return true;
            }
            return false;
        }
        private bool MathchTrait(Pawn pawn)
        {
            if (matchMode == MatchMode.All)
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
            foreach (var trait in traits)
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
            foreach (var trait in traits)
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
            if (gender == Gender.None)
            {
                return true;
            }
            return gender == pawn.gender;
        }
        private bool MatchAge(Pawn pawn)
        {
            return ageRange.InRange(pawn.ageTracker.AgeBiologicalYears);
        }
        public void Save()
        {
            try
            {
                XmlDocument doc = CreateDoc();
                XmlElement root = CreateRoot(doc);
                CreateMisc(doc, root);
                CreateSkills(doc, root);
                CreateBackStories(doc, root, "childHoodBackstories", this.childHoodBackstories);
                CreateBackStories(doc, root, "adulthoodBackstories", this.adulthoodBackstories);
                CreateTraits(doc, root);
                doc.Save(ModPathUtility.FilterSettingsPath);
            }
            catch (Exception e)
            {
                Logger.Warn("Save the pawn random filter settings file failed.");
                Logger.Warn(e.Message);
            }

        }

        private void CreateTraits(XmlDocument doc, XmlElement root)
        {
            XmlElement traitsElement = doc.CreateElement("traits");
            root.AppendChild(traitsElement);
            foreach (var trait in this.traits)
            {
                XmlElement traitElement = doc.CreateElement("trait");
                traitsElement.AppendChild(traitElement);
                CreateChildElement(doc, traitElement, "defName", trait.TraitDef.defName);
                CreateChildElement(doc, traitElement, "degree", trait.Degree);
            }
        }

        private static void CreateBackStories(XmlDocument doc, XmlElement root, string name, List<BackstoryDef> items)
        {
            XmlElement parent = doc.CreateElement(name);
            foreach (var item in items)
            {
                CreateChildElement(doc, parent, "defName", item.defName);
            }
            root.AppendChild(parent);
        }

        private void CreateSkills(XmlDocument doc, XmlElement root)
        {
            XmlElement skills = doc.CreateElement("skills");
            CreateRangeElement(doc, skills, "shootingRange", this.shootingRange);
            CreateRangeElement(doc, skills, "meleeRange", this.meleeRange);
            CreateRangeElement(doc, skills, "constructionRange", this.constructionRange);
            CreateRangeElement(doc, skills, "miningRange", this.miningRange);
            CreateRangeElement(doc, skills, "cookingRange", this.cookingRange);
            CreateRangeElement(doc, skills, "plantsRange", this.plantsRange);
            CreateRangeElement(doc, skills, "animalsRange", this.animalsRange);
            CreateRangeElement(doc, skills, "craftingRange", this.craftingRange);
            CreateRangeElement(doc, skills, "artisticRange", this.artisticRange);
            CreateRangeElement(doc, skills, "medicalRange", this.medicalRange);
            CreateRangeElement(doc, skills, "socialRange", this.socialRange);
            CreateRangeElement(doc, skills, "intellectualRange", this.intellectualRange);
            root.AppendChild(skills);
        }

        private void CreateMisc(XmlDocument doc, XmlElement root)
        {
            XmlElement misc = doc.CreateElement("misc");
            CreateChildElement(doc, misc, "filterMode", this.filterMode);
            CreateChildElement(doc, misc, "maxTimes", this.maxTimes);
            CreateChildElement(doc, misc, "matchMode", this.matchMode);
            CreateChildElement(doc, misc, "gender", this.gender);
            CreateChildElement(doc, misc, "disableWorkState", this.disableWorkState);
            CreateChildElement(doc, misc, "healthState", this.healthState);
            CreateChildElement(doc, misc, "relationshipState", this.relationshipState);
            CreateRangeElement(doc, misc, "ageRange", this.ageRange);
            root.AppendChild(misc);
        }
        private static XmlElement CreateRoot(XmlDocument doc)
        {
            XmlElement root = doc.CreateElement("settings");
            doc.AppendChild(root);
            return root;
        }

        private static XmlDocument CreateDoc()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xd = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xd);
            return doc;
        }
        private static void CreateRangeElement(XmlDocument doc, XmlElement misc, string rangeName, IntRange range)
        {
            XmlElement rangeElement = doc.CreateElement(rangeName);
            CreateChildElement(doc, rangeElement, "min", range.min);
            CreateChildElement(doc, rangeElement, "max", range.max);
            misc.AppendChild(rangeElement);
        }

        private static void CreateChildElement(XmlDocument doc, XmlElement parent, string name, object data)
        {
            XmlElement ele = doc.CreateElement(name);
            ele.InnerText = data.ToString();
            parent.AppendChild(ele);
        }

        public void LoadSettings()
        {
            if (!File.Exists(ModPathUtility.FilterSettingsPath))
            {
                Logger.Debug("load pawn random filter settings file failed");
                return;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ModPathUtility.FilterSettingsPath);
                XmlNode root = doc.DocumentElement;
                ParseMisc(root);
                ParseSkills(root);
                ParseBacksoties(root, "childHoodBackstories", this.childHoodBackstories);
                ParseBacksoties(root, "adulthoodBackstories", this.adulthoodBackstories);
                ParseTraits(root);
            }
            catch (Exception e)
            {
                Logger.Warn("parse pawn random filter settings file failed");
                Logger.Warn(e.Message);
            }

        }

        private void ParseTraits(XmlNode root)
        {
            XmlNode traits = root.SelectSingleNode("traits");
            this.traits.Clear();
            //trait列表
            XmlNodeList childNodes = traits.ChildNodes;
            foreach (var childNode in childNodes)
            {
                if (!(childNode is XmlNode)) continue;
                if (!(childNode is XmlNode node)) continue;
                string defName = node.SelectSingleNode("defName").InnerText;
                string degreeStr = node.SelectSingleNode("degree").InnerText;
                int degree = int.Parse(degreeStr);
                TraitDef traitDef = DefDatabase<TraitDef>.GetNamedSilentFail(defName);
                TraitDegreeData data = traitDef.degreeDatas[degree];
                TraitDegreeDataRecord record = new TraitDegreeDataRecord(traitDef, data);
                this.traits.Add(record);
            }
        }

        private static void ParseBacksoties(XmlNode root, string name, List<BackstoryDef> backstories)
        {
            backstories.Clear();
            XmlNode storiesNode = root.SelectSingleNode(name);
            XmlNodeList nodes = storiesNode.ChildNodes;
            foreach (var node in nodes)
            {
                if (!(node is XmlNode)) continue;
                if (!(node is XmlNode xmlNode)) continue;
                BackstoryDef story = DefDatabase<BackstoryDef>.GetNamedSilentFail(xmlNode.InnerText);
                backstories.Add(story);
            }
        }

        private void ParseSkills(XmlNode root)
        {
            XmlNode skillsNode = root.SelectSingleNode("skills");
            ParseRange(skillsNode, "shootingRange", ref this.shootingRange);
            ParseRange(skillsNode, "meleeRange", ref this.meleeRange);
            ParseRange(skillsNode, "constructionRange   ", ref this.constructionRange);
            ParseRange(skillsNode, "miningRange", ref this.miningRange);
            ParseRange(skillsNode, "cookingRange", ref this.cookingRange);
            ParseRange(skillsNode, "plantsRange", ref this.plantsRange);
            ParseRange(skillsNode, "animalsRange", ref this.animalsRange);
            ParseRange(skillsNode, "craftingRange", ref this.craftingRange);
            ParseRange(skillsNode, "medicalRange", ref this.medicalRange);
            ParseRange(skillsNode, "socialRange", ref this.socialRange);
            ParseRange(skillsNode, "intellectualRange", ref this.intellectualRange);
        }

        private void ParseMisc(XmlNode root)
        {
            XmlNode misc = root.SelectSingleNode("misc");
            string name = misc.SelectSingleNode("filterMode").InnerText;
            bool r = Enum.TryParse(name, out this.filterMode);
            if (!r) this.filterMode = FilterMode.OneInMillion;
            name = misc.SelectSingleNode("matchMode").InnerText;
            r = Enum.TryParse(name, out this.matchMode);
            if (!r) this.matchMode = MatchMode.Any;
            name = misc.SelectSingleNode("gender").InnerText;
            r = Enum.TryParse(name, out this.gender);
            if (!r) this.gender = Gender.None;
            name = misc.SelectSingleNode("disableWorkState").InnerText;
            r = Enum.TryParse(name, out this.disableWorkState);
            if (!r) this.disableWorkState = DisableWorkState.Any;
            name = misc.SelectSingleNode("healthState").InnerText;
            r = Enum.TryParse(name, out this.healthState);
            if (!r) this.healthState = HealthState.Any;
            name = misc.SelectSingleNode("relationshipState").InnerText;
            r = Enum.TryParse(name, out this.relationshipState);
            if (!r) this.relationshipState = RelationshipState.Any;
            ParseRange(misc, "ageRange", ref this.ageRange);
        }

        private static void ParseRange(XmlNode parent, string name, ref IntRange range)
        {
            XmlNode rangeNode = parent.SelectSingleNode(name);
            XmlNode minNode = rangeNode.SelectSingleNode("min");
            int min = int.Parse(minNode.InnerText);
            XmlNode maxNode = rangeNode.SelectSingleNode("max");
            int max = int.Parse(maxNode.InnerText);
            range.min = min;
            range.max = max;
        }
        private void CheckSkill(IntRange intRange)
        {
            if (filterMode != FilterMode.OneInMillion)
            {
                return;
            }
            if (intRange.min >= 15)
            {
                string text = StringColor.OrangeRed.GetColorMessage("warn.maxRange".Translate());
                UIWidgets.ShowMessage<DialogSettings>(text);
            }
        }
        internal void Rest()
        {
            //最大随机次数
            this.maxTimes = 1000;
            //性别
            this.Gender = Gender.None;
            //匹配模式
            this.filterMode = FilterMode.OneInMillion;
            //匹配模式
            this.matchMode = MatchMode.Any;
            //是否启用禁用工作状态
            this.disableWorkState = DisableWorkState.Nothing;
            //是否具有健康状况
            this.healthState = HealthState.Nothing;
            //是否具有关系状态
            this.relationshipState = RelationshipState.Any;
            //年龄范围
            this.ageRange = new IntRange(0, 100);
            //射击等级
            this.shootingRange = new IntRange(0, 20);
            //格斗等级
            this.meleeRange = new IntRange(0, 20);
            //建造等级
            this.constructionRange = new IntRange(0, 20);
            //采矿等级
            this.miningRange = new IntRange(0, 20);
            //烹饪等级
            this.cookingRange = new IntRange(0, 20);
            //种植等级
            this.plantsRange = new IntRange(0, 20);
            //驯兽等级
            this.animalsRange = new IntRange(0, 20);
            //手工等级
            this.craftingRange = new IntRange(0, 20);
            //艺术等级
            this.artisticRange = new IntRange(0, 20);
            //医疗等级
            this.medicalRange = new IntRange(0, 20);
            //射击等级
            this.socialRange = new IntRange(0, 20);
            //智识等级
            this.intellectualRange = new IntRange(0, 20);
            //幼年背景故事
            this.childHoodBackstories.Clear();
            //成年背景故事
            this.adulthoodBackstories.Clear();
            //特性
            this.traits.Clear();
        }
    }
}
