using System;
using TaleWorlds.Localization;

namespace LOTRAOM.Momentum.Effects
{
    public abstract class MomentumBuff
    {
        protected MomentumBuff(MomentumBuffType buffType, int minMomentumToApply, float minEffect, float maxEffect)
        {
            BuffType = buffType;
            MinMomentumToApply = minMomentumToApply;
            MaxEffect = maxEffect;
            MinEffect = minEffect;
        }
        public abstract TextObject Text { get; }
        public MomentumBuffType BuffType { get; }
        public abstract bool Condition(object[] args);
        public abstract void Consequence(object[] args);
        public int MinMomentumToApply { get; }
        public float MaxEffect { get; }
        public float MinEffect { get; }
        public float GetEffect()
        {
            float momentum = Math.Abs(MomentumCampaignBehavior.Instance.warOfTheRingData.Momentum);
            return BuffType switch
            {
                MomentumBuffType.Continuous => (MaxEffect - MinEffect) * momentum / 100,
                MomentumBuffType.OneTime => MaxEffect,
                _ => 0,
            };
        }
    }
}
