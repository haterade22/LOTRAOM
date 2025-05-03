using LOTRAOM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace LOTRAOM.Momentum
{
    public class WarOfTheRingFactionData
    {
        [SaveableField(0)] List<string> _kingdomsStringIds = new();
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
        List<Kingdom> _kingdoms = new();
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
        public bool AddKingdom(Kingdom kingdom)
        {
            if (Kingdoms.Any(k => k.StringId == kingdom.StringId)) return false;
            Kingdoms.Add(kingdom);
            _kingdomsStringIds.Add(kingdom.StringId);
            return true;
        }
        public void SyncData()
        {
            _kingdoms = new();
            foreach (string kingdom in _kingdomsStringIds)
            {
                Kingdoms.Add(Campaign.Current.Kingdoms.FirstOrDefault(k => k.StringId == kingdom)!);
            }
        }

        internal void RemoveKingdom(Kingdom kingdom)
        {
            if (Kingdoms.Any(k => k.StringId == kingdom.StringId))
            {
                Kingdoms.Remove(kingdom);
                _kingdomsStringIds.Remove(kingdom.StringId);
            }
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
        [SaveableField(2)] private bool _hasWarEnded = false;
        public bool HasWarEnded { get { return _hasWarEnded; } }
        [SaveableField(3)] private bool _hasWarStarted = false;
        public bool HasWarStarted { get { return _hasWarStarted; } }

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
            _hasWarStarted = true;
            if (kingdom.Culture.IsGoodCulture() && GoodKingdoms.AddKingdom(kingdom))
            {
                foreach(var evilKingdom in EvilKingdoms.Kingdoms)
                    FactionManager.DeclareWar(kingdom, evilKingdom);
            }
            if (kingdom.Culture.IsEvilCulture() && EvilKingdoms.AddKingdom(kingdom))
            {
                foreach (var goodKingdom in GoodKingdoms.Kingdoms)
                    FactionManager.DeclareWar(kingdom, goodKingdom);
            }
        }
        public bool EndWarIfConditionsMet()
        {
            if (HasWarEnded) return true;
            if (!HasWarStarted) return false;
            if (Momentum == 100 || EvilKingdoms.Kingdoms.Count == 0)
            {
                _hasWarEnded = true;
                InquiryData data = new("End of the war", new TextObject($"The war is over. We have persevered, and won the future for our children. Long live freedom!").ToString(), true, false, new TextObject("Continue").ToString(), "", () => { }, () => { });
                InformationManager.ShowInquiry(data, true, false);
                return true;
            }
            if (Momentum == -100 || GoodKingdoms.Kingdoms.Count == 0)
            {
                _hasWarEnded = true;
                InquiryData data = new("End of the war", new TextObject($"The realms of man and elf alike have been crushed. Long live the empire of Sauron!").ToString(), true, false, new TextObject("Continue").ToString(), "", () => { }, () => { });
                InformationManager.ShowInquiry(data, true, false);
                return true;
            }
            return false;
        }
        public float Momentum // goes between 100 and -100 (divided by a 100)
        {
            get
            {
                float momValue = GoodKingdoms.WarSideMomentum - EvilKingdoms.WarSideMomentum;
                momValue /= MomentumGlobals.MomentumMultiplier;
                return Math.Min(Math.Max(momValue, -100), 100);
            }
        }
        public void RemoveKingdom(Kingdom kingdom)
        {
            if (kingdom.Culture.IsGoodCulture())
                GoodKingdoms.RemoveKingdom(kingdom);

            if (kingdom.Culture.IsEvilCulture())
                EvilKingdoms.RemoveKingdom(kingdom);
            EndWarIfConditionsMet();
        }

        public void SyncData()
        {
            GoodKingdoms.SyncData();
            EvilKingdoms.SyncData();
        }
    }
}
