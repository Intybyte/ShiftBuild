using BepInEx;
using HarmonyLib;

namespace ShiftBuild.Plugin
{

    [BepInPlugin("me.vaan.shiftplace", "ShiftPlace", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake() 
        {
            Harmony harmony = new Harmony("me.vaan.shiftplace");
            harmony.PatchAll();
        }       

    }
}
