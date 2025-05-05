using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM
{
    public class AoMSettings
    {
        static ICustomSettingsProvider? _instance;
        public static ICustomSettingsProvider Instance
        {
            get
            {
                if (_instance != null) return _instance;
                if (GlobalSettings<CustomSettings>.Instance != null)
                {
                    _instance = GlobalSettings<CustomSettings>.Instance;
                    string text = "AoM, found MCM module! you can edit the default settings.";
                    InformationManager.DisplayMessage(new InformationMessage(text, new Color(0, 255, 0)));
                    return _instance;
                }
                else
                {
                    string text = "AoM, no MCM module found, using default settings";
                    InformationManager.DisplayMessage(new InformationMessage(text, new Color(255, 0, 0)));
                    _instance = new HardcodedCustomSettings();
                    return _instance;
                }
            }
        }
    }
    public interface ICustomSettingsProvider
    {
        public bool LoreAccurateDiplomacy { get; set; }
        public bool Momentum { get; set; }
        public int IsengardEarliestAttackDate { get; set; }
        public int IsengardLatestAttackDate { get; set; }
    }

    public class HardcodedCustomSettings : ICustomSettingsProvider
    {
        public bool LoreAccurateDiplomacy { get; set; } = true;
        public bool Momentum { get; set; } = true;
        public int IsengardEarliestAttackDate { get; set; } = 10;
        public int IsengardLatestAttackDate { get; set; } = 20;
    }

    public class CustomSettings : AttributeGlobalSettings<CustomSettings>, ICustomSettingsProvider
    {
        public override string Id { get; } = "Age Of Man Settings";
        public override string DisplayName => new TextObject("{=CustomSettings_Name}Lord of the Rings Age Of Man", new Dictionary<string, object>
        {
            { "VERSION", typeof(CustomSettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR" }
        }).ToString();
        public override string FolderName { get; } = "AgeOfMan";
        public override string FormatType { get; } = "json";

        private bool _loreAccurateDiplomacy = true;

        [SettingPropertyBool("AoM Diplomacy", HintText = "Restricted diplomacy, leading to the War of The Ring (good vs evil factions) - prepare for total war!")]
        [SettingPropertyGroup("{=aom_diplomacy}Diplomacy")]
        public bool LoreAccurateDiplomacy
        {
            get => _loreAccurateDiplomacy;
            set
            {
                if (_loreAccurateDiplomacy != value)
                {
                    _loreAccurateDiplomacy = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingPropertyInteger("Isengard earliest attack date", 1, 730, RequireRestart = false, HintText = "CALCULATED AT CAMPAIGN START, default - 1 season = 21 days"),]
        [SettingPropertyGroup("{=aom_diplomacy}Diplomacy")]
        public int IsengardEarliestAttackDate { get; set; } = 21;

        [SettingPropertyInteger("Isengard latest attack date", 1, 730, RequireRestart = false, HintText = "CALCULATED AT CAMPAIGN START default - 1 year = 4 seasons = 84 days")]
        [SettingPropertyGroup("{=aom_diplomacy}Diplomacy")]
        public int IsengardLatestAttackDate { get; set; } = 84;

        [SettingPropertyBool("Momentum", HintText = "NEEDS AoMDIPLOMACY TO WORK Add the momentum mechanic, during the War of the Ring")]
        [SettingPropertyGroup("{=aom_diplomacy}Diplomacy")]
        public bool Momentum { get; set; } = true;
    }
}
