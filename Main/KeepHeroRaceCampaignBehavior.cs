using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace LOTRAOM
{
    public class KeepHeroRaceCampaignBehavior : CampaignBehaviorBase
    {
        private Dictionary<string, int> _heroRaceMap = new();
        public override void RegisterEvents()
        {
            CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, new Action(this.OnSave));
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionStart));
        }
        private void OnSessionStart(CampaignGameStarter obj)
        {
            if (_heroRaceMap.Count > 0)
            {
                foreach (Hero hero in Hero.AllAliveHeroes)
                {
                    if (_heroRaceMap.ContainsKey(hero.StringId) && _heroRaceMap[hero.StringId] != hero.CharacterObject.Race)
                        hero.CharacterObject.Race = _heroRaceMap[hero.StringId];
                }
            }
        }
        private void OnSave()
        {
            _heroRaceMap = new Dictionary<string, int>();
            foreach (Hero hero in Hero.AllAliveHeroes)
            {
                if (!_heroRaceMap.ContainsKey(hero.StringId))
                    _heroRaceMap.Add(hero.StringId, hero.CharacterObject.Race);
            }
        }
        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData<Dictionary<string, int>>("_heroRaceMap", ref _heroRaceMap);
        }
    }
    public class HeroRaceMapSaveableTypeDefiner : SaveableTypeDefiner
    {
        public HeroRaceMapSaveableTypeDefiner() : base(576011) { }
        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(Dictionary<string, int>));
        }
    }
}
