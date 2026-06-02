import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import { StatsPanel } from "mods/statsPanel";
import { initialize } from "vanilla/Components";

const register: ModRegistrar = (moduleRegistry) => {
    // Resolve the shared base set of vanilla components/themes/focus.
    initialize(moduleRegistry);

    moduleRegistry.append("Menu", HelloWorldComponent);
    moduleRegistry.append("Menu", StatsPanel);
};

export default register;
