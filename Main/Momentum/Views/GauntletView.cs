using LOTRAOM.Momentum.ViewModel;
using SandBox.View.Map;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;

namespace LOTRAOM.Momentum.Views
{
    public class MomentumIndicator : MapView
    {
        private MomentumIndicatorVM _dataSource = null!;
        private GauntletLayer _layerAsGauntletLayer = null!;
        protected override void OnMapScreenUpdate(float dt)
        {
            base.OnMapScreenUpdate(dt);
        }
        protected override void OnResume()
        {
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var resourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, resourceDepot);
            spriteData.SpriteCategories["ui_kingdom"].Load(resourceContext, resourceDepot);
            base.OnResume();
        }
        protected override void CreateLayout()
        {
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var resourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, resourceDepot);
            spriteData.SpriteCategories["ui_kingdom"].Load(resourceContext, resourceDepot);
            base.CreateLayout();
            _dataSource = new MomentumIndicatorVM();
            Layer = new GauntletLayer(100);
            _layerAsGauntletLayer = (Layer as GauntletLayer)!;
            _layerAsGauntletLayer!.LoadMovie("MomentumMapIndicator", _dataSource);
            Layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.MouseButtons | InputUsageMask.Keyboardkeys);
            MapScreen.AddLayer(Layer);
        }
        protected override void OnFinalize()
        {
            if (_layerAsGauntletLayer != null)
            {
                MapScreen.Instance.RemoveLayer(_layerAsGauntletLayer);
                _layerAsGauntletLayer = null;
            }
            var spriteData = UIResourceManager.SpriteData;
            spriteData.SpriteCategories["ui_encyclopedia"].Unload();
            spriteData.SpriteCategories["ui_kingdom"].Unload();
            base.OnFinalize();
        }
    }
}