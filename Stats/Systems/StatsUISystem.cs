namespace Stats.Systems {
    using Game.Input;

    using LucaModsCommon.Extensions;
    using LucaModsCommon.Systems;
    using Stats.Models;

    public partial class StatsUISystem : CommonUISystemBase {
        protected override string ModId => Mod.Instance.Id;

        private ValueBindingHelper<StatAggregate[]> _statsData;
        private ValueBindingHelper<bool> _panelOpen;
        private StatsSystem _statsSystem;
        private ProxyAction _togglePanelAction;

        protected override void OnCreate() {
            base.OnCreate();
            _statsSystem = World.GetOrCreateSystemManaged<StatsSystem>();
            _statsData = CreateBinding("STATS_DATA", System.Array.Empty<StatAggregate>());
            _panelOpen = CreateBinding("PANEL_OPEN", false, value => {
                m_Log.Debug($"PANEL_OPEN set from UI: {value}");
            });

            _togglePanelAction = ((Setting)Mod.Instance.Settings).GetAction(Setting.TogglePanelStr);
            _togglePanelAction.shouldBeEnabled = true;
        }

        protected override void OnDestroy() {
            _togglePanelAction.shouldBeEnabled = false;
            base.OnDestroy();
        }

        protected override void OnUpdate() {
            _statsData.Value = _statsSystem.GetAggregates();

            if (_togglePanelAction.WasPerformedThisFrame()) {
                _panelOpen.Value = !_panelOpen.Value;
            }
        }
    }
}
