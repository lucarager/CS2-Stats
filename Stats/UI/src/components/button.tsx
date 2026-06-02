import { useValue } from "cs2/api";
import { c } from "utils/classes";
import { GAME_BINDINGS } from "gameBindings";
import { Button } from "cs2/ui";

export const ModButton = () => {
    const bikersCountMax = useValue(GAME_BINDINGS.BIKERS_COUNT_MAX.binding);
    const bikersCountLast = useValue(GAME_BINDINGS.BIKERS_COUNT_LAST.binding);
    const open = useValue(GAME_BINDINGS.PANEL_OPEN.binding);

    return (
        <Button
            src={"coui://uil/Standard/XClose.svg"}
            variant="floating"
            onSelect={() => GAME_BINDINGS.PANEL_OPEN.set(!open)}
        />
    );
};
