using JetBrains.Annotations;
using LOTRAOM.Momentum.ViewModel;
using SandBox.View.Map;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

namespace LOTRAOM.Momentum.Views
{
    public class MomentumIndicator : MapView
    {
        private MomentumIndicatorVM _dataSource = null!;
        private GauntletLayer _layerAsGauntletLayer = null!;

        protected override void OnMapScreenUpdate(float dt)
        {
            base.OnMapScreenUpdate(dt);
            _dataSource.UpdateBanners();
        }

        protected override void CreateLayout()
        {
            base.CreateLayout();
            _dataSource = new MomentumIndicatorVM();
            Layer = new GauntletLayer(100);
            _layerAsGauntletLayer = (Layer as GauntletLayer)!;
            _layerAsGauntletLayer!.LoadMovie("MomentumMapIndicator", _dataSource);
            Layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.MouseButtons | InputUsageMask.Keyboardkeys);
            MapScreen.AddLayer(Layer);
        }
    }
}