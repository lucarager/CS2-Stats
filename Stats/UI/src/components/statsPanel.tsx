import { useValue } from "cs2/api";
import { useLocalization } from "cs2/l10n";
import { GAME_BINDINGS, type StatAggregate } from "gameBindings";
import styles from "./statsPanel.module.scss";

const StatRow = ({ label, value }: { label: string; value: string | number }) => (
    <div className={styles.statRow}>
        <span className={styles.statRow__label}>{label}</span>
        <span className={styles.statRow__value}>{value}</span>
    </div>
);

const StatGroup = ({ stat }: { stat: StatAggregate }) => {
    const { translate } = useLocalization();
    const name = translate(`Stats.Stat[${stat.Key}]`, stat.Key);

    return (
        <div className={styles.statGroup}>
            <div className={styles.statGroup__heading}>{name}</div>
            <div className={styles.statGroup__values}>
                <StatRow label="Current" value={stat.Current} />
                <StatRow label="Max" value={stat.Max} />
                <StatRow label="Avg" value={stat.Mean.toFixed(1)} />
            </div>
        </div>
    );
};

export const StatsPanel = () => {
    const statsData = useValue(GAME_BINDINGS.STATS_DATA.binding);
    const open = useValue(GAME_BINDINGS.PANEL_OPEN.binding);

    if (!open) return null;

    return (
        <div className={styles.statsPanel}>            
            {statsData.map((stat) => (
                <StatGroup key={stat.Key} stat={stat} />
            ))}
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
