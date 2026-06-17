import { trigger } from "cs2/api";
import mod from "mod.json";
import { TwoWayBinding } from "utils/bidirectionalBinding";

export interface StatAggregate {
    Key: string;
    Current: number;
    Max: number;
    Min: number;
    Mean: number;
    /** When true this stat is a monotonic counter: render Current as a single total. */
    IsCounter: boolean;
}

/** Lifecycle of a challenge run — mirrors the C# {@link ChallengeState} enum names. */
export type ChallengeState = "NotStarted" | "Running" | "Paused" | "Finished";

/** Fires a payload-less C# trigger (created via CreateTrigger(key, Action)). */
const fireTrigger = (id: string) => trigger(mod.id, `TRIGGER:${id}`);

export const GAME_BINDINGS = {
    STATS_DATA: new TwoWayBinding<StatAggregate[]>("STATS_DATA", []),
    PANEL_OPEN: new TwoWayBinding<boolean>("PANEL_OPEN", false),

    // Challenge run state (read-only bindings pushed from C#).
    CHALLENGE_STATE: new TwoWayBinding<ChallengeState>("CHALLENGE_STATE", "NotStarted"),
    /** Elapsed seconds in the current run (whole seconds). */
    CHALLENGE_ELAPSED: new TwoWayBinding<number>("CHALLENGE_ELAPSED", 0),
    /** Run time limit in seconds (constant). */
    CHALLENGE_LIMIT: new TwoWayBinding<number>("CHALLENGE_LIMIT", 0),

    // Challenge controls (payload-less triggers).
    CHALLENGE_START: () => fireTrigger("CHALLENGE_START"),
    CHALLENGE_TOGGLE_PAUSE: () => fireTrigger("CHALLENGE_TOGGLE_PAUSE"),
    CHALLENGE_RESTART: () => fireTrigger("CHALLENGE_RESTART"),
};
