namespace Stats.Systems {
    using Colossal.Collections;
    using Game.Common;
    using Game.Objects;
    using Game.Tools;
    using Game.Vehicles;
    using LucaModsCommon.Systems;
    using Unity.Collections;
    using Unity.Entities;

    public partial class StatsSystem : CommonGameSystemBase {
        private const int FRAME_INTERVAL = 60;

        // Getters
        public int BikersCountMax => m_BikersCountMax.value;
        public int BikersCountLast => m_BikersCountLast.value;

        public bool EnableStats = true;

        // Containers
        private NativeValue<int> m_BikersCountMax;
        private NativeValue<int> m_BikersCountLast;
        // Queries
        private EntityQuery m_BikersQuery;
        // Frame counter
        private int m_FrameCount;

        protected override void OnCreate() {
            base.OnCreate();

            // Initialize Containers
            m_BikersCountMax  = new NativeValue<int>(0, Allocator.Persistent);
            m_BikersCountLast = new NativeValue<int>(0, Allocator.Persistent);
            m_FrameCount = 0;

            // Initialize Queries
            m_BikersQuery = SystemAPI.QueryBuilder()
                                     .WithAll<Bicycle, Moving>()
                                     .WithNone<Temp, Deleted, ParkedCar, Unspawned, Placeholder>()
                                     .Build();

        }

        protected override void OnDestroy() {
            base.OnDestroy();

            // Dispose Containers
            m_BikersCountMax.Dispose();
            m_BikersCountLast.Dispose();
        }

        protected override void OnUpdate() {
            // Don't run if disabled
            if (!EnableStats) return;

            // Don't do this every frame
            if (m_FrameCount++ % FRAME_INTERVAL != 0) return;

            // Update counts
            var bikersCount = m_BikersQuery.CalculateEntityCount();
            if (bikersCount > m_BikersCountMax.value) {
                m_BikersCountMax.value = bikersCount;
            }
            m_BikersCountLast.value = bikersCount;
        }
    }
}
