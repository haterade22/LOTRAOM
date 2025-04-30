using JetBrains.Annotations;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace LOTRAOM.Momentum.ViewModel
{
    public sealed class FactionRelationshipVM : TaleWorlds.Library.ViewModel
    {
        public IFaction Faction { get; init; }
        private ImageIdentifierVM _imageIdentifier;
        private string _nameText;

        public FactionRelationshipVM(IFaction faction)
        {
            Faction = faction;
            _imageIdentifier = new ImageIdentifierVM(BannerCode.CreateFrom(faction.Banner), true);
            _nameText = Faction.Name.ToString();
        }
        private void ExecuteLink() => Campaign.Current.EncyclopediaManager.GoToLink(Faction.EncyclopediaLink);

        public override bool Equals(object obj) => obj is FactionRelationshipVM vm && Equals(vm);

        public bool Equals(FactionRelationshipVM vm) => EqualityComparer<IFaction>.Default.Equals(Faction, vm.Faction);

        public override int GetHashCode() => -301155118 + EqualityComparer<IFaction>.Default.GetHashCode(Faction);
        [DataSourceProperty] public string NameText
        {
            get => _nameText;
            set
            {
                if (value != _nameText)
                {
                    _nameText = value;
                    OnPropertyChanged(nameof(NameText));
                }
            }
        }
        [DataSourceProperty] public ImageIdentifierVM ImageIdentifier
        {
            get => _imageIdentifier;
            set
            {
                if (value != _imageIdentifier)
                {
                    _imageIdentifier = value;
                    OnPropertyChanged(nameof(ImageIdentifier));
                }
            }
        }
    }
}