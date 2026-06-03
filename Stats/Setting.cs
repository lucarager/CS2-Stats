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
    [SettingsUIGroupOrder(KeybindingsGroupStr)]
    [SettingsUIShowGroupName(KeybindingsGroupStr)]
    [SettingsUIKeyboardAction(TogglePanelStr, ActionType.Button)]
    public class Setting : ModSetting {
        public const string KeybindingsGroupStr = "KeybindingsGroupStr";
        public const string TogglePanelStr = nameof(TogglePanel);

        public Setting(IMod mod) : base(mod) {

        }

        [SettingsUISection(KeybindingsGroupStr)]
        [SettingsUIKeyboardBinding(BindingKeyboard.S, TogglePanelStr, ctrl: true, alt: true)]
        public ProxyBinding TogglePanel { get; set; }

        public override void SetDefaults() {
        }

    }

    public class LocaleEN : IDictionarySource {
        private readonly Setting m_Setting;
        
        public LocaleEN(Setting setting) {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Stats" },
                { m_Setting.GetOptionGroupLocaleID(Setting.KeybindingsGroupStr), "Keybindings" },
                { m_Setting.GetOptionLabelLocaleID(Setting.TogglePanelStr), "Toggle Stats Panel" },
                { m_Setting.GetOptionDescLocaleID(Setting.TogglePanelStr), "Opens or closes the Stats panel" },
                { "Stats.Stat[BIKERS]", "Citizens on Bikes" },
                { "UI.Common.ModButtonTooltip", "Toggle Stats Panel" }
            };
        }

        public void Unload() {

        }
    }
}
