import { TwoWayBinding } from "utils/bidirectionalBinding";

export interface StatAggregate {
    Key: string;
    Current: number;
    Max: number;
    Min: number;
    Mean: number;
}

export const GAME_BINDINGS = {
    STATS_DATA: new TwoWayBinding<StatAggregate[]>("STATS_DATA", []),
    PANEL_OPEN: new TwoWayBinding<boolean>("PANEL_OPEN", false),
};
