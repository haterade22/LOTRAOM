using TaleWorlds.Core;
using System.Collections.Generic;

namespace LOTRAOM
{
    public static class CultureManager
    {
        public struct CultureBonus
        {
            public Dictionary<TerrainType, float> TerrainDamageBonuses { get; private set; }

            public CultureBonus(Dictionary<TerrainType, float> terrainDamageBonuses)
            {
                TerrainDamageBonuses = terrainDamageBonuses ?? new Dictionary<TerrainType, float>();
            }
        }

        private static readonly Dictionary<string, CultureBonus> _cultureBonuses = new Dictionary<string, CultureBonus>
        {
            ["vlandia"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Plain, 0.15f } }), // Rohirrim: Bonus on plains, reflecting their mastery of cavalry warfare (e.g., Battle of the Pelennor Fields).
            ["gondor"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.RuralArea, 0.1f } }), // Gondorians: Bonus in rural areas, representing their fortified farmlands and disciplined infantry.
            ["rivendell"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Forest, 0.15f } }), // Ñoldor Elves: Forest bonus, reflecting their agility and lore-based affinity for wooded areas (e.g., Rivendell’s hidden valley).
            ["mirkwood"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Forest, 0.15f } }), // Silvan Elves: Forest bonus, tied to their woodland realm and archery prowess.
            ["lothlorien"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Forest, 0.15f } }), // Galadhrim Elves: Forest bonus, emphasizing their mystical connection to Lórien’s woods.
            ["erebor"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Mountain, 0.2f } }), // Dwarves: Mountain bonus, reflecting their resilience and combat skill in rugged terrain (e.g., Battle of Dale).
            ["aserai"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Desert, 0.1f }, { TerrainType.Dune, 0.1f } }), // Haradrim: Desert and dune bonus, suited to their Southron origins.
            ["umbar"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.River, 0.1f } }), // Umbar: River bonus, tied to their corsair and naval prowess along coasts and rivers.
            ["khuzait"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Steppe, 0.1f } }), // Easterlings: Steppe bonus, reflecting their nomadic and disciplined infantry tactics.
            ["battania"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Steppe, 0.1f } }), // Variags: Steppe bonus, aligning with their Khand-based nomadic warrior culture.
            ["sturgia"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Snow, 0.1f } }), // Dunlendings: Snow bonus, loosely tied to their rugged, northern-inspired origins.
            ["empire"] = new CultureBonus(new Dictionary<TerrainType, float> { { TerrainType.Plain, 0.1f } }) // Men of Dale: Plain bonus, reflecting their open-field combat style alongside Rohirrim allies.
        };

        public static bool TryGetTerrainBonus(string cultureId, TerrainType terrainType, out float bonus)
        {
            bonus = 0f;
            if (_cultureBonuses.TryGetValue(cultureId, out var cultureBonus) &&
                cultureBonus.TerrainDamageBonuses.TryGetValue(terrainType, out bonus))
            {
                return true;
            }
            return false;
        }
    }
}