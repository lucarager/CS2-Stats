import { ModRegistrar } from "cs2/modding";
import { StatsPanel } from "components/statsPanel";
import { initialize } from "vanilla/Components";
import { ModButton } from "components/button";

const register: ModRegistrar = (moduleRegistry) => {
    // Resolve the shared base set of vanilla components/themes/focus.
    initialize(moduleRegistry);

    moduleRegistry.append("Game", StatsPanel);
    moduleRegistry.append('UniversalModMenu', ModButton);
};

export default register;
