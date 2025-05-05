namespace LOTRAOM.Momentum.Effects
{
    public class BuffHolder
    {
        private static BuffHolder _instance;

        public BuffHolder()
        {
            MoraleBuff = new MoraleMomentumBuff();
        }

        public MoraleMomentumBuff MoraleBuff { get; }
        public static BuffHolder Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
        }
    }
}
