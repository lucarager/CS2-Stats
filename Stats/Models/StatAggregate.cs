namespace Stats.Models {
    /// <summary>
    /// Running aggregation for a single tracked statistic. Public fields are serialized
    /// to the UI via <see cref="LucaModsCommon.Extensions.GenericUIWriter{T}"/>.
    /// </summary>
    public class StatAggregate {
        /// <summary>L10n lookup key (e.g. <c>"BIKERS"</c>). Matches an entry in <see cref="LocaleEn"/>.</summary>
        public string Key;

        /// <summary>Most recent sampled value.</summary>
        public int Current;

        /// <summary>Highest value observed since tracking began.</summary>
        public int Max;

        /// <summary>Lowest value observed since tracking began.</summary>
        public int Min;

        /// <summary>Running arithmetic mean (Welford's online algorithm).</summary>
        public double Mean;

        /// <summary>
        /// When <c>true</c>, this stat is a monotonic session counter (e.g. completed trips) rather
        /// than a sampled gauge: only <see cref="Current"/> is meaningful and the UI renders it as a
        /// single total. <see cref="Max"/>, <see cref="Min"/> and <see cref="Mean"/> are left unset.
        /// </summary>
        public bool IsCounter;

        private long m_SampleCount;

        /// <summary>Initializes a new instance of the <see cref="StatAggregate"/> class.</summary>
        public StatAggregate() { }

        /// <summary>Initializes a new instance of the <see cref="StatAggregate"/> class.</summary>
        /// <param name="key">L10n lookup key that identifies this stat.</param>
        public StatAggregate(string key) {
            Key = key;
        }

        /// <summary>Clears all accumulated state so the aggregate restarts from a clean slate.</summary>
        public void Reset() {
            Current       = 0;
            Max           = 0;
            Min           = 0;
            Mean          = 0;
            m_SampleCount = 0;
        }

        /// <summary>
        /// Records a new sample, updating <see cref="Current"/>, <see cref="Max"/>,
        /// <see cref="Min"/>, and <see cref="Mean"/>.
        /// </summary>
        /// <param name="value">The sampled value (typically an entity count).</param>
        public void Update(int value) {
            Current = value;
            if (value > Max) Max = value;
            if (m_SampleCount == 0 || value < Min) Min = value;
            m_SampleCount++;
            Mean += (value - Mean) / m_SampleCount;
        }
    }
}
