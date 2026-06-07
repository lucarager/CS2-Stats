namespace Stats.Systems {
    using Game.Input;
    using LucaModsCommon.Extensions;
    using LucaModsCommon.Systems;
    using Stats.Models;

    public partial class StatsUISystem : CommonUISystemBase {
        private ValueBindingHelper<bool> m_PanelOpen;

        private            ValueBindingHelper<StatAggregate[]> m_StatsData;
        private            StatsSystem                         m_StatsSystem;
        private            ProxyAction                         m_TogglePanelAction;
        protected override string                              ModId => Mod.Instance.Id;

        protected override void OnCreate() {
            base.OnCreate();
            m_StatsSystem = World.GetOrCreateSystemManaged<StatsSystem>();
            m_StatsData   = CreateBinding("STATS_DATA", System.Array.Empty<StatAggregate>());
            m_PanelOpen = CreateBinding("PANEL_OPEN",
                                       false,
                                       value => { m_Log.Debug($"PANEL_OPEN set from UI: {value}"); });

            m_TogglePanelAction                 = ((Setting)Mod.Instance.Settings).GetAction(Setting.TOGGLE_PANEL_STR);
            m_TogglePanelAction.shouldBeEnabled = true;
        }

        protected override void OnDestroy() {
            m_TogglePanelAction.shouldBeEnabled = false;
            base.OnDestroy();
        }

        protected override void OnUpdate() {
            m_StatsData.Value = m_StatsSystem.GetAggregates();

            if (m_TogglePanelAction.WasPerformedThisFrame()) {
                m_PanelOpen.Value = !m_PanelOpen.Value;
            }
        }
    }
}