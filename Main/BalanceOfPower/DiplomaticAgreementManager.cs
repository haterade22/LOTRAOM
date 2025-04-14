//using System;
//using System.Collections.Generic;
//using System.Linq;
//using JetBrains.Annotations;
//using LOTRAOM.Diplomacy.Pacts.Diplomacy.DiplomaticAction;
//using TaleWorlds.CampaignSystem;
//using TaleWorlds.SaveSystem;

//namespace LOTRAOM.BalanceOfPower
//{
//    internal class DiplomaticAgreementManager
//    {
//        public static DiplomaticAgreementManager Instance { get; internal set; }
//        public DiplomaticAgreementManager()
//        {
//            _agreements = new Dictionary<FactionPair, List<DiplomaticAgreement>>();
//            Instance = this;
//        }
//        public Dictionary<FactionPair, List<DiplomaticAgreement>> Agreements
//        {
//            get
//            {
//                return _agreements;
//            }
//        }
//        public static bool HasNonAggressionPact(Kingdom kingdom, Kingdom otherKingdom, out NonAggressionAgreement pactAgreement)
//        {
//            List<DiplomaticAgreement> source;
//            if (Instance.Agreements.TryGetValue(new FactionPair(kingdom, otherKingdom), out source))
//            {
//                IEnumerable<DiplomaticAgreement> source2 = from agreement in source
//                                                           where agreement.GetAgreementType() == AgreementType.NonAggressionPact && !agreement.IsExpired()
//                                                           select agreement;
//                pactAgreement = source2.OfType<NonAggressionAgreement>().FirstOrDefault();
//                return pactAgreement != null;
//            }
//            pactAgreement = null;
//            return false;
//        }
//        public static void RegisterAgreement(Kingdom kingdom, Kingdom otherKingdom, DiplomaticAgreement diplomaticAgreement)
//        {
//            FactionPair key = new FactionPair(kingdom, otherKingdom);
//            List<DiplomaticAgreement> list;
//            if (Instance.Agreements.TryGetValue(key, out list))
//            {
//                list.Add(diplomaticAgreement);
//                return;
//            }
//            Instance.Agreements[key] = new List<DiplomaticAgreement>
//            {
//                diplomaticAgreement
//            };
//        }
//        public void Sync()
//        {
//            Instance = this;
//        }

//        [SaveableField(1)]
//        private Dictionary<FactionPair, List<DiplomaticAgreement>> _agreements;
//    }
//}
