namespace Stats.Systems {
    using System.Collections.Generic;
    using System.Linq;
    using Game.Citizens;
    using Game.Common;
    using Game.Objects;
    using Game.Tools;
    using Game.Vehicles;
    using LucaModsCommon.Systems;
    using Stats.Models;
    using Unity.Entities;

    /// <summary>
    /// Gathers statistics by sampling ECS entity queries at a fixed interval. 
    /// </summary>
    public partial class StatsSystem : CommonGameSystemBase {
        private const int FrameInterval = 60;

        private readonly List<StatRegistration> m_Stats = new();
        private int             m_FrameCount;
        private ChallengeSystem m_ChallengeSystem;
        private int             m_LastRunId;

        /// <summary>Returns a snapshot of all current aggregates for UI consumption.</summary>
        public StatAggregate[] GetAggregates() => m_Stats.Select(s => s.Aggregate).ToArray();

        /// <inheritdoc/>
        protected override void OnCreate() {
            base.OnCreate();

            m_ChallengeSystem = World.GetOrCreateSystemManaged<ChallengeSystem>();

            Register("BIKERS", SystemAPI.QueryBuilder()
                .WithAll<Bicycle, Moving>()
                .WithNone<Temp, Deleted, ParkedCar, Unspawned, Placeholder>()
                .Build());

            Register("BIKE_OWNERS", SystemAPI.QueryBuilder()
                .WithAll<BicycleOwner>()
                .WithNone<Temp, Deleted>()
                .Build());
        }

        /// <inheritdoc/>
        protected override void OnUpdate() {
            // A new run zeroes every aggregate so min/max/mean restart for the challenge window.
            if (m_ChallengeSystem.RunId != m_LastRunId) {
                foreach (var stat in m_Stats) stat.Aggregate.Reset();
                m_LastRunId = m_ChallengeSystem.RunId;
            }

            // Only sample while a run is actively collecting; otherwise the aggregates stay frozen.
            if (!m_ChallengeSystem.IsCollecting) return;

            if (++m_FrameCount % FrameInterval != 0) return;

            foreach (var stat in m_Stats) {
                stat.Aggregate.Update(stat.Query.CalculateEntityCount());
            }
        }

        /// <summary>Registers a new statistic to be sampled each tick.</summary>
        /// <param name="key">L10n key (must have a matching entry in <see cref="LocaleEn"/>).</param>
        /// <param name="query">ECS query whose entity count is the sampled value.</param>
        private void Register(string key, EntityQuery query) {
            m_Stats.Add(new StatRegistration(key, query));
        }

        /// <summary>Pairs an <see cref="EntityQuery"/> with its running <see cref="StatAggregate"/>.</summary>
        private class StatRegistration {
            public StatAggregate Aggregate { get; }
            public EntityQuery Query { get; }

            public StatRegistration(string key, EntityQuery query) {
                Aggregate = new StatAggregate(key);
                Query = query;
            }
        }
    }
}
