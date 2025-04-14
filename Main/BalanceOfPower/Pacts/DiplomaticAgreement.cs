using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace LOTRAOM.Diplomacy.Pacts
{
    namespace Diplomacy.DiplomaticAction
    {
        public enum AgreementType
        {
            NonAggressionPact
        }
            internal struct FactionPair
            {
                [SaveableProperty(1)]
                internal IFaction Faction1 { readonly get; set; }

                [SaveableProperty(2)]
                internal IFaction Faction2 { readonly get; set; }
                internal FactionPair(IFaction faction1, IFaction faction2)
                {
                    if (string.CompareOrdinal(faction1.StringId, faction2.StringId) < 0)
                    {
                        this.Faction1 = faction1;
                        this.Faction2 = faction2;
                    }
                    else
                    {
                        this.Faction1 = faction2;
                        this.Faction2 = faction1;
                    }
                    this._hashCode = FactionPair.CalculateHash(this.Faction1.StringId + this.Faction2.StringId);
                }
                internal FactionPair(FactionPair other)
                {
                    this.Faction1 = other.Faction1;
                    this.Faction2 = other.Faction2;
                    this._hashCode = other._hashCode;
                }
                public override bool Equals(object obj)
                {
                    if (obj is FactionPair)
                    {
                        FactionPair p = (FactionPair)obj;
                        return this.Equals(p);
                    }
                    return false;
                }
                public bool Equals(FactionPair p)
                {
                    return this.Faction1 == p.Faction1 && this.Faction2 == p.Faction2;
                }
                public static bool operator ==(FactionPair p1, FactionPair p2)
                {
                    return p1.Equals(p2);
                }
                public static bool operator !=(FactionPair p1, FactionPair p2)
                {
                    return !p1.Equals(p2);
                }
                private static int CalculateHash(string s)
                {
                    int num = 0;
                    foreach (char c in s)
                    {
                        num *= 17;
                        num += (int)(char.ToUpper(c) - '0');
                    }
                    return num;
                }
                public override int GetHashCode()
                {
                    return this._hashCode;
                }
                [SaveableField(3)]
                private readonly int _hashCode;
            }

        internal abstract class DiplomaticAgreement
        {
            [SaveableProperty(1)]
            public CampaignTime StartDate { get; protected set; }

            [SaveableProperty(2)]
            public CampaignTime EndDate { get; protected set; }

            [SaveableProperty(3)]
            public FactionPair Factions { get; set; }

            [SaveableProperty(4)]
            public bool ExpireNotified { get; protected set; }

            protected DiplomaticAgreement(CampaignTime startdate, CampaignTime endDate, Kingdom kingdom, Kingdom otherKingdom) : this(startdate, endDate, new FactionPair(kingdom, otherKingdom))
            {
            }

            protected DiplomaticAgreement(CampaignTime startdate, CampaignTime endDate, FactionPair factionMapping)
            {
                this.StartDate = startdate;
                this.EndDate = endDate;
                this.Factions = factionMapping;
            }

            public abstract AgreementType GetAgreementType();

            public virtual void Expire()
            {
                this.EndDate = CampaignTime.Now - CampaignTime.Milliseconds(1L);
                this.TryExpireNotification();
            }

            public void TryExpireNotification()
            {
                if (!this.ExpireNotified && this.IsExpired())
                {
                    //LoggerExtensions.LogTrace(LogFactory.Get<DiplomaticAgreement>(), string.Format("[{0}] Agreement expired between {1} and {2}:", CampaignTime.Now, this.Factions.Faction1.Name, this.Factions.Faction2.Name) + " " + Enum.GetName(typeof(AgreementType), this.GetAgreementType()), Array.Empty<object>());
                    this.NotifyExpired();
                    this.ExpireNotified = true;
                }
            }
            public abstract void NotifyExpired();
            public bool IsExpired()
            {
                return this.EndDate.IsPast;
            }
        }
    }
}
