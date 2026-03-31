using System;

namespace Celeste.Mod.ClckHelper;

public class ClckHelperModule : EverestModule {
    public static ClckHelperModule Instance { get; private set; }

    public override Type SettingsType => typeof(ClckHelperModuleSettings);
    public static ClckHelperModuleSettings Settings => (ClckHelperModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(ClckHelperModuleSession);
    public static ClckHelperModuleSession Session => (ClckHelperModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(ClckHelperModuleSaveData);
    public static ClckHelperModuleSaveData SaveData => (ClckHelperModuleSaveData) Instance._SaveData;

    public ClckHelperModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(ClckHelperModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(ClckHelperModule), LogLevel.Info);
#endif
    }

    public override void Load() {
        // TODO: apply any hooks that should always be active
    }

    public override void Unload() {
        // TODO: unapply any hooks applied in Load()
    }
}