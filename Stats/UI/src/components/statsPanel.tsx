import { Button } from "cs2/ui";
import { useValue } from "cs2/api";
import { GAME_BINDINGS } from "gameBindings";
import styles from "./statsPanel.module.scss";

const StatRow = ({
    label,
    value,
}: {
    label: string;
    value: string | number;
}) => (
    <div className={styles.statRow}>
        <span className={styles.statRow__label}>{label}</span>
        <span className={styles.statRow__value}>{value}</span>
    </div>
);

/** Formats whole seconds as mm:ss. */
const formatTime = (totalSeconds: number) => {
    const s = Math.max(0, Math.floor(totalSeconds));
    const mm = Math.floor(s / 60)
        .toString()
        .padStart(2, "0");
    const ss = (s % 60).toString().padStart(2, "0");
    return `${mm}:${ss}`;
};

export const StatsPanel = () => {
    const statsData = useValue(GAME_BINDINGS.STATS_DATA.binding);
    const open = useValue(GAME_BINDINGS.PANEL_OPEN.binding);
    const state = useValue(GAME_BINDINGS.CHALLENGE_STATE.binding);
    const elapsed = useValue(GAME_BINDINGS.CHALLENGE_ELAPSED.binding);
    const limit = useValue(GAME_BINDINGS.CHALLENGE_LIMIT.binding);

    if (!open) return null;

    const started = state !== "NotStarted";
    const running = state === "Running";
    const finished = state === "Finished";

    const statByKey = (key: string) =>
        statsData.find((stat) => stat.Key === key);
    const bikers = statByKey("BIKERS");
    const bikeOwners = statByKey("BIKE_OWNERS");
    const bikeTrips = statByKey("BIKE_TRIPS");

    return (
        <div className={styles.statsPanel}>
            <div className={styles.statsPanel__header}>
                <div>
                    <img className={styles.logo} src="coui://stats/cpplogo.svg" />
                    <span>CPP One Hour Bike Challenge</span>
                </div>
                <Button
                className={styles.iconButton}
                variant="icon"
                src="coui://stats/close.svg"
                tooltipLabel="Close Panel"
                onSelect={() => GAME_BINDINGS.PANEL_OPEN.set(!open)}
            />
            </div>
            {!started ? (
                <Button
                    className={styles.startButton}
                    variant="primary"
                    onSelect={() => GAME_BINDINGS.CHALLENGE_START()}
                >
                    Start challenge
                </Button>
            ) : (
                <>
                    <div className={styles.timerRow}>
                        <span className={styles.timerRow__time}>
                            {formatTime(elapsed)}
                            <span className={styles.timerRow__limit}>
                                {" "}
                                / {formatTime(limit)}
                            </span>
                        </span>
                        <div className={styles.timerRow__controls}>
                            <Button
                                className={styles.iconButton}
                                disabled={finished}
                                variant="icon"
                                src={
                                    running
                                        ? "coui://stats/pause.svg"
                                        : "coui://stats/play.svg"
                                }
                                tooltipLabel="Pause Challenge"
                                onSelect={() =>
                                    GAME_BINDINGS.CHALLENGE_TOGGLE_PAUSE()
                                }
                            />
                            <Button
                                className={styles.iconButton}
                                variant="icon"
                                src={"coui://stats/restart.svg"}
                                tooltipLabel="Restart Challenge"
                                onSelect={() =>
                                    GAME_BINDINGS.CHALLENGE_RESTART()
                                }
                            />
                        </div>
                    </div>
                    <StatRow label="Trips" value={bikeTrips?.Current ?? 0} />
                    <StatRow
                        label="Current active bikers"
                        value={bikers?.Current ?? 0}
                    />
                    <StatRow
                        label="Avg. active bikers"
                        value={(bikers?.Mean ?? 0).toFixed(1)}
                    />
                </>
            )}            
        </div>
    );
};
