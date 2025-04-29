using LOTRAOM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace LOTRAOM.Momentum
{
    public class WarOfTheRingFactionData
    {
        [SaveableField(0)] List<Kingdom> _kingdoms = new();
        [SaveableField(1)] public Dictionary<MomentumActionType, Queue<MomentumEvent>> WarOfTheRingEvents = new();
        [SaveableField(2)] private int momentum = 0;
        [SaveableField(3)] MomentumFactionTotalStats _factionTotalStats = new();

        public MomentumFactionTotalStats FactionTotalStats { get { return _factionTotalStats; } }
        public WarOfTheRingFactionData()
        {
            foreach (MomentumActionType eventType in Enum.GetValues(typeof(MomentumActionType)))
            {
                if (!WarOfTheRingEvents.ContainsKey(eventType))
                    WarOfTheRingEvents[eventType] = new();
            }
        }

        public int WarSideMomentum { get { return momentum; } }
        public List<Kingdom> Kingdoms { get { return _kingdoms; } }
        public float TotalStrength
        {
            get
            {
                float totalStrength = 0;
                foreach (var kingdom in Kingdoms)
                {
                    totalStrength += kingdom.TotalStrength;
                }
                return totalStrength;
            }
        }
        public void AddEvent(MomentumActionType type, MomentumEvent momentumEvent)
        {
            if (!WarOfTheRingEvents.ContainsKey(type))
                WarOfTheRingEvents.Add(type, new Queue<MomentumEvent>());
            WarOfTheRingEvents[type].Enqueue(momentumEvent);
            EditMomentum(momentumEvent.MomentumValue);
        }
        public void EditMomentum(int amount)
        {
            momentum += amount;
        }
    }
    public enum WarOfTheRingSide
    {
        Good,
        Evil
    }
    public class WarOfTheRingData
    {
        [SaveableField(0)] WarOfTheRingFactionData _goodKingdoms = new();
        [SaveableField(1)] WarOfTheRingFactionData _evilKingdoms = new();
        [SaveableField(2)] private bool hasWarEnded = false;
        public bool HasWarEnded { get; }
        [SaveableField(3)] private bool hasWarStarted = false;
        public bool HasWarStarted { get { return hasWarStarted; } }
        [SaveableField(4)] private int lastDaysMomentumFromStrengthComparison = 0;
        [SaveableField(5)] private int todaysMomentumFromStrengthCompariton = 0;

        public WarOfTheRingFactionData GoodKingdoms { get { return _goodKingdoms; } }
        public WarOfTheRingFactionData EvilKingdoms { get { return _evilKingdoms; } }
        public bool DoesFactionTakePartInWar(IFaction kingdom)
        {
            return GoodKingdoms.Kingdoms.Any(k => k.StringId == kingdom.StringId) || EvilKingdoms.Kingdoms.Any(k => k.StringId == kingdom.StringId);
        }
        public void AddEvent(Kingdom kingdom, MomentumActionType type, MomentumEvent momentumEvent)
        {
            if (kingdom.Culture.IsGoodCulture())
                GoodKingdoms.AddEvent(type, momentumEvent);
            else
                EvilKingdoms.AddEvent(type, momentumEvent);
        }
        public void AddEvent(WarOfTheRingSide side, MomentumActionType type, MomentumEvent momentumEvent)
        {
            if (side == WarOfTheRingSide.Good)
                GoodKingdoms.AddEvent(type, momentumEvent);
            else
                EvilKingdoms.AddEvent(type, momentumEvent);
        }
        public void AddKingdom(Kingdom kingdom)
        {
            if (!HasWarStarted)
                MomentumCampaignBehavior.Instance.AddMomentumUI();
            hasWarStarted = true;
            if (kingdom.Culture.IsGoodCulture())
            {
                GoodKingdoms.Kingdoms.Add(kingdom);
                foreach(var evilKingdom in EvilKingdoms.Kingdoms)
                    FactionManager.DeclareWar(kingdom, evilKingdom);
            }
            else
            {
                EvilKingdoms.Kingdoms.Add(kingdom);
                foreach (var evilKingdom in GoodKingdoms.Kingdoms)
                    FactionManager.DeclareWar(kingdom, evilKingdom);
            }
        }
        public bool ShouldWarEnd()
        {
            if (Math.Abs(Momentum) == 100)
            {
                hasWarEnded = true;
                return true;
            }
            return false;
        }
        public int Momentum // goes between 100 and -100
        {
            get
            {
                int momValue = GoodKingdoms.WarSideMomentum - EvilKingdoms.WarSideMomentum;
                return Math.Min(Math.Max(momValue, -100), 100);
            }
        }

    }
}
