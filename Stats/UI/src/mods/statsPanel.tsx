import { useValue } from "cs2/api";
import { c } from "utils/classes";
import { GAME_BINDINGS } from "gameBindings";

/**
 * Dummy panel demonstrating the end-to-end binding round-trip:
 *  - `useValue(...binding)` reads the current C# value (and re-renders on change),
 *  - `...set(value)` fires TRIGGER:<key>, which StatsUISystem handles and writes back to the binding.
 */
export const StatsPanel = () => {
    const counter = useValue(GAME_BINDINGS.COUNTER.binding);
    const open = useValue(GAME_BINDINGS.PANEL_OPEN.binding);

    return (
        <div className={c("stats-panel")}>
            <div>Counter: {counter}</div>
            <button onClick={() => GAME_BINDINGS.COUNTER.set(counter + 1)}>+1</button>

            <div>Panel open: {open ? "yes" : "no"}</div>
            <button onClick={() => GAME_BINDINGS.PANEL_OPEN.set(!open)}>Toggle</button>
        </div>
    );
};
