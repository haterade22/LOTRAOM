using JetBrains.Annotations;
using LOTRAOM.BalanceOfPower.ViewModel;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.ScreenSystem;

namespace LOTRAOM.BalanceOfPower.Views
{
    internal class DetailWarViewInterface : GenericInterface
    {
        protected override string MovieName => "DetailedBalanceOfPowerView";

        public void ShowInterface(ScreenBase screenBase)
        {
            _screenBase = screenBase;

            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var resourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, resourceDepot);
            spriteData.SpriteCategories["ui_kingdom"].Load(resourceContext, resourceDepot);

            _layer = new GauntletLayer(209);
            _layer.InputRestrictions.SetInputRestrictions();
            _layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
            _layer.IsFocusLayer = true;
            ScreenManager.TrySetFocus(_layer);
            screenBase.AddLayer(_layer);
            _vm = new DetailBalanceOfPowerVM(OnFinalize);
            _movie = LoadMovie();
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
        }
    }
}