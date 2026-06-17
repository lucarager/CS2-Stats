namespace Stats.Systems {
    using Game.Input;
    using LucaModsCommon.Extensions;
    using LucaModsCommon.Systems;
    using Stats.Models;

    public partial class StatsUISystem : CommonUISystemBase {
        private ValueBindingHelper<bool> m_PanelOpen;

        private            ValueBindingHelper<StatAggregate[]> m_StatsData;
        private            ValueBindingHelper<string>          m_ChallengeState;
        private            ValueBindingHelper<int>             m_ChallengeElapsed;
        private            StatsSystem                         m_StatsSystem;
        private            BikeTripsSystem                     m_BikeTripsSystem;
        private            ChallengeSystem                     m_ChallengeSystem;
        private            ProxyAction                         m_TogglePanelAction;
        protected override string                              ModId => Mod.Instance.Id;

        protected override void OnCreate() {
            base.OnCreate();
            m_StatsSystem     = World.GetOrCreateSystemManaged<StatsSystem>();
            m_BikeTripsSystem = World.GetOrCreateSystemManaged<BikeTripsSystem>();
            m_ChallengeSystem = World.GetOrCreateSystemManaged<ChallengeSystem>();

            m_StatsData = CreateBinding("STATS_DATA", System.Array.Empty<StatAggregate>());
            m_PanelOpen = CreateBinding("PANEL_OPEN",
                                       false,
                                       value => { m_Log.Debug($"PANEL_OPEN set from UI: {value}"); });
            m_ChallengeState   = CreateBinding("CHALLENGE_STATE", m_ChallengeSystem.State.ToString());
            m_ChallengeElapsed = CreateBinding("CHALLENGE_ELAPSED", 0);
            CreateBinding("CHALLENGE_LIMIT", (int)ChallengeSystem.TIME_LIMIT_SECONDS);

            CreateTrigger("CHALLENGE_START", m_ChallengeSystem.StartChallenge);
            CreateTrigger("CHALLENGE_TOGGLE_PAUSE", m_ChallengeSystem.TogglePause);
            CreateTrigger("CHALLENGE_RESTART", m_ChallengeSystem.Restart);

            m_TogglePanelAction                 = ((Setting)Mod.Instance.Settings).GetAction(Setting.TOGGLE_PANEL_STR);
            m_TogglePanelAction.shouldBeEnabled = true;
        }

        protected override void OnDestroy() {
            m_TogglePanelAction.shouldBeEnabled = false;
            base.OnDestroy();
        }

        protected override void OnUpdate() {
            // The toggle hotkey must always be polled, even while the panel is closed.
            if (m_TogglePanelAction.WasPerformedThisFrame()) {
                m_PanelOpen.Value = !m_PanelOpen.Value;
            }

            // Nothing is rendered while the panel is closed, so skip the per-frame
            // aggregation, allocation, and binding pushes entirely.
            if (!m_PanelOpen.Value) {
                return;
            }

            var gauges   = m_StatsSystem.GetAggregates();
            var combined = new StatAggregate[gauges.Length + 1];
            System.Array.Copy(gauges, combined, gauges.Length);
            combined[gauges.Length] = new StatAggregate("BIKE_TRIPS") {
                Current   = m_BikeTripsSystem.TotalBikeTrips,
                IsCounter = true,
            };
            m_StatsData.Value = combined;
            m_ChallengeState.Value   = m_ChallengeSystem.State.ToString();
            m_ChallengeElapsed.Value = (int)m_ChallengeSystem.ElapsedSeconds;
        }
    }
}