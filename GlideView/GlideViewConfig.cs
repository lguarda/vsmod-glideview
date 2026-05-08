using System;
using Vintagestory.API.Common;
using Vintagestory.API.Client;

// thanks to
// https://github.com/Xandu93/VSMods/blob/master/mods/xinvtweaks/src/InvTweaksConfig.cs
// for the inspiration

namespace GlideView {
public abstract class BaseConfig {
    public static T Load<T>(ICoreClientAPI capi, string path)
        where T : BaseConfig, new() {
        T config = capi.LoadModConfig<T>(path);

        if (config == null) {
            config = new T();

            Save(capi, path, config);
        }

        return config;
    }

    public static void Save<T>(ICoreClientAPI capi, string path, T obj)
        where T : BaseConfig { capi.StoreModConfig(obj, path); }
}
public class GlideViewConfig : BaseConfig {
    public string default_view = "first";
    public string elk_view = "third";
    public string boat_view = "first";
    public string glide_view = "third";
    public string other_mount_view = "third";
    public string sit_view = "overhead";
    public float sit_pitch = 3.5f;
}
}
