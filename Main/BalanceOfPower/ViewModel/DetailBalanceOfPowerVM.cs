using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace LOTRAOM.BalanceOfPower.ViewModel
{
    internal sealed class DetailBalanceOfPowerVM : TaleWorlds.Library.ViewModel
    {
        private readonly Action _finalize;
        [DataSourceProperty] public string StartDate { get; set; }
        [DataSourceProperty] public string Duration { get; set; }
        [DataSourceProperty] public string BalanceOfPowerOverview { get; set; }
        [DataSourceProperty] public MBBindingList<BalanceOfPowerBreakdownVM> Breakdowns { get; set; }
        [DataSourceProperty] public string StatsLabel { get; set; }
        [DataSourceProperty] public MBBindingList<BalanceOfPowerStatVM> Stats { get; set; } = null!;

        [DataSourceProperty] public HintViewModel HelpHint { get; set; }

        [DataSourceProperty] public string WarExhaustionRate { get; set; }

        [DataSourceProperty] public HintViewModel RateHelpHint { get; set; }

        [DataSourceProperty] public string ScreenName { get { return new TextObject("Balance of Power").ToString(); } }

        private MBBindingList<DiplomacyFactionRelationshipVM> _faction1Allies = new();
        private MBBindingList<DiplomacyFactionRelationshipVM> _faction2Allies = new();
        [DataSourceProperty]
        public MBBindingList<DiplomacyFactionRelationshipVM> Faction1Allies
        {
            get => _faction1Allies;
            set
            {
                if (value != _faction1Allies)
                {
                    _faction1Allies = value;
                    OnPropertyChanged(nameof(Faction1Allies));
                }
            }
        }
        [DataSourceProperty]
        public MBBindingList<DiplomacyFactionRelationshipVM> Faction2Allies
        {
            get => _faction2Allies;
            set
            {
                if (value != _faction2Allies)
                {
                    _faction2Allies = value;
                    OnPropertyChanged(nameof(Faction2Allies));
                }
            }
        }
        [DataSourceProperty] public ImageIdentifierVM GoodAllianceVisual { get; set; } = null!;
        [DataSourceProperty] public ImageIdentifierVM EvilAllianceVisual { get; set; } = null!;
        public DetailBalanceOfPowerVM(Action onFinalize)
        {
            Kingdom mordor = BalanceOfPowerGlobals.Mordor;
            Kingdom gondor = BalanceOfPowerGlobals.Gondor;
            GoodAllianceVisual = new ImageIdentifierVM(BannerCode.CreateFrom(gondor.Banner), true);
            EvilAllianceVisual = new ImageIdentifierVM(BannerCode.CreateFrom(mordor.Banner), true);
            _finalize = onFinalize;
            StartDate = new TextObject("info 1").ToString();
            Duration = new TextObject("info 2").ToString();

            //Duration = new TextObject("{=qHrihV27}War Duration: {WAR_DURATION} days")
            //    .SetTextVariable("WAR_DURATION", (int)warStartDate.ElapsedDaysUntilNow)
            //    .ToString();
            Breakdowns = new();
            var breakdowns = BalanceOfPowerGlobals.MockBalanceOfPowerBreakdown();
            Stats = BalanceOfPowerGlobals.MockTotalStats();
            Breakdowns.Clear();
            foreach (var breakdown in breakdowns) Breakdowns.Add(new BalanceOfPowerBreakdownVM(breakdown));

            BalanceOfPowerOverview = new TextObject("").ToString();
            RateHelpHint = new HintViewModel(GameTexts.FindText("str_warexhaustionrate_help"));
            StatsLabel = "Total stats";
            RefreshValues();

        }
        //private HintViewModel CreateHint(Kingdom kingdom1, Kingdom kingdom2)
        //{
        //    TextObject textObject;
        //    textObject = new("hint");
        //    return new HintViewModel(textObject);
        //}

        public override void RefreshValues()
        {
            base.RefreshValues();
            Kingdom mordor = BalanceOfPowerGlobals.Mordor;
            Kingdom gondor = BalanceOfPowerGlobals.Gondor;
            Faction1Allies.Add(new DiplomacyFactionRelationshipVM(gondor));
            Faction2Allies.Add(new DiplomacyFactionRelationshipVM(mordor));
        }

        private void OnComplete()
        {
            _finalize();
        }
    }
}