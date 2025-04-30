using System.Collections.Generic;


namespace LOTRAOM.Events
{
    public class ForceWar
    {
        public string WarEventId { get; }
        public int DaysAfterIsengardWar { get; }

        public ForceWar(string warEventId, int daysAfterIsengardWar)
        {
            WarEventId = warEventId;
            DaysAfterIsengardWar = daysAfterIsengardWar;
        }
        public static List<ForceWar> All = new()
        {
            new("mordor_war", 20),
            new("rhun_erebor_dale_war", 30),
            new("umbar_harad_khand_gondor_war", 35),
            new("dol_guldur_gundabad_mirkwood_lorien_war", 40),
        };
    }
}