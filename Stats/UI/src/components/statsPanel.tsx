import { useValue } from "cs2/api";
import { c } from "utils/classes";
import { GAME_BINDINGS } from "gameBindings";
import styles from "./statsPanel.module.scss";

export const StatsPanel = () => {
    const bikersCountMax = useValue(GAME_BINDINGS.BIKERS_COUNT_MAX.binding);
    const bikersCountLast = useValue(GAME_BINDINGS.BIKERS_COUNT_LAST.binding);
    const open = useValue(GAME_BINDINGS.PANEL_OPEN.binding);

    if (!open) return null;

    return (
        <div className={styles.statsPanel}>
            <div>Max Bikers: {bikersCountMax}</div>
            <div>Current Bikers: {bikersCountLast}</div>

            <button
                className={styles.closeButton}
                onClick={() => GAME_BINDINGS.PANEL_OPEN.set(!open)}
            >
                <img
                    src={"coui://uil/Standard/XClose.svg"}
                    className={styles.closeButton__icon}
                />
            </button>
        </div>
    );
};
