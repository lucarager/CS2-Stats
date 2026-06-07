namespace Stats {
    using System.Collections.Generic;

    using Colossal;
    using Colossal.IO.AssetDatabase;

    using Game.Input;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Game.UI.Widgets;

    [FileLocation(nameof(Stats))]
    [SettingsUIGroupOrder(KEYBINDINGS_GROUP_STR)]
    [SettingsUIShowGroupName(KEYBINDINGS_GROUP_STR)]
    [SettingsUIKeyboardAction(TOGGLE_PANEL_STR, ActionType.Button)]
    public class Setting : ModSetting {
        public const string KEYBINDINGS_GROUP_STR = "KeybindingsGroupStr";
        public const string TOGGLE_PANEL_STR = nameof(TogglePanel);

        public Setting(IMod mod) : base(mod) {

        }

        [SettingsUISection(KEYBINDINGS_GROUP_STR)]
        [SettingsUIKeyboardBinding(BindingKeyboard.S, TOGGLE_PANEL_STR, ctrl: true, alt: true)]
        public ProxyBinding TogglePanel { get; set; }

        public override void SetDefaults() {
        }

    }

    public class LocaleEn : IDictionarySource {
        private readonly Setting m_Setting;
        
        public LocaleEn(Setting setting) {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Stats" },
                { m_Setting.GetOptionGroupLocaleID(Setting.KEYBINDINGS_GROUP_STR), "Keybindings" },
                { m_Setting.GetOptionLabelLocaleID(Setting.TOGGLE_PANEL_STR), "Toggle Stats Panel" },
                { m_Setting.GetOptionDescLocaleID(Setting.TOGGLE_PANEL_STR), "Opens or closes the Stats panel" },
                { "Stats.Stat[BIKERS]", "Citizens on Bikes" },
                { "UI.Common.ModButtonTooltip", "Toggle Stats Panel" }
            };
        }

        public void Unload() {

        }
    }
}
