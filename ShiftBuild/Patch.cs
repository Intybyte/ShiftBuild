using HarmonyLib;
using System.Reflection;

namespace ShiftBuild.Patches
{
    [HarmonyPatch(typeof(Building))]
    [HarmonyPatch("OnClick", MethodType.Normal)]
    public class ShiftBuild
    {
        public static void Postfix(Building __instance, Building.State ___state)
        {
            if (__instance == null)
            {
                return;
            }


            if (___state == Building.State.None)
            {
                return;
            }

            bool shiftActive = Singleton<PlayerController>.Instance?.ShiftActive ?? false;
            if (!shiftActive)
            {
                return;
            }

            switch (___state)
            {
                case Building.State.Build:
                    if (TryRemoveResources(__instance))
                    {
                        ExecuteMethod("CompleteBuild", __instance);
                    }
                    break;
                case Building.State.Upgrade:
                    if (TryRemoveResources(__instance))
                    {
                        ExecuteMethod("CompleteUpgrade", __instance);
                    }
                    break;
                case Building.State.Broken:
                    if (TryRemoveResources(__instance))
                    {
                        ExecuteMethod("CompleteBroken", __instance);
                    }
                    break;
            }
        }

        public static void ExecuteMethod(string name, Building instance)
        {
            MethodInfo method = typeof(Building).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(instance, null);
        }

        private static bool TryRemoveResources(Building build)
        {
            ResourceData[] remainingResources = build.GetRemainingResources();
            InventoryManager instance = Singleton<InventoryManager>.Instance;
            foreach (ResourceData resourceData in remainingResources)
            {
                if (!instance.HasResource(resourceData))
                {
                    return false;
                }
            }
            foreach (ResourceData resourceData2 in remainingResources)
            {
                instance.Take(resourceData2);
            }
            return true;
        }
    }
}