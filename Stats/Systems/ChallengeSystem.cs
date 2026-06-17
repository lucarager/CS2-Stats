namespace Stats.Systems {
    using LucaModsCommon.Systems;
    using UnityEngine;

    /// <summary>The lifecycle of a single challenge run.</summary>
    public enum ChallengeState {
        NotStarted,
        Running,
        Paused,
        Finished,
    }

    /// <summary>
    /// Owns the challenge system: a real-time stopwatch that counts up to a fixed
    /// limit, plus the start/pause/restart controls that gate data collection.
    /// </summary>
    public partial class ChallengeSystem : CommonGameSystemBase {
        /// <summary>Hard time limit for a run, in seconds. Hardcoded for now (60 minutes).</summary>
        public const float TIME_LIMIT_SECONDS = 60f * 60f;

        /// <summary>
        /// Time banked from earlier Running segments (everything before the latest pause/resume).
        /// </summary>
        private float m_AccumulatedSeconds;

        /// <summary>
        /// realtimeSinceStartup captured when the current Running segment began.
        /// </summary>
        private float m_SegmentStartRealtime;

        /// <summary>
        /// Run id backing field
        /// </summary>
        private int m_RunId;

        /// <summary>Current lifecycle state of the run.</summary>
        public ChallengeState State { get; private set; } = ChallengeState.NotStarted;

        /// <summary>
        /// Increments each time a fresh run begins. Data systems compare this against the last value
        /// they saw to know when to reset their accumulated data.
        /// </summary>
        public int RunId => m_RunId;

        /// <summary>Seconds elapsed in the current run, clamped to <see cref="TIME_LIMIT_SECONDS"/>.</summary>
        public float ElapsedSeconds {
            get {
                var elapsed = m_AccumulatedSeconds;
                if (State == ChallengeState.Running) {
                    elapsed += UnityEngine.Time.realtimeSinceStartup - m_SegmentStartRealtime;
                }

                return Mathf.Min(elapsed, TIME_LIMIT_SECONDS);
            }
        }

        /// <summary>
        /// True while a run is active and still within the time limit. 
        /// </summary>
        public bool IsCollecting => State == ChallengeState.Running && ElapsedSeconds < TIME_LIMIT_SECONDS;

        /// <summary>Begins a fresh run: zeroes the clock, bumps <see cref="RunId"/>, and starts collecting.</summary>
        public void StartChallenge() {
            m_AccumulatedSeconds   = 0f;
            m_SegmentStartRealtime = UnityEngine.Time.realtimeSinceStartup;
            State                  = ChallengeState.Running;
            m_RunId++;
            m_Log.Debug($"Challenge started (run {m_RunId})");
        }

        /// <summary>Resets everything and starts over — identical to a fresh <see cref="StartChallenge"/>.</summary>
        public void Restart() {
            m_Log.Debug("Challenge restart requested");
            StartChallenge();
        }

        /// <summary>Toggles between Running and Paused, banking elapsed time on pause. No-op when finished.</summary>
        public void TogglePause() {
            switch (State) {
                case ChallengeState.Running:
                    m_AccumulatedSeconds = ElapsedSeconds; // bank progress before the clock stops
                    State                = ChallengeState.Paused;
                    m_Log.Debug("Challenge paused");
                    break;
                case ChallengeState.Paused:
                    m_SegmentStartRealtime = UnityEngine.Time.realtimeSinceStartup;
                    State                  = ChallengeState.Running;
                    m_Log.Debug("Challenge resumed");
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnUpdate() {
            // Latch the run as finished once the limit is reached. 
            if (State == ChallengeState.Running && ElapsedSeconds >= TIME_LIMIT_SECONDS) {
                m_AccumulatedSeconds = TIME_LIMIT_SECONDS;
                State                = ChallengeState.Finished;
                m_Log.Debug("Challenge finished (time limit reached)");
            }
        }
    }
}
