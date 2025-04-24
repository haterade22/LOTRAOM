using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using static LOTRAOM.CampaignStart.CampaignStartGlobals;
using static TaleWorlds.MountAndBlade.Agent;
using static TaleWorlds.PlayerServices.Avatar.AvatarData;

namespace LOTRAOM
{
    public static class Globals
    {
        public static float MountedTroopWageMultiplier => 0.3f;
        public static bool IsNewCampaignCreating = false;
        public static string DunlandCulture { get { return "empire"; } }
        public static string HaradCulture { get { return "aserai"; } }
        public static string RhunCulture { get { return "khuzait"; } }
        public static string RohanCulture { get { return "vlandia"; } }
        public static string DaleCulture { get { return "sturgia"; } }
        public static string KhandCulture { get { return "battania"; } }

        public static string Gondorculture { get { return "gondor"; } }
        public static string RivendellCulture { get { return "rivendell"; } }
        public static string MirkwoodCulture { get { return "mirkwood"; } }
        public static string LothlorienCulture { get { return "lothlorien"; } }
        public static string EreborCulture { get { return "erebor"; } }
        public static string MordorCulture { get { return "mordor"; } }
        public static string GundabadCulture { get { return "gundabad"; } }
        public static string DolguldurCulture { get { return "dolguldur"; } }
        public static string IsengardCulture { get { return "isengard"; } }
        public static string UmbarCulture { get { return "umbar"; } }

        // culture groups
        public static readonly List<string> HumanGoodCulture = new() { Gondorculture, RohanCulture, DaleCulture };
        public static readonly List<string> ElvenCulture = new() { RivendellCulture, MirkwoodCulture, LothlorienCulture };
        public static readonly List<string> DwarvenCulture = new() { EreborCulture };
        public static readonly List<string> OrcishCulture = new() { MordorCulture, GundabadCulture, DolguldurCulture, IsengardCulture };
        public static readonly List<string> HumanEvilCulture = new() { HaradCulture, RhunCulture, UmbarCulture, KhandCulture, DunlandCulture };
        public static readonly List<List<string>> CultureGroups = new() { HumanGoodCulture, ElvenCulture, DwarvenCulture, OrcishCulture, HumanEvilCulture };
        public static bool BelongsToSameCultureGroup(CultureObject culture1, CultureObject culture2)
        {
            foreach (List<string> cultureGroup in CultureGroups)
                if (cultureGroup.Contains(culture1.StringId) && cultureGroup.Contains(culture2.StringId))
                    return true;
            return false;
        }

        public static Kingdom? MordorKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == MordorCulture);
        public static Kingdom? IsengardKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == IsengardCulture);
        public static Kingdom? RohanKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == RohanCulture);
        public static Kingdom? GondorKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == Gondorculture);
        public static Kingdom? DunlandKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == DunlandCulture);

        public static Kingdom? DolGuldurKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == DolguldurCulture);
        public static Kingdom? GundabadKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == GundabadCulture);
        public static Kingdom? MirkwoodKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == MirkwoodCulture);
        public static Kingdom? LorienKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == LothlorienCulture);

        public static Kingdom? RhunKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == RhunCulture);
        public static Kingdom? EreborKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == EreborCulture);
        public static Kingdom? DaleKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == DaleCulture);
        public static Kingdom? UmbarKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == UmbarCulture);
        public static Kingdom? HaradKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == HaradCulture);
        public static Kingdom? KhandKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == KhandCulture);
        public static Kingdom? RivendellKingdom = Kingdom.All.FirstOrDefault(k => k.Culture.StringId == RivendellCulture);

        public static string Orthanc = "town_comp_SWAN_ISENGARD1";

        public static Dictionary<int, string> GetRaceStringIdFromInt { get; } = new();
        
        public static RaceBonus GetRacialData(BasicCharacterObject character)
        {
            GetRaceStringIdFromInt.TryGetValue(character.Race, out string? value);
            value ??= "human";
            return raceBonuses[value];
        }
        // COPY THIS
        private static void GiveRaceBonusWhenGotHit(BasicCharacterObject character, DamageType damage, ref float returnDamange)
        {
            RaceBonus bonus = GetRacialData(character);
            switch (damage)
            {
                case DamageType.Ranged:
                    returnDamange -= bonus.RangedResistance;
                    break;
                case DamageType.Melee:
                    returnDamange -= bonus.MeleeResistance;
                    break;
            }
        }
        public enum DamageType
        {
            Melee,
            Ranged,
            Other // fall damage, kick, other
        }
        public static DamageType GetDefaultDamage(MissionWeapon missionWeapon)
        {
            if (missionWeapon.CurrentUsageItem == null) return DamageType.Other;
            return missionWeapon.CurrentUsageItem.WeaponClass switch
            {
                WeaponClass.Arrow or WeaponClass.Bolt or WeaponClass.Javelin or WeaponClass.ThrowingAxe or WeaponClass.ThrowingKnife or WeaponClass.Stone => DamageType.Ranged,
                // standard melee weapon
                _ => DamageType.Melee,
            };
        }
        // END COPY THIS
        public class RaceBonus
        {
            public int MeleeResistance;
            public int RangedResistance;


            public RaceBonus(int meleeResistance, int rangedResistance)
            {
                MeleeResistance = meleeResistance;
                RangedResistance = rangedResistance;
            }
        }
        public static Dictionary<string, RaceBonus> raceBonuses = new()
        {
            ["human"] = new RaceBonus(0, 0)
        };

    }
}