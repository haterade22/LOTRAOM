using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using System.Reflection;
using HarmonyLib;
using LOTRAOM.CultureFeats;
using TaleWorlds.CampaignSystem.Settlements;
using LOTRAOM.CampaignStart;
using TaleWorlds.InputSystem;
using System.Linq;

namespace LOTRAOM
{

    public class LOTRAOMCharacterCreationContent : SandboxCharacterCreationContent
    {
        public override TextObject ReviewPageDescription
        {
            get
            {
                return new("{=lotraom_start}Your journey through the lands of Middle-earth begins! Behold your character, forged for this epic tale. Continue onward if you are ready, or return to make any final adjustments.", null);
            }
        }

        public override IEnumerable<Type> CharacterCreationStages
        {
            get
            {
                yield return typeof(CharacterCreationCultureStage);
                yield return typeof(CharacterCreationFaceGeneratorStage);
                yield return typeof(CharacterCreationGenericStage);
                yield return typeof(CharacterCreationBannerEditorStage);
                yield return typeof(CharacterCreationClanNamingStage);
                yield return typeof(CharacterCreationReviewStage);
                yield return typeof(CharacterCreationOptionsStage);
                yield break;
            }
        }

        protected override void OnCultureSelected()
        {
            base.SelectedTitleType = 1;
            base.SelectedParentType = 0;
            TextObject textObject = FactionHelper.GenerateClanNameforPlayer();
            Clan.PlayerClan.ChangeClanName(textObject, textObject);
            CharacterObject playerCharacter = CharacterObject.PlayerCharacter;
            string cultureId = playerCharacter.Culture.StringId;

            CampaignStartGlobals.CCCultureData playerRaceData = CampaignStartGlobals.CCCulturesRaceData.TryGetValue(playerCharacter.Culture.StringId, out CampaignStartGlobals.CCCultureData? value) ? value : CampaignStartGlobals.CCCulturesRaceData["default"]; ;
            string bodyPropString = playerRaceData.BodyPropertiesString;
            string raceId = playerRaceData.PossibleRaces.First();

            BodyProperties.FromString(bodyPropString, out BodyProperties properties);
            playerCharacter.UpdatePlayerCharacterBodyProperties(properties, playerCharacter.Race, playerCharacter.IsFemale);
            playerCharacter.Race = FaceGen.GetRaceOrDefault(raceId);
        }

        public override int GetSelectedParentType()
        {
            return base.SelectedParentType;
        }

        public override void OnCharacterCreationFinalized()
        {
            base.SetHeroAge((float)_startingAge);

            string settlementId = "town_EW1";
            string playersCulture = Hero.MainHero.Culture.StringId;
            if (CampaignStartGlobals.StartingSettlement.ContainsKey(playersCulture))
                settlementId = CampaignStartGlobals.StartingSettlement[playersCulture];

            Hero.MainHero.PartyBelongedTo.Position2D = Settlement.Find(settlementId).GatePosition;
            if (GameStateManager.Current.ActiveState is MapState mapState)
            {
                mapState.Handler.ResetCamera(true, true);
                mapState.Handler.TeleportCameraToMainParty();
            }
        }

        protected override void OnInitialized(CharacterCreation characterCreation)
        {
            AddParentsMenu(characterCreation);
            AddChildhoodMenu(characterCreation);
            AddEducationMenu(characterCreation);
            AddYouthMenu(characterCreation);
            AddAdulthoodMenu(characterCreation);
            base.AddAgeSelectionMenu(characterCreation);
            //AddCultureStartMenu(characterCreation);

            CultureFeatsCampaignStartText culturalFeats = new();

            foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
            {
                string cultureId = cultureObject.StringId;

                FieldInfo _description = AccessTools.Field(typeof(PropertyObject), "_description");

                _description.SetValue(DefaultCulturalFeats.BattanianMilitiaFeat, new TextObject("Towns owned by rulers with the current culture have +1 militia production."));
                _description.SetValue(DefaultCulturalFeats.KhuzaitAnimalProductionFeat, new TextObject("25% production bonus to horse, mule, cow and sheep in villages owned by all rulers."));


                // this is an example on how to remove a feat text
                //if (cultureObject.CultureFeats.Contains(DefaultCulturalFeats.AseraiDesertFeat))
                //    cultureObject.CultureFeats.Remove(DefaultCulturalFeats.AseraiDesertFeat);

                switch (cultureId) //Will have to investigate if these are actually work
                {
                    case "vlandia": //Rohan
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "sturgia": // Dale
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "battania": //Khand
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "aserai": //Harad
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "empire": //Dunland
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "khuzait": //Rhun
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "gondor":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "mordor":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "rivendell":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "mirkwood":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "lothlorien":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "erebor":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "gundabad":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "dolguldur":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "umbar":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                    case "isengard":
                        cultureObject.CultureFeats.Add(culturalFeats.exampleFeat);
                        break;
                }
            }
        }

