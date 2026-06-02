namespace Stats.Systems {
    using LucaModsCommon.Extensions;
    using LucaModsCommon.Utils;

    /// <summary>
    /// Sample UI system on the shared <see cref="ExtendedUISystemBase"/>. Demonstrates a full
    /// C# &lt;-&gt; TypeScript round-trip: each <c>CreateBinding(key, initial, cb)</c> publishes a value
    /// binding <c>BINDING:{key}</c> (C# -&gt; UI) and a trigger <c>TRIGGER:{key}</c> (UI -&gt; C#),
    /// which line up with the shared TS <c>TwoWayBinding&lt;T&gt;(key)</c> in <c>gameBindings.ts</c>.
    /// </summary>
    public partial class StatsUISystem : ExtendedUISystemBase {
        protected override string ModId => Mod.Instance.Id;

        private PrefixedLogger m_Log;
        private ValueBindingHelper<int> m_Counter;
        private ValueBindingHelper<bool> m_PanelOpen;

        protected override void OnCreate() {
            base.OnCreate();
            m_Log = new PrefixedLogger(nameof(StatsUISystem));

            // int counter: UI reads it and can push new values back via .set(...)
            m_Counter = CreateBinding("COUNTER", 0, value => {
                m_Log.Debug($"COUNTER set from UI: {value}");
            });

            // bool flag: same two-way pattern
            m_PanelOpen = CreateBinding("PANEL_OPEN", false, value => {
                m_Log.Debug($"PANEL_OPEN set from UI: {value}");
            });
        }
    }
}
