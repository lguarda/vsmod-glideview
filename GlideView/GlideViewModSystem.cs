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

    ICoreClientAPI _capi;

  public bool shouldDoThirdPerson() {
    var entity =
        _capi.World.Player
            .Entity; // this is already EntityPlayer which extends EntityAgent

    //_capi.ShowChatMessage($"1 oooooooooooooooooooo");
    if (entity.MountedOn != null) {
      //_capi.ShowChatMessage($"2 oooooooooooooooooooo");

      // var seat = entity.MountedOn;
      //_capi.ShowChatMessage("MountedOn type: " + seat?.GetType()?.FullName);
      //_capi.ShowChatMessage("MountSupplier type: " +
      //seat?.MountSupplier?.GetType()?.FullName);
      return true;

      //    var mount = entity.MountedOn?.MountSupplier as Entity;
      //    var path = mount?.Code?.Path;
      //_capi.ShowChatMessage($"3 oooooooooooooooooooo path{path}
      //code{mount?.Code} mount{mount} ");
      //    if (path != null && (path.Contains("elk") || path.Contains("boat")))
      //    {
      //_capi.ShowChatMessage($"4 oooooooooooooooooooo");
      //        return true;
      //    }
    } else if (entity.Controls.Gliding) {
      return true;
    }
    return false;
  }

  // Tested on 1.22.2
  // This is not part of public api so i use reflection to fetch the camera then
  // set the mode
  public void setCameraMode(ICoreClientAPI capi, EnumCameraMode mode) {
    var gameField = capi.GetType().GetField("game", BindingFlags.NonPublic |
                                                        BindingFlags.Instance);
    var game = gameField.GetValue(capi);

    var cameraField = game.GetType().GetField(
        "MainCamera", BindingFlags.Public | BindingFlags.Instance);
    var camera = cameraField.GetValue(game);

    // Call SetMode directly
    var setMode = camera.GetType().GetMethod(
        "SetMode", BindingFlags.NonPublic | BindingFlags.Instance);

    setMode.Invoke(camera, new object[] { mode });
  }

  public override void StartClientSide(ICoreClientAPI api) {
    Mod.Logger.Notification("GlideView starting");
    _capi = api;

    _capi.Event.RegisterGameTickListener(dt => {
      if (shouldDoThirdPerson()) {
        setCameraMode(_capi, EnumCameraMode.ThirdPerson);
      } else {
        setCameraMode(_capi, EnumCameraMode.FirstPerson);
      }
    }, 100);
  }
}
}