        protected new void AddParentsMenu(CharacterCreation characterCreation)
        {
            // FAMILY MENU
            CharacterCreationMenu parentsMenu = new(new("{=b4lDDcli}Family", null), new("{=XgFU1pCx}You were born into a family of...", null), new CharacterCreationOnInit(base.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);


            // FAMILY MENU -> Dunland (EMPIRE)
            CharacterCreationCategory dunlandParentsCategory = parentsMenu.AddMenuCategory(base.EmpireParentsOnCondition);
            dunlandParentsCategory.AddCategoryOption(new("{=lotraom_dunland_1}Descendants of the Old Chieftains"), new() { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.EmpireLandlordsRetainerOnConsequence, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_dunland_desc_1}Your lineage traces back to the old chieftains of Dunland, proud leaders who stood against the encroachment of outsiders. Your father was a respected warrior and a trusted companion of the clan leader, riding at the forefront of battles to protect your people and their traditions."), null, 0, 0, 0, 0, 0);
            dunlandParentsCategory.AddCategoryOption(new("{=lotraom_dunland_2}Clan Merchants"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.EmpireMerchantOnConsequence, base.EmpireMerchantOnApply, new("{=lotraom_dunland_desc_2}Your family traded goods within Dunland and beyond, bartering livestock, furs, and crafted items. They were skilled negotiators, often securing alliances and agreements that benefitted the clan."), null, 0, 0, 0, 0, 0);
            dunlandParentsCategory.AddCategoryOption(new("{=lotraom_dunland_3}Free Clansfolk"), new() { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.EmpireFreeholderOnConsequence, base.EmpireFreeholderOnApply, new("{=lotraom_dunland_desc_3}Your family were free clansfolk, tending to their modest herds and fields in the rugged lands of Dunland. They embodied the independence and resilience of your people, contributing to the strength and survival of the clan."), null, 0, 0, 0, 0, 0);
            dunlandParentsCategory.AddCategoryOption(new("{=lotraom_dunland_4}Skilled Artisans"), new() { DefaultSkills.Crafting, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.EmpireArtisanOnConsequence, base.EmpireArtisanOnApply, new("{=lotraom_dunland_desc_4}Your family were known for their craftsmanship, creating tools, weapons, and intricate carvings valued by the clan. Their work not only supported the community but also served as a source of pride and tradition."), null, 0, 0, 0, 0, 0);
            dunlandParentsCategory.AddCategoryOption(new("{=lotraom_dunland_5}Woodland Foragers"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.EmpireWoodsmanOnConsequence, base.EmpireWoodsmanOnApply, new("{=lotraom_dunland_desc_5}Your family lived on the edge of the forest, relying on hunting, trapping, and foraging. They knew the wilderness intimately, navigating its dangers and resources with skill and caution."), null, 0, 0, 0, 0, 0);
            dunlandParentsCategory.AddCategoryOption(new("{=lotraom_dunland_6}Wanderers and Outcasts"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.EmpireVagabondOnConsequence, base.EmpireVagabondOnApply, new("{=lotraom_dunland_desc_6}Your family lived a transient life on the fringes of Dunland society, scraping by through odd jobs, hunting, or even thievery. They were resourceful survivors, adapting to the harsh realities of life in the shadow of more powerful realms."), null, 0, 0, 0, 0, 0);

            // FAMILY MENU -> Rohan (VLANDIAN)
            CharacterCreationCategory rohanParentsCategory = parentsMenu.AddMenuCategory(base.VlandianParentsOnCondition);
            rohanParentsCategory.AddCategoryOption(new("{=lotraom_rohan_1}Retainers of the Marshal"), new() { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.VlandiaBaronsRetainerOnConsequence, base.VlandiaBaronsRetainerOnApply, new("{=lotraom_rohan_desc_1}Your father was a loyal retainer to one of the Marshals of the Riddermark. He managed the lands of his lord, ensured the readiness of the local levy, and rode at the forefront of battle as a trusted warrior of the Mark."), null, 0, 0, 0, 0, 0);
            rohanParentsCategory.AddCategoryOption(new("{=lotraom_rohan_2}Merchants of Edoras"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.VlandiaMerchantOnConsequence, base.VlandiaMerchantOnApply, new("{=lotraom_rohan_desc_2}Your family traded goods in the bustling markets of Edoras, ensuring that the capital of the Riddermark thrived. They organized caravans, bartered with traders from Gondor and beyond, and were known for their fair dealings."), null, 0, 0, 0, 0, 0);
            rohanParentsCategory.AddCategoryOption(new("{=lotraom_rohan_3}Freeholders of the Westfold"), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.VlandiaYeomanOnConsequence, base.VlandiaYeomanOnApply, new("{=lotraom_rohan_desc_3}Your family tilled the fertile lands of the Westfold, growing crops and raising livestock. As free folk, they were the backbone of Rohan’s strength, providing food and warriors for the kingdom's defense."), null, 0, 0, 0, 0, 0);
            rohanParentsCategory.AddCategoryOption(new("{=lotraom_rohan_4}Smiths of Aldburg"), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.VlandiaBlacksmithOnConsequence, base.VlandiaBlacksmithOnApply, new("{=lotraom_rohan_desc_4}Your family were renowned smiths in Aldburg, crafting weapons and armor for the warriors of the Riddermark. Your father’s skill with the forge was matched only by his dedication to the defense of Rohan."), null, 0, 0, 0, 0, 0);
            rohanParentsCategory.AddCategoryOption(new("{=lotraom_rohan_5}Hunters of the Eastfold"), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.VlandiaHunterOnConsequence, base.VlandiaHunterOnApply, new("{=lotraom_rohan_desc_5}Your family lived on the edge of the Eastfold, relying on their skill with bow and blade to provide for themselves. They were skilled hunters and trackers, navigating the rolling plains and dense woods with ease."), null, 0, 0, 0, 0, 0);
            rohanParentsCategory.AddCategoryOption(new("{=lotraom_rohan_6}Wanderers of the Wold"), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.VlandiaMercenaryOnConsequence, base.VlandiaMercenaryOnApply, new("{=lotraom_rohan_desc_6}Your father was a wanderer, traveling across the Wold and beyond. Known for his cunning and resourcefulness, he often worked as a scout or tracker for passing merchants, and sometimes dealt with less savory elements in the borderlands."), null, 0, 0, 0, 0, 0);

            // FAMILY MENU -> Dale (STURGIAN)
            CharacterCreationCategory daleParentsCategory = parentsMenu.AddMenuCategory(base.SturgianParentsOnCondition);
            daleParentsCategory.AddCategoryOption(new("{=lotraom_dale_1}Servants of the King of Dale"), new() { DefaultSkills.Riding, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.SturgiaBoyarsCompanionOnConsequence, base.SturgiaBoyarsCompanionOnApply, new("{=lotraom_dale_desc_1}Your family served the King of Dale, holding positions of respect and authority in the court. They trained with the king’s guards and rode alongside the royal retinue, ensuring the safety of the realm."), null, 0, 0, 0, 0, 0);
            daleParentsCategory.AddCategoryOption(new("{=lotraom_dale_2}Urban traders"), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.SturgiaTraderOnConsequence, base.SturgiaTraderOnApply, new("{=lotraom_dale_desc_2}Your family were merchants in Dale, organizing trade with Erebor and distant lands. They brought prosperity to the city by mastering the art of negotiation and securing rare goods."), null, 0, 0, 0, 0, 0);
            daleParentsCategory.AddCategoryOption(new("{=lotraom_dale_3}Free Farmers"), new() { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.SturgiaFreemanOnConsequence, base.SturgiaFreemanOnApply, new("{=lotraom_dale_desc_3}Your family were hard-working farmers cultivating the fertile lands near the Running River, providing food for the city and its defenders."), null, 0, 0, 0, 0, 0);
            daleParentsCategory.AddCategoryOption(new("{=lotraom_dale_4}Urban artisans"), new() { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.SturgiaArtisanOnConsequence, base.SturgiaArtisanOnApply, new("{=lotraom_dale_desc_4}Your family were skilled artisans of Dale, crafting weapons, armor, and tools of remarkable quality for the city and its allies."), null, 0, 0, 0, 0, 0);
            daleParentsCategory.AddCategoryOption(new("{=lotraom_dale_5}Forest Hunters"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.SturgiaHunterOnConsequence, base.SturgiaHunterOnApply, new("{=lotraom_dale_desc_5}Your family lived on the forest fringes, hunting deer and gathering herbs. They were adept at tracking and survival, skills passed down through generations."), null, 0, 0, 0, 0, 0);
            daleParentsCategory.AddCategoryOption(new("{=lotraom_dale_6}Vagabonds"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.SturgiaVagabondOnConsequence, base.SturgiaVagabondOnApply, new("{=lotraom_dale_desc_6}Your family lived in the outskirts of Dale, scraping by with odd jobs. Sometimes they fell in with less savory elements, learning the underbelly of city life."), null, 0, 0, 0, 0, 0);

            // FAMILY MENU -> Harad (ASERAI)
            CharacterCreationCategory haradParentsCategory = parentsMenu.AddMenuCategory(base.AseraiParentsOnCondition);
            haradParentsCategory.AddCategoryOption(new("{=lotraom_harad_1}Allies of the Black Serpent"), new() { DefaultSkills.Riding, DefaultSkills.Throwing }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.AseraiTribesmanOnConsequence, base.AseraiTribesmanOnApply, new("{=lotraom_harad_desc_1}Your family were trusted allies of the Black Serpent, sworn to fight for the Haradrim and uphold their ancient oaths to the Dark Lord."), null, 0, 0, 0, 0, 0);
            haradParentsCategory.AddCategoryOption(new("{=lotraom_harad_2}Desert Warriors"), new() { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.AseraiWariorSlaveOnConsequence, base.AseraiWariorSlaveOnApply, new("{=lotraom_harad_desc_2}Your father served as a warrior of Harad, a fierce and loyal soldier skilled in mounted combat and the use of deadly polearms."), null, 0, 0, 0, 0, 0);
            haradParentsCategory.AddCategoryOption(new("{=lotraom_harad_3}Traders of the South"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.AseraiMerchantOnConsequence, base.AseraiMerchantOnApply, new("{=lotraom_harad_desc_3}Your family were traders who carried exotic goods like spices, ivory, and silks across the vast deserts and into distant lands."), null, 0, 0, 0, 0, 0);
            haradParentsCategory.AddCategoryOption(new("{=lotraom_harad_4}Desert Farmers"), new() { DefaultSkills.Athletics, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.AseraiOasisFarmerOnConsequence, base.AseraiOasisFarmerOnApply, new("{=lotraom_harad_desc_4}Your family cultivated crops and date palms in an oasis, ensuring their survival through careful irrigation and unyielding labor."), null, 0, 0, 0, 0, 0);
            haradParentsCategory.AddCategoryOption(new("{=lotraom_harad_5}Nomadic Tribespeople"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.AseraiBedouinOnConsequence, base.AseraiBedouinOnApply, new("{=lotraom_harad_desc_5}Your family roamed the deserts, herding camels and goats, and living in harmony with the unforgiving land."), null, 0, 0, 0, 0, 0);
            haradParentsCategory.AddCategoryOption(new("{=lotraom_harad_6}Desert Outlaws"), new() { DefaultSkills.Roguery, DefaultSkills.Polearm }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.AseraiBackAlleyThugOnConsequence, base.AseraiBackAlleyThugOnApply, new("{=lotraom_harad_desc_6}Your family survived on the fringes of Harad society, taking what they could by force or cunning. They were feared and respected as desert outlaws."), null, 0, 0, 0, 0, 0);

            // FAMILY MENU -> Khand (BATTANIAN)
            CharacterCreationCategory khandParentsCategory = parentsMenu.AddMenuCategory(new(base.BattanianParentsOnCondition));
            khandParentsCategory.AddCategoryOption(new("{=lotraom_khand_1}Horse-Lords of Khand"), new() { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.BattaniaChieftainsHearthguardOnConsequence, base.BattaniaChieftainsHearthguardOnApply, new("{=lotraom_khand_desc_1}Your family were part of the ruling elite of Khand, known for their prowess on horseback and mastery of cavalry warfare."), null, 0, 0, 0, 0, 0);
            khandParentsCategory.AddCategoryOption(new("{=lotraom_khand_2}Nomadic Healers"), new() { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.BattaniaHealerOnConsequence, base.BattaniaHealerOnApply, new("{=lotraom_khand_desc_2}Your parents were healers among the clans of Khand, using herbs and rituals to cure the sick and uphold ancient traditions."), null, 0, 0, 0, 0, 0);
            khandParentsCategory.AddCategoryOption(new("{=lotraom_khand_3}Clan Warriors"), new() { DefaultSkills.Athletics, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.BattaniaTribesmanOnConsequence, base.BattaniaTribesmanOnApply, new("{=lotraom_khand_desc_3}Your family were warriors of Khand, fighting fiercely to protect their land and their clan's honor."), null, 0, 0, 0, 0, 0);
            khandParentsCategory.AddCategoryOption(new("{=lotraom_khand_4}Smiths of the Steppes"), new() { DefaultSkills.Crafting, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.BattaniaSmithOnConsequence, base.BattaniaSmithOnApply, new("{=lotraom_khand_desc_4}Your family were renowned smiths, crafting deadly weapons and ornate armor that symbolized the strength of Khand."), null, 0, 0, 0, 0, 0);
            khandParentsCategory.AddCategoryOption(new("{=lotraom_khand_5}Steppe Hunters"), new() { DefaultSkills.Scouting, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.BattaniaWoodsmanOnConsequence, base.BattaniaWoodsmanOnApply, new("{=lotraom_khand_desc_5}Your family survived on the steppe, hunting game and mastering the art of tracking and ambush."), null, 0, 0, 0, 0, 0);
            khandParentsCategory.AddCategoryOption(new("{=lotraom_khand_6}Bards of Khand"), new() { DefaultSkills.Roguery, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.BattaniaBardOnConsequence, base.BattaniaBardOnApply, new("{=lotraom_khand_desc_6}Your father was a bard, keeping the stories and songs of Khand alive to inspire and unite the clans."), null, 0, 0, 0, 0, 0);


            // FAMILY MENU -> Easterling (KHUZAIT)
            CharacterCreationCategory easterlingParentsCategory = parentsMenu.AddMenuCategory(new(base.KhuzaitParentsOnCondition));
            easterlingParentsCategory.AddCategoryOption(new("{=lotraom_easterling_1}Kinsfolk of the Chieftain"), new() { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.KhuzaitNoyansKinsmanOnConsequence, base.KhuzaitNoyansKinsmanOnApply, new("{=lotraom_easterling_desc_1}Your family were close relatives of the ruling chieftain. Your father was a trusted advisor and warrior, fighting as an armored lancer, while your mother helped manage the affairs of the clan."), null, 0, 0, 0, 0, 0);
            easterlingParentsCategory.AddCategoryOption(new("{=lotraom_easterling_2}Merchants of the East"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.KhuzaitMerchantOnConsequence, base.KhuzaitMerchantOnApply, new("{=lotraom_easterling_desc_2}Your family controlled trade caravans across the eastern steppes, bringing silks, spices, and fine wares from distant lands to the cities of Rhûn. Their dealings earned respect and wealth within the region."), null, 0, 0, 0, 0, 0);
            easterlingParentsCategory.AddCategoryOption(new("{=lotraom_easterling_3}Nomadic Tribesfolk"), new() { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.KhuzaitTribesmanOnConsequence, base.KhuzaitTribesmanOnApply, new("{=lotraom_easterling_desc_3}Your family belonged to a nomadic clan, tending herds of horses and cattle across the vast steppes. They were known for their marksmanship and their skill as mounted warriors."), null, 0, 0, 0, 0, 0);
            easterlingParentsCategory.AddCategoryOption(new("{=lotraom_easterling_4}Steppe Farmers"), new() { DefaultSkills.Polearm, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.KhuzaitFarmerOnConsequence, base.KhuzaitFarmerOnApply, new("{=lotraom_easterling_desc_4}Your family cultivated fertile patches of land on the steppes, growing wheat and barley for the clans. They were also part of the levied forces, wielding spears and shields in defense of their kin."), null, 0, 0, 0, 0, 0);
            easterlingParentsCategory.AddCategoryOption(new("{=lotraom_easterling_5}Spirit Callers"), new() { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.KhuzaitShamanOnConsequence, base.KhuzaitShamanOnApply, new("{=lotraom_easterling_desc_5}Your family were spiritual guides, channeling the will of the spirits through rituals and ancient songs. They healed the wounded and advised the clans on matters of tradition and strategy."), null, 0, 0, 0, 0, 0);
            easterlingParentsCategory.AddCategoryOption(new("{=lotraom_easterling_6}Outland Scouts"), new() { DefaultSkills.Scouting, DefaultSkills.Riding }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, base.KhuzaitNomadOnConsequence, base.KhuzaitNomadOnApply, new("{=lotraom_easterling_desc_6}Your family lived on the fringes of the great steppe, acting as scouts and trackers for the clans. They were experts at navigating the wilderness and evading pursuers in hostile terrain."), null, 0, 0, 0, 0, 0);

            // Gondor
            CharacterCreationCategory gondorParentsCategory = parentsMenu.AddMenuCategory(new(GondorParentsOnCondition));
            gondorParentsCategory.AddCategoryOption(new("{=lotraom_gondor_1}Noble Houses of Gondor"), new() { DefaultSkills.TwoHanded, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_gondor_desc_1}Your family belonged to one of Gondor's noble houses, sworn to defend the White Tree. Your father was a knight who fought bravely in service to the Steward, while your mother managed the family’s lands and affairs."), null, 0, 0, 0, 0, 0);
            gondorParentsCategory.AddCategoryOption(new("{=lotraom_gondor_2}Merchants of the White City"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Merchant); }, base.EmpireMerchantOnApply, new("{=lotraom_gondor_desc_2}Your family were prosperous traders within Minas Tirith, dealing in fine goods and wares. Their influence within the city’s merchant guilds brought wealth and respect to your household."), null, 0, 0, 0, 0, 0);
            gondorParentsCategory.AddCategoryOption(new("{=lotraom_gondor_3}Farmers of the Pelennor"), new() { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_gondor_desc_3}Your family tilled the rich soil of the Pelennor Fields, providing sustenance for Minas Tirith. They also served as levy soldiers, standing in defense of the city during times of war."), null, 0, 0, 0, 0, 0);
            gondorParentsCategory.AddCategoryOption(new("{=lotraom_gondor_4}Artisans of Minas Tirith"), new() { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Artisan); }, base.EmpireArtisanOnApply, new("{=lotraom_gondor_desc_4}Your family were skilled craftsmen in Minas Tirith, creating fine armor, weapons, and tapestries that adorned the halls of the White City. Their artistry was renowned across Gondor."), null, 0, 0, 0, 0, 0);
            gondorParentsCategory.AddCategoryOption(new("{=lotraom_gondor_5}Rangers of Ithilien"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_gondor_desc_5}Your family were members of the Rangers of Ithilien, skilled in the arts of stealth and archery. They patrolled the wilds, protecting Gondor’s borders from the forces of Mordor."), null, 0, 0, 0, 0, 0);
            gondorParentsCategory.AddCategoryOption(new("{=lotraom_gondor_6}Street Urchins of Osgiliath"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_gondor_desc_6}Your family lived among the ruins of Osgiliath, surviving through cunning and resourcefulness. They scavenged what they could and occasionally served under Gondor’s banners as irregular troops."), null, 0, 0, 0, 0, 0);


            // Mordor
            CharacterCreationCategory mordorParentsCategory = parentsMenu.AddMenuCategory(new(MordorParentsOnCondition));
            mordorParentsCategory.AddCategoryOption(new("{=lotraom_mordor_1}Servants of the Dark Tower"), new() { DefaultSkills.Polearm, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_mordor_desc_1}Your family served directly under the command of the Dark Tower. Your father was a brutal enforcer, while your mother worked to instill the will of Sauron among the lesser folk."), null, 0, 0, 0, 0, 0);
            mordorParentsCategory.AddCategoryOption(new("{=lotraom_mordor_2}Merchants of the Black Road"), new() { DefaultSkills.Trade, DefaultSkills.Roguery }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Merchant); }, base.EmpireMerchantOnApply, new("{=lotraom_mordor_desc_2}Your family managed trade and supplies within Mordor, ensuring the armies of Sauron were equipped for war. Their dealings were shadowed by intrigue and violence."), null, 0, 0, 0, 0, 0);
            mordorParentsCategory.AddCategoryOption(new("{=lotraom_mordor_3}Slaves of Barad-dûr"), new() { DefaultSkills.Athletics, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_mordor_desc_3}Your family were enslaved laborers, toiling in the forges and quarries of Mordor to fuel the war machine of the Dark Lord. Their resilience became a testament to their will to survive."), null, 0, 0, 0, 0, 0);
            mordorParentsCategory.AddCategoryOption(new("{=lotraom_mordor_4}Orc Taskmasters"), new() { DefaultSkills.Tactics, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Artisan); }, base.EmpireArtisanOnApply, new("{=lotraom_mordor_desc_4}Your family were tasked with overseeing the labor of Orcs and Men alike, ensuring that Sauron's orders were carried out with ruthless efficiency."), null, 0, 0, 0, 0, 0);
            mordorParentsCategory.AddCategoryOption(new("{=lotraom_mordor_5}Hunters of Gorgoroth"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 5, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_mordor_desc_5}Your family roamed the volcanic plains of Gorgoroth, hunting and tracking prey to sustain the armies of Mordor. Their skill in the wilderness was unmatched."), null, 0, 0, 0, 0, 0);
            mordorParentsCategory.AddCategoryOption(new("{=lotraom_mordor_6}Uruk Overseers"), new() { DefaultSkills.Roguery, DefaultSkills.Polearm }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_mordor_desc_6}Your family served as overseers for the Uruk-hai legions, enforcing discipline and maintaining order within Sauron’s ranks."), null, 0, 0, 0, 0, 0);


            // Rivendell
            CharacterCreationCategory rivendellParentsCategory = parentsMenu.AddMenuCategory(new(RivendellParentsOnCondition));
            rivendellParentsCategory.AddCategoryOption(new("{=lotraom_rivendell_1}Nobles of Imladris"), new() { DefaultSkills.TwoHanded, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_rivendell_desc_1}Your family were esteemed Elves of Imladris, sworn to serve Elrond. They carried the wisdom of ages and the strength to defend Rivendell in its time of need."), null, 0, 0, 0, 0, 0);
            rivendellParentsCategory.AddCategoryOption(new("{=lotraom_rivendell_2}Artisans of Rivendell"), new() { DefaultSkills.Crafting, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Merchant); }, base.EmpireArtisanOnApply, new("{=lotraom_rivendell_desc_2}Your family were skilled craftsmen and loremasters, creating wonders of Elven craftsmanship and preserving the ancient knowledge of Middle-earth."), null, 0, 0, 0, 0, 0);
            rivendellParentsCategory.AddCategoryOption(new("{=lotraom_rivendell_3}Farmers of the Hidden Valley"), new() { DefaultSkills.Athletics, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_rivendell_desc_3}Your family tended the fertile fields of Rivendell, ensuring its people and guests were well-fed and sustained in their peaceful haven."), null, 0, 0, 0, 0, 0);
            rivendellParentsCategory.AddCategoryOption(new("{=lotraom_rivendell_4}Wardens of the Valley"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_rivendell_desc_4}Your family served as scouts and guardians, patrolling the borders of Imladris and ensuring the valley remained a sanctuary free from evil."), null, 0, 0, 0, 0, 0);
            rivendellParentsCategory.AddCategoryOption(new("{=lotraom_rivendell_5}Elven Scholars"), new() { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 5, OccupationTypes.Healer); }, base.BattaniaHealerOnApply, new("{=lotraom_rivendell_desc_5}Your family were scholars of lore, healing, and magic, preserving the ancient knowledge of the Elves and aiding the free peoples of Middle-earth."), null, 0, 0, 0, 0, 0);
            rivendellParentsCategory.AddCategoryOption(new("{=lotraom_rivendell_6}Wanderers of the Wild"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_rivendell_desc_6}Your family were known to venture beyond Rivendell, aiding those in need and learning of the changing world beyond the valley."), null, 0, 0, 0, 0, 0);


            // Mirkwood
            CharacterCreationCategory mirkwoodParentsCategory = parentsMenu.AddMenuCategory(new(MirkwoodParentsOnCondition));
            mirkwoodParentsCategory.AddCategoryOption(new("{=lotraom_mirkwood_1}Descendants of Woodland Nobility"), new() { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_mirkwood_desc_1}Your family descended from the noble houses of the Woodland Realm, serving as close advisors and protectors of King Thranduil’s court."), null, 0, 0, 0, 0, 0);
            mirkwoodParentsCategory.AddCategoryOption(new("{=lotraom_mirkwood_2}Merchants of the Forest Path"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Merchant); }, base.EmpireMerchantOnApply, new("{=lotraom_mirkwood_desc_2}Your family were merchants who traveled the treacherous paths of Mirkwood, bringing goods and trade to the Elvenking’s halls."), null, 0, 0, 0, 0, 0);
            mirkwoodParentsCategory.AddCategoryOption(new("{=lotraom_mirkwood_3}Woodland Farmers"), new() { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_mirkwood_desc_3}Your family lived on the fringes of the Woodland Realm, cultivating the land and gathering what little the shadowed forest provided."), null, 0, 0, 0, 0, 0);
            mirkwoodParentsCategory.AddCategoryOption(new("{=lotraom_mirkwood_4}Artisans of the Woodland Halls"), new() { DefaultSkills.Crafting, DefaultSkills.Bow }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Artisan); }, base.EmpireArtisanOnApply, new("{=lotraom_mirkwood_desc_4}Your family were artisans of the Woodland Realm, crafting exquisite weapons and tools from the resources of the forest."), null, 0, 0, 0, 0, 0);
            mirkwoodParentsCategory.AddCategoryOption(new("{=lotraom_mirkwood_5}Hunters of the Deep Woods"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_mirkwood_desc_5}Your family were skilled hunters who braved the shadowy depths of Mirkwood to provide food and fend off the creatures that lurk within."), null, 0, 0, 0, 0, 0);
            mirkwoodParentsCategory.AddCategoryOption(new("{=lotraom_mirkwood_6}Rangers of the Woodland Realm"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_mirkwood_desc_6}Your family served as rangers, protecting the borders of the Woodland Realm and guarding the forest paths from intruders and threats alike."), null, 0, 0, 0, 0, 0);

            // Lothlorien
            CharacterCreationCategory lothlorienParentsCategory = parentsMenu.AddMenuCategory(new(LothlorienParentsOnCondition));
            lothlorienParentsCategory.AddCategoryOption(new("{=lotraom_lothlorien_1}Nobles of Caras Galadhon"), new() { DefaultSkills.TwoHanded, DefaultSkills.Bow }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_lothlorien_desc_1}Your family served directly under the guidance of Lady Galadriel and Lord Celeborn, defending the heart of Lothlórien with unwavering dedication."), null, 0, 0, 0, 0, 0);
            lothlorienParentsCategory.AddCategoryOption(new("{=lotraom_lothlorien_2}Artisans of the Golden Wood"), new() { DefaultSkills.Crafting, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Merchant); }, base.EmpireArtisanOnApply, new("{=lotraom_lothlorien_desc_2}Your family were master artisans, weaving the beauty of the Golden Wood into their creations, from armor to musical instruments."), null, 0, 0, 0, 0, 0);
            lothlorienParentsCategory.AddCategoryOption(new("{=lotraom_lothlorien_3}Guardians of the Mallorn Trees"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_lothlorien_desc_3}Your family protected the sacred Mallorn trees, ensuring the splendor of Lothlórien endured against any threat."), null, 0, 0, 0, 0, 0);
            lothlorienParentsCategory.AddCategoryOption(new("{=lotraom_lothlorien_4}Rangers of the Dimrill Dale"), new() { DefaultSkills.Bow, DefaultSkills.Scouting }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Hunter); }, base.BattaniaHealerOnApply, new("{=lotraom_lothlorien_desc_4}Your family patrolled the borders of Lothlórien, guarding the passes and defending against Orcs and other intruders."), null, 0, 0, 0, 0, 0);
            lothlorienParentsCategory.AddCategoryOption(new("{=lotraom_lothlorien_5}Healers of the Golden Wood"), new() { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 5, OccupationTypes.Healer); }, base.BattaniaHealerOnApply, new("{=lotraom_lothlorien_desc_5}Your family tended to the wounded and preserved the ancient wisdom of healing within the Golden Wood."), null, 0, 0, 0, 0, 0);
            lothlorienParentsCategory.AddCategoryOption(new("{=lotraom_lothlorien_6}Wanderers of the Galadhrim"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_lothlorien_desc_6}Your family roamed the lands beyond Lothlórien, aiding allies and spreading word of the Golden Wood’s splendor."), null, 0, 0, 0, 0, 0);

            // Erebor
            CharacterCreationCategory ereborParentsCategory = parentsMenu.AddMenuCategory(new(EreborParentsOnCondition));
            ereborParentsCategory.AddCategoryOption(new("{=lotraom_erebor_1}Nobles of the Lonely Mountain"), new() { DefaultSkills.TwoHanded, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_erebor_desc_1}Your family belonged to the noble houses of Erebor, renowned for their skill in battle and crafting unparalleled treasures."), null, 0, 0, 0, 0, 0);
            ereborParentsCategory.AddCategoryOption(new("{=lotraom_erebor_2}Master Smiths"), new() { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Merchant); }, base.EmpireArtisanOnApply, new("{=lotraom_erebor_desc_2}Your family were master smiths of Erebor, crafting the finest weapons and armor in all Middle-earth."), null, 0, 0, 0, 0, 0);
            ereborParentsCategory.AddCategoryOption(new("{=lotraom_erebor_3}Miners of Erebor"), new() { DefaultSkills.Engineering, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_erebor_desc_3}Your family delved deep into the Lonely Mountain, extracting precious metals and gemstones to enrich the kingdom."), null, 0, 0, 0, 0, 0);
            ereborParentsCategory.AddCategoryOption(new("{=lotraom_erebor_4}Warriors of the Iron Hills"), new() { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Mercenary); }, base.EmpireVagabondOnApply, new("{=lotraom_erebor_desc_4}Your family fought alongside the Iron Hills dwarves, renowned for their unbreakable shield walls and mighty axes."), null, 0, 0, 0, 0, 0);
            ereborParentsCategory.AddCategoryOption(new("{=lotraom_erebor_5}Merchants of Dale"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 5, OccupationTypes.Merchant); }, base.EmpireMerchantOnApply, new("{=lotraom_erebor_desc_5}Your family traded goods with the nearby city of Dale, fostering a thriving relationship between the Men of Dale and the Dwarves of Erebor."), null, 0, 0, 0, 0, 0);
            ereborParentsCategory.AddCategoryOption(new("{=lotraom_erebor_6}Wanderers of the Wild"), new() { DefaultSkills.Scouting, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_erebor_desc_6}Your family ventured beyond Erebor, seeking adventure, trade, or treasures lost to time."), null, 0, 0, 0, 0, 0);

            // Gundabad
            CharacterCreationCategory gundabadParentsCategory = parentsMenu.AddMenuCategory(new(GundabadParentsOnCondition));
            gundabadParentsCategory.AddCategoryOption(new("{=lotraom_gundabad_1}Nobles of Gundabad"), new() { DefaultSkills.TwoHanded, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_gundabad_desc_1}Your family were among the noble elite of Gundabad, dedicated to reclaiming their ancestral home and defending it from invaders."), null, 0, 0, 0, 0, 0);
            gundabadParentsCategory.AddCategoryOption(new("{=lotraom_gundabad_2}Smiths of Gundabad"), new() { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Artisan); }, base.EmpireArtisanOnApply, new("{=lotraom_gundabad_desc_2}Your family were skilled smiths, crafting tools and weapons to equip the defenders of Gundabad."), null, 0, 0, 0, 0, 0);
            gundabadParentsCategory.AddCategoryOption(new("{=lotraom_gundabad_3}Miners of Gundabad"), new() { DefaultSkills.Engineering, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_gundabad_desc_3}Your family worked tirelessly in the mines of Gundabad, uncovering its hidden riches."), null, 0, 0, 0, 0, 0);
            gundabadParentsCategory.AddCategoryOption(new("{=lotraom_gundabad_4}Warriors of the Grey Mountains"), new() { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Mercenary); }, base.EmpireVagabondOnApply, new("{=lotraom_gundabad_desc_4}Your family fought against Orcs and other threats in the Grey Mountains, upholding the honor of their kin."), null, 0, 0, 0, 0, 0);
            gundabadParentsCategory.AddCategoryOption(new("{=lotraom_gundabad_5}Rangers of Gundabad"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireWoodsmanOnApply, new("{=lotraom_gundabad_desc_5}Your family served as rangers, patrolling the wilds around Gundabad and safeguarding its borders from harm."), null, 0, 0, 0, 0, 0);

            // Dol Guldur
            CharacterCreationCategory dolguldurParentsCategory = parentsMenu.AddMenuCategory(new(DolguldurParentsOnCondition));
            dolguldurParentsCategory.AddCategoryOption(new("{=lotraom_dolguldur_1}Servants of the Necromancer"), new() { DefaultSkills.TwoHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_dolguldur_desc_1}Your family served directly under the Necromancer, performing his dark bidding and enforcing his will across the region."), null, 0, 0, 0, 0, 0);
            dolguldurParentsCategory.AddCategoryOption(new("{=lotraom_dolguldur_2}Crafters of Shadowed Steel"), new() { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Artisan); }, base.EmpireArtisanOnApply, new("{=lotraom_dolguldur_desc_2}Your family forged weapons and armor imbued with the dark magic of Dol Guldur, equipping its armies for conquest."), null, 0, 0, 0, 0, 0);
            dolguldurParentsCategory.AddCategoryOption(new("{=lotraom_dolguldur_3}Miners of the Shadowed Halls"), new() { DefaultSkills.Engineering, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_dolguldur_desc_3}Your family labored deep within the caverns of Dol Guldur, extracting resources to feed its war machine."), null, 0, 0, 0, 0, 0);
            dolguldurParentsCategory.AddCategoryOption(new("{=lotraom_dolguldur_4}Warriors of the Enchanted Wood"), new() { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Mercenary); }, base.EmpireVagabondOnApply, new("{=lotraom_dolguldur_desc_4}Your family were fierce warriors who fought to extend the shadow of Dol Guldur over the surrounding lands."), null, 0, 0, 0, 0, 0);
            dolguldurParentsCategory.AddCategoryOption(new("{=lotraom_dolguldur_5}Hunters of the Dark Forest"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 5, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_dolguldur_desc_5}Your family hunted the twisted creatures of the forest and tracked those who dared trespass into the Necromancer's domain."), null, 0, 0, 0, 0, 0);
            dolguldurParentsCategory.AddCategoryOption(new("{=lotraom_dolguldur_6}Rogues of Dol Guldur"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_dolguldur_desc_6}Your family were spies and saboteurs, spreading the influence of Dol Guldur through deception and fear."), null, 0, 0, 0, 0, 0);

            // Isengard
            CharacterCreationCategory isengardParentsCategory = parentsMenu.AddMenuCategory(new(IsengardParentsOnCondition));
            isengardParentsCategory.AddCategoryOption(new("{=lotraom_isengard_1}Servants of Saruman"), new() { DefaultSkills.TwoHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_isengard_desc_1}Your family worked under the orders of Saruman the White, managing his domain and assisting in his schemes of conquest."), null, 0, 0, 0, 0, 0);
            isengardParentsCategory.AddCategoryOption(new("{=lotraom_isengard_2}Craftsmen of Orthanc"), new() { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Artisan); }, base.EmpireArtisanOnApply, new("{=lotraom_isengard_desc_2}Your family crafted tools and weapons within the shadow of Orthanc, forging instruments of Saruman's will."), null, 0, 0, 0, 0, 0);
            isengardParentsCategory.AddCategoryOption(new("{=lotraom_isengard_3}Laborers of the Deeping Caves"), new() { DefaultSkills.Engineering, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Farmer); }, base.EmpireFreeholderOnApply, new("{=lotraom_isengard_desc_3}Your family toiled in the deep pits of Isengard, extracting stone and ore to fuel Saruman’s ambitions."), null, 0, 0, 0, 0, 0);
            isengardParentsCategory.AddCategoryOption(new("{=lotraom_isengard_4}Uruk-Hai Warriors"), new() { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Mercenary); }, base.EmpireVagabondOnApply, new("{=lotraom_isengard_desc_4}Your family were among the first Uruk-Hai bred by Saruman, warriors forged for war and domination."), null, 0, 0, 0, 0, 0);
            isengardParentsCategory.AddCategoryOption(new("{=lotraom_isengard_5}Scouts of the Wold"), new() { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 5, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_isengard_desc_5}Your family were scouts and trackers, ensuring Saruman’s forces moved unseen across the plains of Rohan."), null, 0, 0, 0, 0, 0);
            isengardParentsCategory.AddCategoryOption(new("{=lotraom_isengard_6}Rogues of Isengard"), new() { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_isengard_desc_6}Your family operated in the shadows, gathering intelligence and undermining the enemies of Saruman."), null, 0, 0, 0, 0, 0);

            // Umbar
            CharacterCreationCategory umbarParentsCategory = parentsMenu.AddMenuCategory(new(UmbarParentsOnCondition));
            umbarParentsCategory.AddCategoryOption(new("{=lotraom_umbars_1}Corsair Captains"), new() { DefaultSkills.TwoHanded, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 1, OccupationTypes.Retainer); }, base.EmpireLandlordsRetainerOnApply, new("{=lotraom_umbars_desc_1}Your family were leaders among the Corsairs of Umbar, commanding ships and raiding the coasts of Gondor."), null, 0, 0, 0, 0, 0);
            umbarParentsCategory.AddCategoryOption(new("{=lotraom_umbars_2}Merchants of the Black Tide"), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 2, OccupationTypes.Merchant); }, base.EmpireMerchantOnApply, new("{=lotraom_umbars_desc_2}Your family conducted trade across the seas, often smuggling goods to fund the Corsairs’ endeavors."), null, 0, 0, 0, 0, 0);
            umbarParentsCategory.AddCategoryOption(new("{=lotraom_umbars_3}Shipwrights of Umbar"), new() { DefaultSkills.Crafting, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Artisan); }, base.EmpireArtisanOnApply, new("{=lotraom_umbars_desc_3}Your family constructed the vessels that carried the Corsairs to war, crafting ships of speed and stealth."), null, 0, 0, 0, 0, 0);
            umbarParentsCategory.AddCategoryOption(new("{=lotraom_umbars_4}Pirates of the Coasts"), new() { DefaultSkills.OneHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 3, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_umbars_desc_4}Your family were Corsairs who raided the shores of Gondor and beyond, striking fear into the hearts of their enemies."), null, 0, 0, 0, 0, 0);
            umbarParentsCategory.AddCategoryOption(new("{=lotraom_umbars_5}Navigators of the High Seas"), new() { DefaultSkills.Scouting, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 4, OccupationTypes.Hunter); }, base.EmpireWoodsmanOnApply, new("{=lotraom_umbars_desc_5}Your family were skilled navigators, charting the dangerous seas and leading the Corsairs to victory."), null, 0, 0, 0, 0, 0);
            umbarParentsCategory.AddCategoryOption(new("{=lotraom_umbars_6}Smugglers of Umbar"), new() { DefaultSkills.Roguery, DefaultSkills.Trade }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, (characterCreation) => { SetParentAndOccupationType(characterCreation, 6, OccupationTypes.Vagabond); }, base.EmpireVagabondOnApply, new("{=lotraom_umbars_desc_6}Your family moved in the shadows, smuggling goods and information vital to the survival of the Corsairs."), null, 0, 0, 0, 0, 0);

            characterCreation.AddNewMenu(parentsMenu);
        }
        protected new void AddChildhoodMenu(CharacterCreation characterCreation)
        {
            // CHILDHOOD MENU
            CharacterCreationMenu childhoodMenu = new(new("{=8Yiwt1z6}Early Childhood", null), new("{=character_creation_content_16}As a child you were noted for...", null), new CharacterCreationOnInit(base.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            CharacterCreationCategory childhoodCategory = childhoodMenu.AddMenuCategory(null);
            childhoodCategory.AddCategoryOption(new("{=kmM68Qx4}your leadership skills."), new() { DefaultSkills.Leadership, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, SandboxCharacterCreationContent.ChildhoodYourLeadershipSkillsOnConsequence, SandboxCharacterCreationContent.ChildhoodGoodLeadingOnApply, new("{=FfNwXtii}In the shadow of ancient ruins, your youthful band of companions looked to you for guidance. Whether charting a safe path through the woods or rallying courage against imagined foes, your natural leadership shone like the banners of the kings of old."), null, 0, 0, 0, 0, 0);
            childhoodCategory.AddCategoryOption(new("{=5HXS8HEY}your brawn."), new() { DefaultSkills.TwoHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, SandboxCharacterCreationContent.ChildhoodYourBrawnOnConsequence, SandboxCharacterCreationContent.ChildhoodGoodAthleticsOnApply, new("{=YKzuGc54}Your strength was the stuff of stories among the village children. They marveled at your ability to carry burdens like a seasoned warrior and hurl stones with the force of a trebuchet. Even then, you were compared to the heroes of legend."), null, 0, 0, 0, 0, 0);
            childhoodCategory.AddCategoryOption(new("{=QrYjPUEf}your attention to detail."), new() { DefaultSkills.Athletics, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, SandboxCharacterCreationContent.ChildhoodAttentionToDetailOnConsequence, SandboxCharacterCreationContent.ChildhoodGoodMemoryOnApply, new("{=JUSHAPnu}With keen eyes that missed no movement and quick feet that outran most, you were often the first to spot a deer trail or the glint of a hidden trinket. Your friends likened you to the rangers of the north, silent and observant."), null, 0, 0, 0, 0, 0);
            childhoodCategory.AddCategoryOption(new("{=Y3UcaX74}your aptitude for numbers."), new() { DefaultSkills.Engineering, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, SandboxCharacterCreationContent.ChildhoodAptitudeForNumbersOnConsequence, SandboxCharacterCreationContent.ChildhoodGoodMathOnApply, new("{=DFidSjIf}While others played, you lingered in the market, watching merchants haggle over prices and tally profits. You were fascinated by the craft of trade, often imagining yourself as a steward of Gondor or a dwarven craftsman counting gems."), null, 0, 0, 0, 0, 0);
            childhoodCategory.AddCategoryOption(new("{=GEYzLuwb}your way with people."), new() { DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, SandboxCharacterCreationContent.ChildhoodWayWithPeopleOnConsequence, SandboxCharacterCreationContent.ChildhoodGoodMannersOnApply, new("{=w2TEQq26}You had a natural gift for understanding others, often diffusing arguments and winning friends with ease. Your elders said you had the charisma of an elven lord, while your peers followed you with unwavering loyalty."), null, 0, 0, 0, 0, 0);
            childhoodCategory.AddCategoryOption(new("{=MEgLE2kj}your skill with horses."), new() { DefaultSkills.Riding, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, SandboxCharacterCreationContent.ChildhoodSkillsWithHorsesOnConsequence, SandboxCharacterCreationContent.ChildhoodAffinityWithAnimalsOnApply, new("{=ngazFofr}You felt a deep bond with animals, particularly horses. The stable hands marveled at how you could calm even the most spirited steed, and your skill in caring for wounds brought whispers that you might rival the horsemasters of Rohan one day."), null, 0, 0, 0, 0, 0);

            characterCreation.AddNewMenu(childhoodMenu);
        }
        protected new void AddEducationMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new(new("{=rcoueCmk}Adolescence", null), _educationIntroductoryText, new CharacterCreationOnInit(base.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            // EDUCATION MENU -> RURAL
            CharacterCreationCategory educationCategory = characterCreationMenu.AddMenuCategory(null);
            educationCategory.AddCategoryOption(new("{=RKVNvimC}tended the flocks in the shadow of the mountains.", null), new() { DefaultSkills.Athletics, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.RuralAdolescenceOnCondition, base.RuralAdolescenceHerderOnConsequence, SandboxCharacterCreationContent.RuralAdolescenceHerderOnApply, new("{=KfaqPpbK}Amid rolling hills and rocky outcroppings, you kept watch over the flocks, guarding them against wolves and wargs with your keen aim and steady vigilance."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("learned the ancient craft of the forge.", null), new() { DefaultSkills.TwoHanded, DefaultSkills.Crafting }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.BattanianParentsOnCondition, base.RuralAdolescenceSmithyOnConsequence, SandboxCharacterCreationContent.RuralAdolescenceSmithyOnApply, new("{=y6j1bJTH}The forge was a place of glowing embers and the ringing of hammers. Under the guidance of a master smith, you learned to shape steel into tools, weapons, and symbols of your people's pride."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=tI8ZLtoA}rebuilt and restored.", null), new() { DefaultSkills.Crafting, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.RuralAdolescenceOnCondition, base.RuralAdolescenceRepairmanOnConsequence, SandboxCharacterCreationContent.RuralAdolescenceRepairmanOnApply, new("{=6LFj919J}In the aftermath of storms or skirmishes, you worked to mend fences, rebuild homes, and strengthen walls, leaving the village better defended than before."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=TRwgSLD2}searched the woods for healing herbs.", null), new() { DefaultSkills.Medicine, DefaultSkills.Scouting }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.RuralAdolescenceOnCondition, base.RuralAdolescenceGathererOnConsequence, SandboxCharacterCreationContent.RuralAdolescenceGathererOnApply, new("{=9ks4u5cH}You wandered through ancient forests, learning the lore of plants and their medicinal properties. You carried back leaves and roots that could heal wounds and ease fever, saving many lives."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=T7m7ReTq}tracked and hunted small prey.", null), new() { DefaultSkills.Bow, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.RuralAdolescenceOnCondition, base.RuralAdolescenceHunterOnConsequence, SandboxCharacterCreationContent.RuralAdolescenceHunterOnApply, new("{=RuvSk3QT}With a bow in hand, you moved silently through the woods, learning to read the signs of the wild. Your skills as a tracker and hunter became second nature."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=qAbMagWq}sold wares at the bustling market.", null), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.RuralAdolescenceOnCondition, base.RuralAdolescenceHelperOnConsequence, SandboxCharacterCreationContent.RuralAdolescenceHelperOnApply, new("{=DIgsfYfz}Amid the lively chatter and bartering of the marketplace, you haggled with traders from distant lands, learning both their stories and the art of negotiation."), null, 0, 0, 0, 0, 0);

            // EDUCATION MENU -> URBAN
            educationCategory.AddCategoryOption(new("{=nOfSqRnI}trained with the town guard.", null), new() { DefaultSkills.Crossbow, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanAdolescenceOnCondition, base.UrbanAdolescenceWatcherOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceWatcherOnApply, new("{=qnqdEJOv}The clanging of shields and the twang of crossbows filled your days as you learned the discipline required to defend your town from the threats of the wider world."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=8a6dnLd2}found your place among the shadowy guilds.", null), new() { DefaultSkills.Roguery, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanAdolescenceOnCondition, base.UrbanAdolescenceGangerOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceGangerOnApply, new("{=1SUTcF0J}The underbelly of the city became your domain, where alliances were forged in whispers, and survival demanded both cunning and steel."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=7Hv984Sf}toiled at the docks and in the workshops.", null), new() { DefaultSkills.Athletics, DefaultSkills.Crafting }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanAdolescenceOnCondition, base.UrbanAdolescenceDockerOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply, new("{=bhdkegZ4}You learned the value of hard work, whether unloading ships or repairing the stone walls of grand halls. Each day tested both your body and your resolve."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=kbcwb5TH}worked among the merchants and traders.", null), new() { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanPoorAdolescenceOnCondition, base.UrbanAdolescenceMarketerOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceMarketerOnApply, new("{=lLJh7WAT}The call of distant lands and the weight of precious goods taught you the ebb and flow of trade that held the realm together."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=mfRbx5KE}devoured ancient tomes.", null), new() { DefaultSkills.Engineering, DefaultSkills.Leadership }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanPoorAdolescenceOnCondition, base.UrbanAdolescenceTutorOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply, new("{=elQnygal}Through the pages of old lore, you glimpsed the triumphs and tragedies of ages past, gaining insight into the forces that shaped the world."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=etG87fB7}studied under a wise mentor.", null), new() { DefaultSkills.Engineering, DefaultSkills.Leadership }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanRichAdolescenceOnCondition, base.UrbanAdolescenceTutorOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply, new("{=hXl25avg}Under the guidance of a learned tutor, you honed your mind, delving into strategy, history, and the mysteries of the natural world."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=FKpLEamz}cared for the steeds of kings.", null), new() { DefaultSkills.Riding, DefaultSkills.Steward }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanRichAdolescenceOnCondition, base.UrbanAdolescenceHorserOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply, new("{=Ghz90npw}You gained a deep respect for the horses that carried warriors into battle and messengers across the realm, learning to understand their needs and their strength."), null, 0, 0, 0, 0, 0);
            educationCategory.AddCategoryOption(new("{=vH7GtuuK}worked in the stables.", null), new() { DefaultSkills.Riding, DefaultSkills.Steward }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, base.UrbanPoorAdolescenceOnCondition, base.UrbanAdolescenceHorserOnConsequence, SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply, new("{=csUq1RCC}You handled the steeds of travelers and knights, preparing them for long journeys and fierce battles, and learning the bond between rider and mount."), null, 0, 0, 0, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected new void AddYouthMenu(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new(new("{=ok8lSW6M}Youth", null), _youthIntroductoryText, new CharacterCreationOnInit(LOTRAOMYouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            // Harad
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(new(base.AseraiParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=youth_harad_1}trained with the war elephants.", null), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=youth_harad_desc_1}You trained alongside Harad's mighty war elephants, learning to wield the spear while perched atop these towering beasts. Your skill earned you a place among the elite elephant riders, a fearsome force in battle.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=youth_harad_2}guarded the city walls.", null), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=youth_harad_desc_2}You served as a sentinel on the fortified walls of Harad's desert cities, honing your skills with crossbows and maintaining the intricate defenses that protected your people from invaders.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=youth_harad_3}rode with the desert outriders.", null), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=youth_harad_desc_3}You joined Harad's swift outriders, scouting the vast deserts and harassing enemy flanks with your deadly bow and quick reflexes.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=youth_harad_4}trained with the spearmen.", null), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, null, new(base.YouthInfantryOnApply), new("{=youth_harad_desc_4}You trained as a spearman, one of the steadfast warriors of Harad who form the backbone of its armies, defending against cavalry and holding the line in fierce desert battles.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=youth_harad_5}joined the skirmishers.", null), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=youth_harad_desc_5}You fought as a skirmisher, using javelins to harry the enemy from a distance, relying on your speed and agility to evade their counterattacks.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=youth_harad_6}marched with the nomadic clans.", null), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=youth_harad_desc_6}You traveled with the nomadic clans of Harad, learning to survive in the harsh desert environment. Their cunning and resourcefulness became your own.", null), null, 0, 0, 0, 0, 0);

            // Khand
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(base.BattanianParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new("{=lotraom_khand_youth_1}Trained with the Kheshig Guard."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_khand_youth_desc_1}As one of the most promising riders, you were chosen to train with the Kheshig, the elite mounted guard of the Khandish chieftains. With a lance in hand and a steed beneath you, you learned the art of disciplined cavalry combat.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new("{=lotraom_khand_youth_2}Joined the Wall-Watchers."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_khand_youth_desc_2}Stationed at the fortified encampments along the borders, you became a Wall-Watcher. Your duties included mastering the crossbow and maintaining the defenses that kept your people safe from invaders.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new("{=lotraom_khand_youth_3}Rode with the Nomadic Scouts."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_khand_youth_desc_3}You joined the swift-moving Nomadic Scouts, tasked with surveying the vast steppes and deserts of Khand. Armed with a bow, you tracked enemies and delivered critical intelligence back to the warbands.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new("{=lotraom_khand_youth_4}Trained with the Ashkat Warriors."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, null, new(base.YouthInfantryOnApply), new("{=lotraom_khand_youth_desc_4}As a recruit of the Ashkat Warriors, you were equipped with spear and shield, joining the ranks of the Khandish infantry. These hardened fighters form the backbone of the Khandish armies, renowned for their unyielding discipline and strength.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new("{=lotraom_khand_youth_5}Joined the Shadow Riders."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_khand_youth_desc_5}The Shadow Riders are masters of deception and infiltration, often acting as spies and saboteurs. You learned to blend into enemy camps and settlements, using trickery, disguise, and cunning to gather intelligence and sow chaos.", null), null, 0, 0, 0, 0, 0);

            // Dunland
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(base.EmpireParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dunland_youth_1}Rode with the Hill Riders."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_dunland_youth_desc_1}Mounted on sturdy hill ponies, you joined the Hill Riders, a rare but fierce cavalry force among the Dunlendings. Armed with spear and shield, you learned to navigate the rugged terrain with precision and strength.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dunland_youth_2}Served in the Tribal Longhouse."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_dunland_youth_desc_2}Within the tribal longhouses, you helped guard your people’s stockpiles and crafted defenses for the village. Your skill with crossbows and constructing fortifications became invaluable during raids and sieges.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dunland_youth_3}Stood guard with the Border Watch."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_dunland_youth_desc_3}You joined the Border Watch, a group of young warriors tasked with defending Dunland’s borders. Armed with javelins and axes, you learned to strike swiftly and retreat before the enemy could retaliate.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dunland_youth_4}Scouted the highlands."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_dunland_youth_desc_4}You roamed the highlands and forests as a scout, tracking enemy movements and learning to survive off the land. Your bow became an extension of your arm, your skill saving your people from surprise attacks.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dunland_youth_5}Joined the Spear Brothers."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, null, new(base.YouthInfantryOnApply), new("{=lotraom_dunland_youth_desc_5}You became one of the Spear Brothers, a close-knit band of fighters armed with spears and shields. Drawn from the ranks of farmers and herdsmen, they form the heart of Dunland’s infantry, known for their ferocity and endurance.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dunland_youth_6}Trained as a Tribal Skirmisher."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_dunland_youth_desc_6}As a Tribal Skirmisher, you learned the art of hit-and-run tactics, striking quickly with thrown weapons and melting into the terrain. Your cunning and agility were critical in harassing enemy forces.", null), null, 0, 0, 0, 0, 0);

            // Rohan
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(base.VlandianParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rohan_youth_1}Trained with the Riders of the Mark."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_rohan_youth_desc_1}You proved your skill as a rider, earning the trust of a local marshal who lent you a horse and spear. You trained with the Riders of the Mark, learning to charge into battle with precision and discipline.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rohan_youth_2}Served in the city watch of Edoras."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_rohan_youth_desc_2}You stood guard over the gates and walls of Edoras, keeping a watchful eye on the horizon for approaching enemies. You honed your skills with missile weapons and learned the art of fortification.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rohan_youth_3}Joined the skirmishers of the Westfold."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_rohan_youth_desc_3}You joined the Westfold skirmishers, a band of agile warriors skilled in harassing the enemy with javelins and close-quarters combat. You learned to stay nimble and strike swiftly.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rohan_youth_4}Rode with the scouts of the Wold."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_rohan_youth_desc_4}You rode with the scouts of the Wold, tracking enemy movements and mapping the terrain. Your skill in archery and swift riding made you an invaluable asset to the Rohirrim.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rohan_youth_5}Stood among the spearmen of the Eastfold."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_rohan_youth_desc_5}As a spearman of the Eastfold, you fought alongside farmers and villagers, armed with spear and shield. You stood resolute in the shield wall, defending your people against the foes of Rohan.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rohan_youth_6}Traveled with the camp followers of Rohan."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_rohan_youth_desc_6}Rather than taking up arms directly, you followed the armies of Rohan as a camp follower. You assisted in tending to the wounded, cooking meals, and sometimes dealing with less savory tasks among the soldiers.", null), null, 0, 0, 0, 0, 0);

            // Rhun
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(base.KhuzaitParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rhun_youth_1}Rode with the Khelek Warriors."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_rhun_youth_desc_1}You proved your worth as a rider, and a chieftain entrusted you with a sturdy horse and a finely wrought spear. You trained with the Khelek Warriors, the elite cavalry of Rhûn, mastering the art of the charge.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rhun_youth_2}Guarded the settlements of Rhûn."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_rhun_youth_desc_2}You patrolled the walls of Rhûn's settlements, vigilant against invaders and raiders. You honed your skills with ranged weapons, preparing for the sieges that threatened your people.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rhun_youth_3}Joined the ranks of the Urûk-thar."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_rhun_youth_desc_3}As a member of the Urûk-thar, you fought on the frontlines with agility and precision. Armed with javelins and a short blade, you became adept at harassing enemy formations and retreating to safety.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rhun_youth_4}Scouted the steppes."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_rhun_youth_desc_4}You joined the scouts of Rhûn, riding across the vast steppes to gather intelligence and track enemy movements. Your keen eye and swift bow made you a valuable asset.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rhun_youth_5}Fought with the spear line."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_rhun_youth_desc_5}Standing shoulder to shoulder with your kin, you fought in the spear line, the backbone of Rhûn’s armies. With your spear and shield, you defended your homeland against all who dared threaten it.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rhun_youth_6}Traveled with the Karash bands."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_rhun_youth_desc_6}You avoided direct service in the armies of Rhûn, instead joining the Karash bands—nomadic groups of traders, entertainers, and survivalists who supported the war effort through unconventional means.", null), null, 0, 0, 0, 0, 0);

            // Dale
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(base.SturgianParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dale_youth_1}Served in the Royal Guard of Dale."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_dale_youth_desc_1}You pledged your loyalty to the King of Dale, serving as a guard in his elite mounted retinue. Equipped with a fine steed and a sturdy lance, you trained to protect the royal court.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dale_youth_2}Defended the gates of Dale."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_dale_youth_desc_2}As a member of the garrison, you stood watch over the gates and walls of Dale. You trained with crossbows and siege engines to prepare for attacks on the city.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dale_youth_3}Trained with the infantry of Dale."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_dale_youth_desc_3}You joined the ranks of Dale’s infantry, where you learned to fight with throwing axes and short blades. Quick and light-footed, you harassed enemies from a distance and supported the main lines.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dale_youth_4}Rode with the trackers."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_dale_youth_desc_4}As part of the trackers, you scouted the surrounding lands, warning of approaching threats. Your keen aim with a bow and your skill on horseback proved invaluable.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dale_youth_5}Joined the shieldwall."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_dale_youth_desc_5}You took your place in Dale’s shieldwall, the backbone of the city’s defenses. Armed with a spear and shield, you stood firm against the enemy’s advance.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dale_youth_6}Traveled with the merchant caravans."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_dale_youth_desc_6}You avoided direct military service, choosing instead to accompany merchant caravans as they crossed the Wilderland. You learned to defend yourself and negotiate with traders, rogues, and bandits alike.", null), null, 0, 0, 0, 0, 0);

            // Gondor
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(GondorParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new("{=lotraom_gondor_youth_1}Trained with the Swan Knights."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_gondor_youth_desc_1}You joined the ranks of the Swan Knights of Dol Amroth, renowned for their valor and skill in mounted combat. Training with a lance and shield, you prepared to defend Gondor’s borders.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gondor_youth_2}Stood guard in Minas Tirith."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_gondor_youth_desc_2}You served in the garrison of Minas Tirith, guarding the White City’s walls. Your training focused on defending against sieges, mastering missile weapons and siege equipment.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gondor_youth_3}Rode with the Rangers of Ithilien."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_gondor_youth_desc_3}You patrolled the wilds of Ithilien as a Ranger, scouting enemy movements and striking swiftly from the shadows with bow and sword.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gondor_youth_4}Trained with the Tower Guard."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, null, new(base.YouthInfantryOnApply), new("{=lotraom_gondor_youth_desc_4}You joined the elite Tower Guard of Gondor, standing as a defender of the Steward and the White Tower. Armed with spear and shield, you stood firm against all foes.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gondor_youth_5}Marched with the camp followers."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_gondor_youth_desc_5}You traveled with Gondor’s armies, aiding in their needs as a camp follower. From foraging to scavenging, you learned the skills of survival and subterfuge in times of war.", null), null, 0, 0, 0, 0, 0);

            // Mordor
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(MordorParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new("{=lotraom_mordor_youth_1}Trained with the Warg Riders."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_mordor_youth_desc_1}You were chosen to train with the elite Warg Riders, learning to master the ferocious beasts and wield a spear in devastating charges against Mordor's enemies.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mordor_youth_2}Patrolled the dark fortresses."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_mordor_youth_desc_2}You served as a watchman on the towers and walls of Mordor’s fortresses, honing your skills with crossbows and preparing siege engines for inevitable war.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mordor_youth_3}Rode with the Black Scouts."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_mordor_youth_desc_3}You joined the Black Scouts, riding across the borders of Mordor to track enemy movements and relay crucial intelligence to the Dark Lord's armies.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mordor_youth_4}Trained with the Orc Infantry."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, null, new(base.YouthInfantryOnApply), new("{=lotraom_mordor_youth_desc_4}You marched among the Orc infantry, wielding crude but effective weapons and forming the relentless backbone of Mordor’s war machine.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mordor_youth_5}Joined the Shadow Skirmishers."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_mordor_youth_desc_5}You became a Shadow Skirmisher, throwing jagged projectiles and darting in and out of battle to harry the enemy’s flanks.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mordor_youth_6}Marched with the war host."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_mordor_youth_desc_6}You avoided the front lines, instead joining the war host’s camp followers who supplied, scavenged, and supported the Dark Lord's forces in unconventional ways.", null), null, 0, 0, 0, 0, 0);

            // Rivendell
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(RivendellParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new("{=lotraom_rivendell_youth_1}Trained with the Guard of Imladris."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_rivendell_youth_desc_1}You served under the banners of Rivendell, riding as part of its elite cavalry. You were entrusted with the protection of the realm’s borders, wielding a long spear with skill and precision.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rivendell_youth_2}Stood watch at the halls of Elrond."), new()
            {
                DefaultSkills.Bow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_rivendell_youth_desc_2}You served as a sentry, guarding Rivendell’s sacred halls. Your training focused on long-range accuracy with a bow and defensive preparations for potential sieges.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rivendell_youth_3}Joined the scouting parties."), new()
            {
                DefaultSkills.Scouting,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_rivendell_youth_desc_3}You became part of the scouting parties, tasked with traversing the wilds and watching for the stirrings of darkness. You honed your agility and sharp senses during these missions.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rivendell_youth_4}Trained in the art of Elven infantry."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_rivendell_youth_desc_4}You stood with the ranks of Rivendell’s warriors, learning to master the spear and sword. The disciplined formation of your comrades inspired your determination.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_rivendell_youth_5}Journeyed with the loremasters."), new()
            {
                DefaultSkills.Charm,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_rivendell_youth_desc_5}You accompanied Rivendell’s loremasters as they traveled across Middle-earth, spreading wisdom and forging alliances. You learned the subtleties of diplomacy and persuasion.", null), null, 0, 0, 0, 0, 0);

            // Mirkwood
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(MirkwoodParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new("{=lotraom_mirkwood_youth_1}Trained with the Woodland Riders."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_mirkwood_youth_desc_1}You rode through the dark woods, honing your skill as an archer on horseback. You were the swift protectors of the Woodland Realm.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mirkwood_youth_2}Patrolled the darkened glades."), new()
            {
                DefaultSkills.Scouting,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_mirkwood_youth_desc_2}You joined the wardens who patrolled the glades of Mirkwood, keeping watch for spiders and darker shadows. Your agility and vigilance served you well.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mirkwood_youth_3}Stood with the Elven Spear Guard."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_mirkwood_youth_desc_3}You took your place among the Spear Guard, protecting the Woodland King’s halls from threats both seen and unseen.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mirkwood_youth_4}Joined the shadow trackers."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_mirkwood_youth_desc_4}You were among those tasked with tracking the movements of orcs and other foul creatures in the shadows of Mirkwood. Your cunning and adaptability ensured your survival.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_mirkwood_youth_5}Marched with the royal banners."), new()
            {
                DefaultSkills.Charm,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_mirkwood_youth_desc_5}You joined the royal court as part of the Woodland King’s entourage, learning diplomacy and leadership among the Elven nobility.", null), null, 0, 0, 0, 0, 0);

            // Lothlorien
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(LothlorienParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new("{=lotraom_lothlorien_youth_1}Trained with the Galadhrim cavalry."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_lothlorien_youth_desc_1}You joined the elite cavalry of Lothlórien, protecting the Golden Wood. You became adept with the lance, defending the borders against intrusions.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_lothlorien_youth_2}Guarded the golden borders."), new()
            {
                DefaultSkills.Bow,
                DefaultSkills.Scouting
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_lothlorien_youth_desc_2}You patrolled the borders of the Golden Wood, ensuring that no unwelcome foot entered its sacred grounds. Your skill with a bow was critical in defending against dark forces.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_lothlorien_youth_3}Joined the archers of Caras Galadhon."), new()
            {
                DefaultSkills.Bow,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_lothlorien_youth_desc_3}You trained with the famed archers of Lothlórien, learning how to strike swiftly and retreat to cover, leaving the enemy in disarray.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_lothlorien_youth_4}Joined the wardens of the trees."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_lothlorien_youth_desc_4}You became a warden of the trees, standing guard beneath the towering mallorn trees. Your strength and steadfastness ensured the safety of Lórien’s heart.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_lothlorien_youth_5}Marched with the singers of tales."), new()
            {
                DefaultSkills.Charm,
                DefaultSkills.Roguery
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_lothlorien_youth_desc_5}You traveled with the singers of tales, sharing the stories of the Eldar and spreading hope. You learned the art of persuasion and subtlety during your journeys.", null), null, 0, 0, 0, 0, 0);

            // Erebor
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(EreborParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_erebor_youth_1}Served in the Royal Guard of Erebor."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_erebor_youth_desc_1}You pledged your loyalty to the King Under the Mountain, joining his elite guard. Though Dwarves rarely ride, you trained to defend the halls of Erebor with unwavering precision and discipline.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_erebor_youth_2}Defended the gates of Erebor."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_erebor_youth_desc_2}As part of Erebor’s garrison, you honed your skills with crossbows and stone-throwers. You stood ready to repel any intruder threatening the gates of the Lonely Mountain.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_erebor_youth_3}Trained with the Iron Guard."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_erebor_youth_desc_3}You joined the Iron Guard, a unit known for their agility and precision. Armed with throwing axes and short blades, you harried foes while protecting the deeper halls.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_erebor_youth_4}Scouted the foothills of the Lonely Mountain."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_erebor_youth_desc_4}You ventured into the wild lands surrounding Erebor, tracking movements of orcs, trolls, and other threats. Your skill with a bow and knowledge of the terrain made you invaluable.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_erebor_youth_5}Stood in the vanguard."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.TwoHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_erebor_youth_desc_5}You trained as part of Erebor’s vanguard, wielding massive warhammers and halberds. Standing firm at the front lines, you were a bulwark against any assault.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_erebor_youth_6}Worked with the caravan guards."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Trade
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_erebor_youth_desc_6}You accompanied trading caravans laden with Erebor’s treasures, ensuring their safe passage through the Wilderland. You learned the ways of merchants, as well as how to fend off thieves and raiders.", null), null, 0, 0, 0, 0, 0);

