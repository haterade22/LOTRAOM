using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System.Collections.Generic;
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
        bool BalanceOfPower { get; set; }
    }

    public class HardcodedCustomSettings : ICustomSettingsProvider
    {
        public bool BalanceOfPower { get; set; } = true;
    }

    public class CustomSettings : AttributeGlobalSettings<CustomSettings>, ICustomSettingsProvider
    {
        public override string Id { get; } = "aom_mcm_settings";
        public override string DisplayName => new TextObject("{=CustomSettings_Name}Lord of the Rings Age Of Man {VERSION}", new Dictionary<string, object>
        {
            { "VERSION", typeof(CustomSettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR" }
        }).ToString();

        [SettingPropertyBool("Balance of Power", RequireRestart = true, HintText = "Good & Evil factions will gravitate towards one big alliance, prepare for total war!")]
        [SettingPropertyGroup("{=CustomSettings_General}General")]
        public bool BalanceOfPower { get; set; } = true;
    }
}
