namespace Stats.Systems {
    using System.Collections.Generic;
    using System.Linq;
    using Game.Common;
    using Game.Objects;
    using Game.Tools;
    using Game.Vehicles;
    using LucaModsCommon.Systems;
    using Stats.Models;
    using Unity.Entities;

    /// <summary>
    /// Gathers statistics by sampling ECS entity queries at a fixed interval. New stats are
    /// added via <see cref="Register"/> in <see cref="OnCreate"/>; the generic
    /// <see cref="OnUpdate"/> loop handles aggregation for all registered stats.
    /// </summary>
    public partial class StatsSystem : CommonGameSystemBase {
        private const int FrameInterval = 60;

        private readonly List<StatRegistration> m_Stats = new();
        private int m_FrameCount;

        /// <summary>Returns a snapshot of all current aggregates for UI consumption.</summary>
        public StatAggregate[] GetAggregates() => m_Stats.Select(s => s.Aggregate).ToArray();

        /// <inheritdoc/>
        protected override void OnCreate() {
            base.OnCreate();

            Register("BIKERS", SystemAPI.QueryBuilder()
                .WithAll<Bicycle, Moving>()
                .WithNone<Temp, Deleted, ParkedCar, Unspawned, Placeholder>()
                .Build());
        }

        /// <inheritdoc/>
        protected override void OnUpdate() {
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
