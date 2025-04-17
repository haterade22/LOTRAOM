using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace LOTRAOM.Momentum
{
    public class WarOfTheRingFactionData
    {
        [SaveableField(0)]
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
    }
    public class WarOfTheRingData
    {
        [SaveableField(0)] public Dictionary<MomentumActionType, Queue<MomentumEvent>> WarOfTheRingEvents = new();
        [SaveableField(1)] WarOfTheRingFactionData _goodKingdoms = new();
        [SaveableField(2)] WarOfTheRingFactionData _evilKingdoms = new();
        [SaveableField(3)] private int momentum = 0;

        public WarOfTheRingFactionData GoodKingdoms { get { return _goodKingdoms; } }
        public WarOfTheRingFactionData EvilKingdoms { get { return _evilKingdoms; } }
        public bool DoesFactionTakePartInWar(IFaction kingdom)
        {
            return GoodKingdoms.Kingdoms.Contains(kingdom) || EvilKingdoms.Kingdoms.Contains(kingdom);
        }
        public void AddEvent(MomentumActionType type, MomentumEvent momentumEvent)
        {
            if (!WarOfTheRingEvents.ContainsKey(type))
                WarOfTheRingEvents.Add(type, new Queue<MomentumEvent>());
            WarOfTheRingEvents[type].Enqueue(momentumEvent);
            EditMomentum(momentumEvent.MomentumValue);
        }
        public bool HasWarStarted()
        {
            return GoodKingdoms.Kingdoms.Count + EvilKingdoms.Kingdoms.Count > 0;
        }
        public int Momentum // goes between 100 and -100
        {
            get
            {
                return momentum;
            }
        }
        public void EditMomentum(int amount)
        {
            momentum += amount;
            if (momentum > 100)
                momentum = 100;
            else if (momentum < -100)
                momentum = -100;
        }
    }
}
