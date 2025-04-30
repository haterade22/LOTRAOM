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
        [DataSourceProperty] public string Duration { get; set; }
        [DataSourceProperty] public string BalanceOfPowerOverview { get; set; }
        [DataSourceProperty] public MBBindingList<BreakdownVM> MomentumBreakdown { get; set; }
        [DataSourceProperty] public string StatsLabel { get; set; }
        [DataSourceProperty] public MBBindingList<BreakdownVM> StatsBreakdown { get; set; }

        [DataSourceProperty] public HintViewModel HelpHint { get; set; }

        [DataSourceProperty] public string WarExhaustionRate { get; set; }

        [DataSourceProperty] public HintViewModel RateHelpHint { get; set; }

        [DataSourceProperty] public string ScreenName { get { return new TextObject("War of the Ring").ToString(); } }

        private MBBindingList<FactionRelationshipVM> _goodFactionParticipants1 = new();
        private MBBindingList<FactionRelationshipVM> _goodFactionParticipants2 = new();
        private MBBindingList<FactionRelationshipVM> _evilFactionParticipants1 = new();
        private MBBindingList<FactionRelationshipVM> _evilFactionParticipants2 = new();
        private WarOfTheRingData warOfTheRingData { get { return MomentumCampaignBehavior.Instance.warOfTheRingData; } }

        MBBindingList<FactionRelationshipVM> _goodFactionLeader;
        MBBindingList<FactionRelationshipVM> _evilFactionLeader;
        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> GoodFactionLeader { get { return _goodFactionLeader; } }
        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> EvilFactionLeader { get { return _evilFactionLeader; } }

        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> GoodFactionParticipants
        {
            get => _goodFactionParticipants1;
            set
            {
                if (value != _goodFactionParticipants1)
                {
                    _goodFactionParticipants1 = value;
                    OnPropertyChanged(nameof(GoodFactionParticipants));
                }
            }
        }
        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> EvilFactionParticipants
        {
            get => _evilFactionParticipants1;
            set
            {
                if (value != _evilFactionParticipants1)
                {
                    _evilFactionParticipants1 = value;
                    OnPropertyChanged(nameof(EvilFactionParticipants));
                }
            }
        }
        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> GoodFactionParticipants2
        {
            get => _goodFactionParticipants2;
            set
            {
                if (value != _goodFactionParticipants2)
                {
                    _goodFactionParticipants2 = value;
                    OnPropertyChanged(nameof(GoodFactionParticipants));
                }
            }
        }
        [DataSourceProperty] public MBBindingList<FactionRelationshipVM> EvilFactionParticipants2
        {
            get => _evilFactionParticipants2;
            set
            {
                if (value != _evilFactionParticipants2)
                {
                    _evilFactionParticipants2 = value;
                    OnPropertyChanged(nameof(EvilFactionParticipants));
                }
            }
        }
        [DataSourceProperty] public ImageIdentifierVM GoodAllianceVisual { get; set; } = null!;
        [DataSourceProperty] public ImageIdentifierVM EvilAllianceVisual { get; set; } = null!;
        [DataSourceProperty] public string Momentum
        {
            get
            {
                return "Total: " + ((int)MomentumCampaignBehavior.Instance.warOfTheRingData.Momentum).ToString();
            }
        }
        public MomentumVM(Action onFinalize)
        {
            Kingdom mordor = Globals.MordorKingdom;
            Kingdom gondor = Globals.GondorKingdom;
            _goodFactionLeader = new() { new FactionRelationshipVM(gondor) };
            _evilFactionLeader = new() { new FactionRelationshipVM(mordor) };
            EvilAllianceVisual = new ImageIdentifierVM(BannerCode.CreateFrom(mordor.Banner), true);
            GoodAllianceVisual = new ImageIdentifierVM(BannerCode.CreateFrom(gondor.Banner), true);
            _finalize = onFinalize;
            Duration = new TextObject("info 2").ToString();
            CalculateBreakdowns();
            CreateTotalStats();
            BalanceOfPowerOverview = new TextObject("Momentum Breakdown ").ToString();
            StatsLabel = "Total stats";
            RefreshValues();
            FillKingdoms();
        }
        private void FillKingdoms()
        {

            List<Kingdom> goodKingdoms = warOfTheRingData.GoodKingdoms.Kingdoms;
            for (int i = 0; i < goodKingdoms.Count; i++)
            {
                if (i % 2 == 0) GoodFactionParticipants.Add(new FactionRelationshipVM(goodKingdoms[i]));
                else GoodFactionParticipants2.Add(new FactionRelationshipVM(goodKingdoms[i]));
            }
            List<Kingdom> evilKingdoms = warOfTheRingData.EvilKingdoms.Kingdoms;
            for (int i = 0; i < evilKingdoms.Count; i++)
            {
                if (i % 2 == 0) EvilFactionParticipants.Add(new FactionRelationshipVM(evilKingdoms[i]));
                else EvilFactionParticipants2.Add(new FactionRelationshipVM(evilKingdoms[i]));
            }
        }
        private void CreateTotalStats()
        {
            List<TextObject> names = new()
            {
                new("Enemies killed"), new("Settlements captured"), new("Villages raided")
            };
            List<Func<MomentumFactionTotalStats, int>> values = new()
            {
                x => x.TotalKills,
                x => x.TotalSettlementsCaptured,
                x => x.TotalVillagesRaided
            };
            MBBindingList<BreakdownVM> momentumStats = new();
            for (int i = 0; i < names.Count; i++)
            {
                BreakdownVM stat = new(names[i], values[i].Invoke(warOfTheRingData.GoodKingdoms.FactionTotalStats).ToString(),
                    values[i].Invoke(warOfTheRingData.EvilKingdoms.FactionTotalStats).ToString());
                momentumStats.Add(stat);
            }
            StatsBreakdown = momentumStats;
        }
        private void CalculateBreakdowns()
        {
            MomentumBreakdown = new();
            foreach (MomentumActionType eventType in Enum.GetValues(typeof(MomentumActionType)))
            {
                Queue<MomentumEvent> goodEvents = warOfTheRingData.GoodKingdoms.WarOfTheRingEvents[eventType];
                Queue<MomentumEvent> badEvents = warOfTheRingData.EvilKingdoms.WarOfTheRingEvents[eventType];
                MomentumBreakdown tempBreakdown = new(eventType, goodEvents, badEvents);
                MomentumBreakdown.Add(new BreakdownVM(tempBreakdown));
            }
        }
        private void OnComplete()
        {
            _finalize();
        }
    }
}