using Game;
using HarmonyLib;
using Services;
using SML;
using System;

namespace DevTools;

[Mod.SalemMod]
public class ExampleMod
{
    public static void Start() {
        Console.WriteLine("DevTools works!");
    }
}

[DynamicSettings]
public class DevToolsDynamicSettings {
    public ModSettings.CheckboxSetting DebugMode {
        get {
            return new ModSettings.CheckboxSetting {
                Name = "Debug Mode",
                Description = "Client side only.",
                DefaultValue = false,
                Available = true,
                OnChanged = (bool b) => {
                    if (!Leo.IsGameScene()) return;

                    int userLevel = b ? 999 : 1;

                    Service.Game.Sim.simulation.userLevel.Set(userLevel);
                }
            };
        }
    }
}

[HarmonyPatch(typeof(LocalClient), nameof(LocalClient.Start))]
public class DebugUI {
    [HarmonyPostfix]
    public static void Postfix(LocalClient __instance) {
        if (__instance.gameNetworkService.LoginInfo == null) return;

        __instance.simulation.userLevel.Set(ModSettings.GetBool("Debug Mode") ? 999 : 1);
    }
}