            // Isengard
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(IsengardParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_isengard_youth_1}Joined Saruman's cavalry."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_isengard_youth_desc_1}You trained as a rider for Saruman's forces, patrolling the lands surrounding Isengard and engaging in skirmishes with Rohan.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_isengard_youth_2}Guarded the pits of Orthanc."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_isengard_youth_desc_2}You were stationed at Orthanc, guarding the forges and machinery that fueled Saruman’s war effort. Your skill with ranged weapons and siege equipment was honed in preparation for battle.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_isengard_youth_3}Fought among the Uruk-hai infantry."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_isengard_youth_desc_3}You trained with the ferocious Uruk-hai, learning to fight with brutal efficiency. Your skill with thrown weapons and close combat earned you respect among your kin.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_isengard_youth_4}Scouted for Saruman's spies."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Scouting
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_isengard_youth_desc_4}You were sent as a scout, gathering intelligence for Saruman. Your ability to track movements and remain unseen made you an invaluable asset.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_isengard_youth_5}Joined the ranks of Isengard's pikes."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_isengard_youth_desc_5}You became a member of Isengard’s formidable pikemen, known for their discipline and ability to hold the line against cavalry charges.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_isengard_youth_6}Marched with Saruman's warbands."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_isengard_youth_desc_6}You traveled with Isengard’s warbands, raiding villages and sowing fear among the Free Peoples. You learned to survive on plunder and cunning.", null), null, 0, 0, 0, 0, 0);

            // Gundabad
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(GundabadParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gundabad_youth_1}Joined the Warg riders."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_gundabad_youth_desc_1}You rode with the Warg riders of Gundabad, striking terror into the hearts of your enemies with swift and deadly raids.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gundabad_youth_2}Guarded the forges of Gundabad."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_gundabad_youth_desc_2}You served in the garrison of Gundabad, defending the dark halls and the forges that armed its legions.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gundabad_youth_3}Trained with the berserkers."), new()
            {
                DefaultSkills.TwoHanded,
                DefaultSkills.Athletics
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_gundabad_youth_desc_3}You were among the berserkers of Gundabad, wielding massive weapons with unrelenting ferocity to crush your foes.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gundabad_youth_4}Scouted the mountain passes."), new()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_gundabad_youth_desc_4}You roamed the treacherous mountain paths of Gundabad, watching for intruders and ambushing unwary travelers.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gundabad_youth_5}Joined the spear legions."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_gundabad_youth_desc_5}You marched with the disciplined spear legions of Gundabad, ready to pierce through any defense.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_gundabad_youth_6}Marched with the warbands."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_gundabad_youth_desc_6}You followed the warbands of Gundabad, pillaging and striking fear into the lands of men and elves.", null), null, 0, 0, 0, 0, 0);

            // Dol Guldur
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(DolguldurParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dolguldur_youth_1}Served the Necromancer's riders."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_dolguldur_youth_desc_1}You rode in the service of Dol Guldur, spreading terror across the lands of Rhovanion with swift and brutal attacks.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dolguldur_youth_2}Guarded the halls of shadow."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_dolguldur_youth_desc_2}You defended the dark halls of Dol Guldur, ensuring the stronghold's defenses remained impenetrable.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dolguldur_youth_3}Trained in the dark arts of war."), new()
            {
                DefaultSkills.OneHanded,
                DefaultSkills.Tactics
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_dolguldur_youth_desc_3}You honed your skills in the dark arts of warfare, learning to fight with cunning and ruthlessness.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dolguldur_youth_4}Scouted the borders of Mirkwood."), new()
            {
                DefaultSkills.Scouting,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_dolguldur_youth_desc_4}You patrolled the edges of Mirkwood, watching for threats and preying on those who wandered too close.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dolguldur_youth_5}Joined the shadow legions."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.TwoHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_dolguldur_youth_desc_5}You marched under the banners of shadow, wielding dark weapons to crush your enemies.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_dolguldur_youth_6}Lurked in the shadows."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_dolguldur_youth_desc_6}You avoided open combat, working instead in secret to undermine the forces of light with cunning and deception.", null), null, 0, 0, 0, 0, 0);

            // Umbar
            characterCreationCategory = characterCreationMenu.AddMenuCategory(new(UmbarParentsOnCondition));

            characterCreationCategory.AddCategoryOption(new("{=lotraom_umber_youth_1}Served in the Corsair Fleet."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Polearm
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCavalryOnConsequence), new(base.YouthCavalryOnApply), new("{=lotraom_umber_youth_desc_1}You joined the Corsair Fleet of Umbar, a feared naval force. Training aboard swift ships, you mastered naval combat and the use of polearms for boarding enemy vessels.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_umber_youth_2}Guarded the harbors of Umbar."), new()
            {
                DefaultSkills.Crossbow,
                DefaultSkills.Engineering
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthGarrisonOnConsequence), new(base.YouthGarrisonOnApply), new("{=lotraom_umber_youth_desc_2}You served in the harbor garrisons, defending the bustling ports of Umbar. With training in crossbows and fortifications, you ensured the safety of its lucrative trade routes.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_umber_youth_3}Fought alongside the raiders."), new()
            {
                DefaultSkills.Throwing,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthSkirmisherOnConsequence), new(base.YouthSkirmisherOnApply), new("{=lotraom_umber_youth_desc_3}You joined the raiders of Umbar, specializing in swift and deadly strikes on coastal settlements. Armed with throwing weapons and short blades, you excelled in ambush tactics.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_umber_youth_4}Scouted the open seas."), new()
            {
                DefaultSkills.Riding,
                DefaultSkills.Bow
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthOtherOutridersOnConsequence), new(base.YouthOtherOutridersOnApply), new("{=lotraom_umber_youth_desc_4}You sailed with the Corsairs as a scout, navigating treacherous waters and spotting enemy ships. Your skill with a bow was invaluable for ranged attacks during naval skirmishes.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_umber_youth_5}Joined the spearline."), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.OneHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthInfantryOnConsequence), new(base.YouthInfantryOnApply), new("{=lotraom_umber_youth_desc_5}You took up the spear and shield in Umbar’s infantry, the backbone of its coastal defenses. Standing firm against attackers, you proved your worth in the heat of battle.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=lotraom_umber_youth_6}Sailed with the outlaws."), new()
            {
                DefaultSkills.Roguery,
                DefaultSkills.Throwing
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.YouthCamperOnConsequence), new(base.YouthCamperOnApply), new("{=lotraom_umber_youth_desc_6}You chose a life with the outlaws of Umbar, crewing ships that plundered the coasts and harried merchant vessels. You learned to survive by wit and cunning, thriving in a life of intrigue and danger.", null), null, 0, 0, 0, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }


        private void LOTRAOMYouthOnInit(CharacterCreation characterCreation)
        {
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            TextObject textObject = new TextObject("{=lotraom_youth_init_male}As a youngster growing up in Arda, war was never too far away. You...", null);
            TextObject textObject2 = new TextObject("{=lotraom_youth_init_female}In wartorn Arda, some women as well as men learn to fight from an early age. You...", null);
            _youthIntroductoryText.SetTextVariable("YOUTH_INTRO", CharacterObject.PlayerCharacter.IsFemale ? textObject2 : textObject);
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge((float)YouthAge, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_schooled"
            });
            if (base.SelectedTitleType < 1 || base.SelectedTitleType > 10)
            {
                base.SelectedTitleType = 1;
            }
            RefreshPlayerAppearance(characterCreation);

        }

        protected new void AddAdulthoodMenu(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", SkillLevelToAdd);
            CharacterCreationMenu characterCreationMenu = new(new("{=MafIe9yI}Young Adulthood", null), new("{=4WYY0X59}Before you set out for a life of adventure, your biggest achievement was...", null), new CharacterCreationOnInit(base.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new("{=8bwpVpgy}you defeated an enemy in battle.", null), new()
            {
                DefaultSkills.OneHanded,
                DefaultSkills.TwoHanded
            }, DefaultCharacterAttributes.Vigor, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.AccomplishmentDefeatedEnemyOnConsequence), new(base.AccomplishmentDefeatedEnemyOnApply), new("{=1IEroJKs}Not everyone who musters for the levy marches to war, and not everyone who goes on campaign sees action. You did both, and you also took down an enemy warrior in direct one-to-one combat, in the full view of your comrades.", null), new()
            {
                DefaultTraits.Valor
            }, 1, 20, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=mP3uFbcq}you led a successful manhunt.", null), new()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentPosseOnConditions), new(base.AccomplishmentExpeditionOnConsequence), new(base.AccomplishmentExpeditionOnApply), new("{=4f5xwzX0}When your community needed to organize a posse to pursue horse thieves, you were the obvious choice. You hunted down the raiders, surrounded them and forced their surrender, and took back your stolen property.", null), new()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=wfbtS71d}you led a caravan.", null), new()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentMerchantOnCondition), new(base.AccomplishmentMerchantOnConsequence), new(base.AccomplishmentExpeditionOnApply), new("{=joRHKCkm}Your family needed someone trustworthy to take a caravan to a neighboring town. You organized supplies, ensured a constant watch to keep away bandits, and brought it safely to its destination.", null), new()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=x1HTX5hq}you saved your village from a flood.", null), new()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentSavedVillageOnCondition), new(base.AccomplishmentSavedVillageOnConsequence), new(base.AccomplishmentExpeditionOnApply), new("{=bWlmGDf3}When a sudden storm caused the local stream to rise suddenly, your neighbors needed quick-thinking leadership. You provided it, directing them to build levees to save their homes.", null), new()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=s8PNllPN}you saved your city quarter from a fire.", null), new()
            {
                DefaultSkills.Tactics,
                DefaultSkills.Leadership
            }, DefaultCharacterAttributes.Cunning, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentSavedStreetOnCondition), new(base.AccomplishmentSavedStreetOnConsequence), new(base.AccomplishmentExpeditionOnApply), new("{=ZAGR6PYc}When a sudden blaze broke out in a back alley, your neighbors needed quick-thinking leadership and you provided it. You organized a bucket line to the nearest well, putting the fire out before any homes were lost.", null), new()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=xORjDTal}you invested some money in a workshop.", null), new()
            {
                DefaultSkills.Trade,
                DefaultSkills.Crafting
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentUrbanOnCondition), new(base.AccomplishmentWorkshopOnConsequence), new(base.AccomplishmentWorkshopOnApply), new("{=PyVqDLBu}Your parents didn't give you much money, but they did leave just enough for you to secure a loan against a larger amount to build a small workshop. You paid back what you borrowed, and sold your enterprise for a profit.", null), new()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=xKXcqRJI}you invested some money in land.", null), new()
            {
                DefaultSkills.Trade,
                DefaultSkills.Crafting
            }, DefaultCharacterAttributes.Intelligence, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentRuralOnCondition), new(base.AccomplishmentWorkshopOnConsequence), new(base.AccomplishmentWorkshopOnApply), new("{=cbF9jdQo}Your parents didn't give you much money, but they did leave just enough for you to purchase a plot of unused land at the edge of the village. You cleared away rocks and dug an irrigation ditch, raised a few seasons of crops, than sold it for a considerable profit.", null), new()
            {
                DefaultTraits.Calculating
            }, 1, 10, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=TbNRtUjb}you hunted a dangerous animal.", null), new()
            {
                DefaultSkills.Polearm,
                DefaultSkills.Crossbow
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentRuralOnCondition), new(base.AccomplishmentSiegeHunterOnConsequence), new(base.AccomplishmentSiegeHunterOnApply), new("{=I3PcdaaL}Wolves, bears are a constant menace to the flocks of northern Athas, while hyenas and leopards trouble the south. You went with a group of your fellow villagers and fired the missile that brought down the beast.", null), null, 0, 5, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=WbHfGCbd}you survived a siege.", null), new()
            {
                DefaultSkills.Bow,
                DefaultSkills.Crossbow
            }, DefaultCharacterAttributes.Control, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentUrbanOnCondition), new(base.AccomplishmentSiegeHunterOnConsequence), new(base.AccomplishmentSiegeHunterOnApply), new("{=FhZPjhli}Your hometown was briefly placed under siege, and you were called to defend the walls. Everyone did their part to repulse the enemy assault, and everyone is justly proud of what they endured.", null), null, 0, 5, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=kNXet6Um}you had a famous escapade in town.", null), new()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Roguery
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentRuralOnCondition), new(base.AccomplishmentEscapadeOnConsequence), new(base.AccomplishmentEscapadeOnApply), new("{=DjeAJtix}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, on one of your trips into town you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new()
            {
                DefaultTraits.Valor
            }, 1, 5, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=qlOuiKXj}you had a famous escapade.", null), new()
            {
                DefaultSkills.Athletics,
                DefaultSkills.Roguery
            }, DefaultCharacterAttributes.Endurance, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, new(base.AccomplishmentUrbanOnCondition), new(base.AccomplishmentEscapadeOnConsequence), new(base.AccomplishmentEscapadeOnApply), new("{=lD5Ob3R4}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new()
            {
                DefaultTraits.Valor
            }, 1, 5, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new("{=Yqm0Dics}you treated people well.", null), new()
            {
                DefaultSkills.Charm,
                DefaultSkills.Steward
            }, DefaultCharacterAttributes.Social, FocusToAdd, SkillLevelToAdd, AttributeLevelToAdd, null, new(base.AccomplishmentTreaterOnConsequence), new(base.AccomplishmentTreaterOnApply), new("{=dDmcqTzb}Yours wasn't the kind of reputation that local legends are made of, but it was the kind that wins you respect among those around you. You were consistently fair and honest in your business dealings and helpful to those in trouble. In doing so, you got a sense of what made people tick.", null), new()
            {
                DefaultTraits.Mercy,
                DefaultTraits.Generosity,
                DefaultTraits.Honor
            }, 1, 5, 0, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }
        //public void AddCultureStartMenu(CharacterCreation characterCreation)
        //{

        //    CharacterCreationMenu characterCreationMenu = new(new("{=CulturedStart07}Start Options", null), new("{=CulturedStart08}Who are you in Arda...", null), new CharacterCreationOnInit(StartOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
        //    CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart09}A commoner (Default Start)", null), new(), null, 0, 0, 0, null, new(DefaultStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart10}Setting off with your Father, Mother, Brother and your two younger siblings to a new town you'd heard was safer. But you did not make it.", null), null, 0, 0, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart11}A budding caravaneer", null), new MBList<SkillObject>
        //    {
        //        DefaultSkills.Trade,
        //        DefaultSkills.Charm
        //    }, null, 1, 25, 0, null, new(MerchantStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart12}With what savings you could muster you purchased some mules and mercenaries." + $"\n{startingSkillMult[StartType.Merchant]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 0, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart13}A ranger of {CULTURE} in exile", null), new MBList<SkillObject>
        //    {
        //        DefaultSkills.Leadership,
        //        DefaultSkills.Scouting
        //    }, null, 1, 50, 0, null, new(ExiledStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart14}Forced into exile after your parents were executed for suspected treason. With only your family's bodyguard you set off. Should you return you'd be viewed as a criminal." + $"\n{startingSkillMult[StartType.Exiled]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 150, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart15}A leader of a failing mercenary company", null), new MBList<SkillObject>
        //    {
        //        DefaultSkills.Tactics,
        //        DefaultSkills.Roguery
        //    }, null, 1, 50, 0, null, new(MercenaryStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart16}With men deserting over lack of wages, your company leader was found dead, and you decided to take your chance and lead." + $"\n{startingSkillMult[StartType.Mercenary]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 50, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart17}A cheap outlaw", null), new MBList<SkillObject>
        //    {
        //        DefaultSkills.Roguery,
        //        DefaultSkills.Scouting
        //    }, null, 1, 25, 0, null, new(LooterStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart18}Left impoverished from war, you found a group of like-minded ruffians who were desperate to get by." + $"\n{startingSkillMult[StartType.Looter]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 0, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart19}An Knight of {CULTURE}", null), new MBList<SkillObject>
        //    {
        //        DefaultSkills.Steward,
        //        DefaultSkills.Charm
        //    }, null, 1, 50, 0, null, new(KingdomStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart20}A young noble who came into an arrangement with the king for a chance at land." + $"\n{startingSkillMult[StartType.VassalNoFief]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 150, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart21}A cleric of {CULTURE}", null), new MBList<SkillObject>
        //    {
        //        DoesNotExist.Faith,
        //        DefaultSkills.Riding
        //    }, DefaultCharacterAttributes.Social, 1, 50, 1, null, new(VassalStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart22}With the support of companions you have gathered an army. With limited funds and food you decided it's time for action." + $"\n{startingSkillMult[StartType.KingdomRuler]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 900, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart23}A lord with a castle", null), new MBList<SkillObject>
        //    {
        //        DefaultSkills.Leadership,
        //        DefaultSkills.Steward
        //    }, DefaultCharacterAttributes.Social, 1, 25, 1, null, new(CastleRulerStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart24}You acquired a castle through your own means and declared yourself a kingdom for better or worse." + $"\n{startingSkillMult[StartType.CastleRuler]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 900, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart25}An ursurper of {CULTURE}", null), new MBList<SkillObject>
        //    {
        //        DefaultSkills.Steward,
        //        DefaultSkills.Roguery
        //    }, null, 1, 50, 0, null, new(KnightStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart26}A young noble who came into an arrangement with the king for land." + $"\n{startingSkillMult[StartType.VassalFief]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 150, 0, 0, 0);
        //    characterCreationCategory.AddCategoryOption(new("{=CulturedStart27}A wanderer mystic of {CULTURE}", null), new MBList<SkillObject>
        //    {
        //        DoesNotExist.Arcane,
        //        DefaultSkills.Scouting
        //    }, null, 1, 10, 0, null, new(WandererMysticalStartOnConsequence), new(DoNothingOnApply), new("{=CulturedStart28}A mystic peregrin in pursuit of arcane misteries." + $"\n{startingSkillMult[StartType.EscapedPrisoner]} " + "{=rf_skill_change}times starting skill level multiplier", null), null, 0, 0, 0, 0, 0);
        //    characterCreation.AddNewMenu(characterCreationMenu);
        //}
        protected bool GondorParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "gondor";
        }
        protected bool MordorParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "mordor";
        }

        protected bool RivendellParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "rivendell";
        }
        protected bool MirkwoodParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "mirkwood";
        }

        protected bool LothlorienParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "lothlorien";
        }
        protected bool EreborParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "erebor";
        }
        protected bool IsengardParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "isengard";
        }
        protected bool GundabadParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "gundabad";
        }
        protected bool DolguldurParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "dolguldur";
        }
        protected bool UmbarParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "umbar";
        }
        //protected void StartOnInit(CharacterCreation characterCreation)
        //{
        //    MBTextManager.SetTextVariable("CULTURE", CharacterCreationContentBase.Instance.GetSelectedCulture().Name, false);
        //}
        //private Equipment getMaleEquipment(IEnumerable<Equipment> eq) { return eq.FirstOrDefault(); }
        //private Equipment getFemaleEquipment(IEnumerable<Equipment> eq) { return eq.LastOrDefault(); }
        //protected void ChooseCharacterEquipment(CharacterCreation characterCreation, StartType startType)
        //{
        //    MBEquipmentRoster equipmentRoster;
        //    try
        //    {
        //        equipmentRoster = MBObjectManager.Instance.GetObject<MBEquipmentRoster>(CulturedStartAction.mainHeroStartingEquipment[startType][Hero.MainHero.Culture.StringId]);
        //        IEnumerable<Equipment> battleEquipments = equipmentRoster.GetBattleEquipments();
        //        IEnumerable<Equipment> civillianEquipments = equipmentRoster.GetCivilianEquipments();
        //        Equipment battleEquipment = CharacterObject.PlayerCharacter.IsFemale ? getFemaleEquipment(battleEquipments) : getMaleEquipment(battleEquipments);
        //        Equipment civillianEquipment = CharacterObject.PlayerCharacter.IsFemale ? getFemaleEquipment(civillianEquipments) : getMaleEquipment(civillianEquipments);
        //        if (battleEquipment != null)
        //        {
        //            var a = new List<int> { 1 };
        //            characterCreation.ChangeCharactersEquipment(new List<Equipment> { battleEquipment });
        //            CharacterObject.PlayerCharacter.FirstBattleEquipment.FillFrom(battleEquipment);
        //            ChangePlayerMount(characterCreation, Hero.MainHero);
        //        }
        //        if (civillianEquipment != null) CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(civillianEquipment);
        //    }
        //    catch
        //    {
        //        InformationManager.DisplayMessage(new InformationMessage("Error while giving player the equipment", new Color(255, 0, 0)));
        //    }
        //}
        //protected void DefaultStartOnConsequence(CharacterCreation characterCreation)
        //{

        //    ChooseCharacterEquipment(characterCreation, StartType.Default);
        //    Manager.SetStoryOption(0);
        //}

        //protected void MerchantStartOnConsequence(CharacterCreation characterCreation)
        //{

        //    ChooseCharacterEquipment(characterCreation, StartType.Merchant);
        //    Manager.SetStoryOption(1);
        //}

        //protected void ExiledStartOnConsequence(CharacterCreation characterCreation)
        //{
        //    ChooseCharacterEquipment(characterCreation, StartType.Exiled);
        //    Manager.SetStoryOption(2);
        //}

        //protected void MercenaryStartOnConsequence(CharacterCreation characterCreation)
        //{
        //    ChooseCharacterEquipment(characterCreation, StartType.Mercenary);
        //    Manager.SetStoryOption(3);
        //}

        //protected void LooterStartOnConsequence(CharacterCreation characterCreation)
        //{
        //    ChooseCharacterEquipment(characterCreation, StartType.Looter);
        //    Manager.SetStoryOption(4);
        //}

        //protected void VassalStartOnConsequence(CharacterCreation characterCreation)
        //{

        //    ChooseCharacterEquipment(characterCreation, StartType.VassalFief);
        //    Manager.SetStoryOption(5);
        //}

        //protected void KingdomStartOnConsequence(CharacterCreation characterCreation)
        //{
        //    ChooseCharacterEquipment(characterCreation, StartType.KingdomRuler);
        //    Manager.SetStoryOption(6);
        //}

        //protected void CastleRulerStartOnConsequence(CharacterCreation characterCreation)
        //{

        //    ChooseCharacterEquipment(characterCreation, StartType.CastleRuler);
        //    Manager.SetStoryOption(7);
        //}

        //protected void KnightStartOnConsequence(CharacterCreation characterCreation)
        //{
        //    ChooseCharacterEquipment(characterCreation, StartType.VassalFief);
        //    Manager.SetStoryOption(5);
        //}

        //protected void WandererMysticalStartOnConsequence(CharacterCreation characterCreation)
        //{
        //    ChooseCharacterEquipment(characterCreation, StartType.EscapedPrisoner);
        //    Manager.SetStoryOption(8);
        //}

        //protected bool CastleLocationOnCondition()
        //{
        //    return Manager.StoryOption == 7 || Manager.StoryOption == 8;
        //}

        //protected bool EscapingLocationOnCondition()
        //{
        //    return Manager.StoryOption == 9;
        //}
    }
}
