using LOTRAOM.Momentum;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using System.Collections.Generic;
namespace LOTRAOM.Momentum.ViewModel
{
    internal sealed class MomentumVM : TaleWorlds.Library.ViewModel
    {
        private readonly Action _finalize;
        [DataSourceProperty] public string StartDate { get; set; }
        [DataSourceProperty] public string Duration { get; set; }
        [DataSourceProperty] public string BalanceOfPowerOverview { get; set; }
        [DataSourceProperty] public MBBindingList<MomentumBreakdownVM> Breakdowns { get; set; }
        [DataSourceProperty] public string StatsLabel { get; set; }
        [DataSourceProperty] public MBBindingList<MomentumStatVM> Stats { get; set; } = null!;

        [DataSourceProperty] public HintViewModel HelpHint { get; set; }

        [DataSourceProperty] public string WarExhaustionRate { get; set; }

        [DataSourceProperty] public HintViewModel RateHelpHint { get; set; }

        [DataSourceProperty] public string ScreenName { get { return new TextObject("War of the Ring").ToString(); } }

        private MBBindingList<FactionRelationshipVM> _goodFactionParticipants = new();
        private MBBindingList<FactionRelationshipVM> _evilFactionParticipants = new();
        private WarOfTheRingData warOfTheRingData { get { return MomentumCampaignBehavior.Instance.warOfTheRingData; } }

        MBBindingList<FactionRelationshipVM> _goodFactionLeader;
        MBBindingList<FactionRelationshipVM> _evilFactionLeader;
        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> GoodFactionLeader { get { return _goodFactionLeader; } }
        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> EvilFactionLeader { get { return _evilFactionLeader; } }

        [DataSourceProperty]
        public MBBindingList<FactionRelationshipVM> GoodFactionParticipants
        {
            get => _goodFactionParticipants;
            set
            {
                if (value != _goodFactionParticipants)
                {
                    _goodFactionParticipants = value;
                    OnPropertyChanged(nameof(GoodFactionParticipants));
                }
            }
        }
        [DataSourceProperty]
        public MBBindingList<FactionRelationshipVM> EvilFactionParticipants
        {
            get => _evilFactionParticipants;
            set
            {
                if (value != _evilFactionParticipants)
                {
                    _evilFactionParticipants = value;
                    OnPropertyChanged(nameof(EvilFactionParticipants));
                }
            }
        }
        [DataSourceProperty] public ImageIdentifierVM GoodAllianceVisual { get; set; } = null!;
        [DataSourceProperty] public ImageIdentifierVM EvilAllianceVisual { get; set; } = null!;
        public MomentumVM(Action onFinalize)
        {
            Kingdom mordor = MomentumGlobals.Mordor;
            Kingdom gondor = MomentumGlobals.Gondor;
            _goodFactionLeader = new()
            {
                new FactionRelationshipVM(gondor)
            };
            _evilFactionLeader = new()
            {
                new FactionRelationshipVM(mordor)
            };
            EvilAllianceVisual = new ImageIdentifierVM(BannerCode.CreateFrom(mordor.Banner), true);
            GoodAllianceVisual = new ImageIdentifierVM(BannerCode.CreateFrom(gondor.Banner), true);
            _finalize = onFinalize;
            StartDate = new TextObject("info 1").ToString();
            Duration = new TextObject("info 2").ToString();
            CalculateBreakdowns();
            Stats = CreateTotalStats();

            BalanceOfPowerOverview = new TextObject("").ToString();
            RateHelpHint = new HintViewModel(GameTexts.FindText("str_warexhaustionrate_help"));
            StatsLabel = "Total stats";
            RefreshValues();

            foreach (var kingdom in warOfTheRingData.GoodKingdoms.Kingdoms)
            {
                GoodFactionParticipants.Add(new FactionRelationshipVM(kingdom));
            }
            foreach (var kingdom in warOfTheRingData.EvilKingdoms.Kingdoms)
            {
                EvilFactionParticipants.Add(new FactionRelationshipVM(kingdom));
            }
        }

        private MBBindingList<MomentumStatVM> CreateTotalStats()
        {
            List<string> names = new()
            {
                "Enemies killed", "Settlements captured", "Villages raided"
            };
            List<Func<MomentumFactionTotalStats, int>> values = new()
            {
                x => x.TotalKills,
                x => x.TotalSettlementsCaptured,
                x => x.TotalVillagesRaided
            };
            MBBindingList<MomentumStatVM> momentumStats = new();
            for (int i = 0; i < names.Count; i++)
            {
                MomentumStatVM stat = new(names[i], values[i].Invoke(warOfTheRingData.GoodKingdoms.FactionTotalStats).ToString(),
                    values[i].Invoke(warOfTheRingData.EvilKingdoms.FactionTotalStats).ToString());
                momentumStats.Add(stat);
            }
            return momentumStats;
        }
        private void CalculateBreakdowns()
        {
            Breakdowns = new();
            Breakdowns.Clear();

            foreach (MomentumActionType eventType in Enum.GetValues(typeof(MomentumActionType)))
            {
                Queue<MomentumEvent> goodEvents = warOfTheRingData.GoodKingdoms.WarOfTheRingEvents[eventType];
                Queue<MomentumEvent> badEvents = warOfTheRingData.EvilKingdoms.WarOfTheRingEvents[eventType];
                MomentumTempBreakdown tempBreakdown = new(eventType, goodEvents, badEvents);
                Breakdowns.Add(new MomentumBreakdownVM(tempBreakdown));
            }
        }

        //private HintViewModel CreateHint(Kingdom kingdom1, Kingdom kingdom2)
        //{
        //    TextObject textObject;
        //    textObject = new("hint");
        //    return new HintViewModel(textObject);
        //}

        //public override void RefreshValues()
        //{
        //    base.RefreshValues();
        //}

        private void OnComplete()
        {
            _finalize();
        }
    }
}