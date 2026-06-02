import { TwoWayBinding } from "utils/bidirectionalBinding";

// Shared two-way bindings (C# <-> UI). Each key matches a CreateBinding(key, ...) call in
// StatsUISystem: read with `useValue(BINDING.binding)`, push back with `BINDING.set(value)`.
export const GAME_BINDINGS = {
    COUNTER: new TwoWayBinding<number>("COUNTER", 0),
    PANEL_OPEN: new TwoWayBinding<boolean>("PANEL_OPEN", false),
};
