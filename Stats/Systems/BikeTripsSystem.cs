namespace Stats.Systems {
    using Game.Common;
    using Game.Objects;
    using Game.Tools;
    using Game.Vehicles;
    using LucaModsCommon.Systems;
    using Stats.Components;
    using Unity.Collections;
    using Unity.Entities;

    /// <summary>
    /// Counts unique bicycle trips for the current challenge run (not persisted across saves).
    ///
    /// <para>A bike is a <see cref="Game.Vehicles.PersonalCar"/> carrying an empty <see cref="Bicycle"/>
    /// tag; the base game runs both through <c>PersonalCarAISystem</c>.</para>
    ///
    /// <para>The <see cref="BikeInTransit"/> tag provides the once-per-trip guarantee.</para>
    /// </summary>
    public partial class BikeTripsSystem : CommonGameSystemBase {
        /// Bikes that are in transit and which we have not started tracking yet. 
        private EntityQuery     m_RegisterQuery;

        /// Tracked bikes that have since parked or despawned
        private EntityQuery     m_LeftTransitQuery;
        
        /// Every currently-tracked bike 
        private EntityQuery     m_TaggedQuery;
        
        private int             m_TotalBikeTrips;
        
        private ChallengeSystem m_ChallengeSystem;
        
        private int             m_LastRunId;

        /// <summary>Total bike trips counted for the current run.</summary>
        public int TotalBikeTrips => m_TotalBikeTrips;

        /// <inheritdoc/>
        protected override void OnCreate() {
            base.OnCreate();

            m_ChallengeSystem = World.GetOrCreateSystemManaged<ChallengeSystem>();

            m_RegisterQuery = SystemAPI.QueryBuilder()
                .WithAll<Bicycle, Moving, Game.Vehicles.PersonalCar>()
                .WithNone<BikeInTransit, ParkedCar, Deleted, Temp, Unspawned>()
                .Build();

            m_LeftTransitQuery = SystemAPI.QueryBuilder()
                .WithAll<Bicycle, BikeInTransit>()
                .WithAny<ParkedCar, Deleted>()
                .Build();

            m_TaggedQuery = SystemAPI.QueryBuilder()
                .WithAll<BikeInTransit>()
                .Build();
        }

        /// <inheritdoc/>
        protected override void OnUpdate() {
            // A new run resets the count and clears every tracking tag
            if (m_ChallengeSystem.RunId != m_LastRunId) {
                Reset();
            }

            // Count nothing while not collecting 
            if (!m_ChallengeSystem.IsCollecting) {
                return;
            }

            CountNewTrips();
            TagBikesInTransit();
        }

        /// <summary>
        /// Reset the count and clears every tracking tag.
        /// </summary>
        private void Reset() {
            m_TotalBikeTrips = 0;
            m_LastRunId = m_ChallengeSystem.RunId;
            EntityManager.RemoveComponent<BikeInTransit>(m_TaggedQuery);
        }

        /// <summary>
        /// Adds each newly-seen, non-dummy bike to the running total. Must run before the bikes are
        /// tagged, since tagging empties <see cref="m_RegisterQuery"/>.
        /// </summary>
        private void CountNewTrips() {
            if (m_RegisterQuery.IsEmptyIgnoreFilter) {
                return;
            }

            using var personalCars = m_RegisterQuery.ToComponentDataArray<Game.Vehicles.PersonalCar>(Allocator.Temp);

            for (var i = 0; i < personalCars.Length; i++) {
                // Background "dummy" traffic is not a real citizen trip.
                if ((personalCars[i].m_State & PersonalCarFlags.DummyTraffic) != 0) {
                    continue;
                }

                m_TotalBikeTrips++;
            }
        }

        /// <summary>
        /// Tag every bike now in transit (including dummy traffic) so none are re-examined 
        /// </summary>
        private void TagBikesInTransit() {
            EntityManager.AddComponent<BikeInTransit>(m_RegisterQuery);
            EntityManager.RemoveComponent<BikeInTransit>(m_LeftTransitQuery);
        }
    }
}
