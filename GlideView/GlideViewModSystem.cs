using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Common.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using Vintagestory.API.MathTools;

namespace GlideView {

public class GlideViewModSystem : ModSystem {

    const string CONFIG_PATH = "glideview.json";
    ICoreClientAPI _capi;
    EnumCameraMode last_mode;
    GlideViewConfig config;

    public EnumCameraMode default_view;
    public EnumCameraMode elk_view;
    public EnumCameraMode boat_view;
    public EnumCameraMode other_mount_view;
    public EnumCameraMode sit_view;
    public EnumCameraMode glide_view;

    public EnumCameraMode detectCameraMode() {
        var entity = _capi.World.Player.Entity;

        if (entity.Controls.FloorSitting) {
            EntityPos pos = entity.Pos;
            if (pos.Pitch < config.sit_pitch) {
                return sit_view;
            }
        }

        if (entity.MountedOn != null) {

            var seat = entity.MountedOn;
            var path = seat?.Entity?.Code?.Path;

#if DEBUG
            _capi.ShowChatMessage("MountedOn code: " + seat?.Entity?.Code);
            _capi.ShowChatMessage("MountedOn path: " + seat?.Entity?.Code?.Path);
            _capi.ShowChatMessage("MountedOn type: " + seat?.GetType()?.FullName);
            _capi.ShowChatMessage("MountSupplier type: " + seat?.MountSupplier?.GetType()?.FullName);
#endif

            if (path != null && path.Contains("boat")) {
                return boat_view;
            } else if (path != null && path.Contains("elk")) {
                return elk_view;
            }

            // for other mods let's just keep it enable for now
            return other_mount_view;
        } else if (entity.Controls.Gliding) {
            return glide_view;
        }
        return default_view;
    }

    // Tested on 1.22.2
    // This is not part of public api so i use reflection to fetch the camera then
    // set the mode
    public void setCameraMode(ICoreClientAPI capi, EnumCameraMode mode) {
        if (last_mode == mode) {
            return;
        }
        var gameField = capi.GetType().GetField("game", BindingFlags.NonPublic | BindingFlags.Instance);
        var game = gameField.GetValue(capi);

        var cameraField = game.GetType().GetField("MainCamera", BindingFlags.Public | BindingFlags.Instance);
        var camera = cameraField.GetValue(game);

        // Call SetMode directly
        var setMode = camera.GetType().GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);

        var entity = _capi.World.Player.Entity;
        // Ok this is mostly for sitting but it cant do bad thing?
        // the yaw postion when sitting is not always the right one
        // i don't really understand why but let's force it when sitting and not moving
        if (!entity.Controls.TriesToMove) {
            entity.WalkYaw = entity.Pos.Yaw;
        }
        setMode.Invoke(camera, new object[] { mode });
        last_mode = mode;
    }

    public static EnumCameraMode ParseCameraMode(string value) {
        return value.ToLowerInvariant() switch { "first" => EnumCameraMode.FirstPerson,
                                                 "third" => EnumCameraMode.ThirdPerson,
                                                 "overhead" => EnumCameraMode.Overhead,
                                                 _ => EnumCameraMode.FirstPerson };
    }

    public override void StartClientSide(ICoreClientAPI api) {
        Mod.Logger.Notification("GlideView starting");
        config = BaseConfig.Load<GlideViewConfig>(api, CONFIG_PATH);
        default_view = ParseCameraMode(config.default_view);
        elk_view = ParseCameraMode(config.elk_view);
        boat_view = ParseCameraMode(config.boat_view);
        sit_view = ParseCameraMode(config.sit_view);
        glide_view = ParseCameraMode(config.glide_view);
        other_mount_view = ParseCameraMode(config.other_mount_view);
        // BaseConfig.Save<GlideViewConfig>(api, CONFIG_PATH, config);
        _capi = api;

        _capi.Event.RegisterGameTickListener(dt => { setCameraMode(_capi, detectCameraMode()); }, 100);
    }
}
}
