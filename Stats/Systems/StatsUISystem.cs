namespace Stats.Systems {
    using LucaModsCommon.Extensions;
    using LucaModsCommon.Systems;
    using Stats.Models;

    /// <summary>
    /// Bridges <see cref="StatsSystem"/> aggregates to the React UI. Publishes a single
    /// <c>STATS_DATA</c> binding (an array of <see cref="StatAggregate"/>) and a
    /// <c>PANEL_OPEN</c> two-way binding for panel visibility.
    /// </summary>
    public partial class StatsUISystem : CommonUISystemBase {
        /// <inheritdoc/>
        protected override string ModId => Mod.Instance.Id;

        private ValueBindingHelper<StatAggregate[]> _statsData;
        private ValueBindingHelper<bool> _panelOpen;
        private StatsSystem _statsSystem;

        /// <inheritdoc/>
        protected override void OnCreate() {
            base.OnCreate();
            _statsSystem = World.GetOrCreateSystemManaged<StatsSystem>();
            _statsData = CreateBinding("STATS_DATA", System.Array.Empty<StatAggregate>());
            _panelOpen = CreateBinding("PANEL_OPEN", false, value => {
                m_Log.Debug($"PANEL_OPEN set from UI: {value}");
            });
        }

        /// <inheritdoc/>
        protected override void OnUpdate() {
            _statsData.Value = _statsSystem.GetAggregates();
        }
    }
}
