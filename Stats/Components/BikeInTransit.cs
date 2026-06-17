namespace Stats.Components {
    using Unity.Entities;

    /// <summary>
    /// Mod-owned tag placed on a bicycle while it is actively making a trip (i.e. <c>Moving</c>).
    /// Used by <see cref="Stats.Systems.BikeTripsSystem"/> to:
    /// <list type="bullet">
    ///   <item>recognise that a terminating bike was a genuine in-progress trip — distinguishing it
    ///   from idle bikes that spawn already parked (e.g. a household's owned bicycle); and</item>
    ///   <item>tally each despawning trip exactly once across the 1–2 frames a <c>Deleted</c> entity
    ///   stays alive before the cleanup pipeline destroys it.</item>
    /// </list>
    /// </summary>
    public struct BikeInTransit : IComponentData { }
}
