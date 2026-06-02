import { useValue } from "cs2/api";
import { c } from "utils/classes";
import { GAME_BINDINGS } from "gameBindings";
import { Button, Tooltip } from "cs2/ui";
import { useLocalization } from "cs2/l10n";

export const ModButton = () => {
    const open = useValue(GAME_BINDINGS.PANEL_OPEN.binding);
    const { translate } = useLocalization();
    
    return (

        <Button
            src={"coui://stats/logo.svg"}
            variant="floating"
            tooltipLabel={translate("UI.Common.ModButtonTooltip", "Toggle Stats Panel")}
            onSelect={() => GAME_BINDINGS.PANEL_OPEN.set(!open)}
        />
    );
};
