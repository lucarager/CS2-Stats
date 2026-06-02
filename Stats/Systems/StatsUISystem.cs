namespace Stats.Systems {
    using LucaModsCommon.Extensions; // ValueBindingHelper
    using LucaModsCommon.Systems;    // CommonUISystemBase

    /// <summary>
    /// Sample UI system on the shared <see cref="CommonUISystemBase"/>. Demonstrates a full
    /// C# &lt;-&gt; TypeScript round-trip: each <c>CreateBinding(key, initial, cb)</c> publishes a value
    /// binding <c>BINDING:{key}</c> (C# -&gt; UI) and a trigger <c>TRIGGER:{key}</c> (UI -&gt; C#),
    /// which line up with the shared TS <c>TwoWayBinding&lt;T&gt;(key)</c> in <c>gameBindings.ts</c>.
    /// </summary>
    public partial class StatsUISystem : CommonUISystemBase {
        protected override string ModId => Mod.Instance.Id;

        private ValueBindingHelper<int> m_BikersCountMax;
        private ValueBindingHelper<int> m_BikersCountLast;
        private ValueBindingHelper<bool> m_PanelOpen;
        private StatsSystem m_StatsSystem;

        protected override void OnCreate() {
            base.OnCreate(); // sets the inherited m_Log

            m_StatsSystem = World.GetOrCreateSystemManaged<StatsSystem>();

            m_BikersCountMax = CreateBinding("BIKERS_COUNT_MAX", 0);
            m_BikersCountLast = CreateBinding("BIKERS_COUNT_LAST", 0);
            m_PanelOpen = CreateBinding("PANEL_OPEN", false, value => {
                m_Log.Debug($"PANEL_OPEN set from UI: {value}");
            });
        }

        protected override void OnUpdate() {
            if (!m_StatsSystem.EnableStats) {
                return;
            }

            m_BikersCountMax.Value = m_StatsSystem.BikersCountMax;
            m_BikersCountLast.Value = m_StatsSystem.BikersCountLast;
        }
    }
}
