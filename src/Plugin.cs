using BepInEx;
using OptionalUI;
using squeezeThrough;

namespace squeezeThrough
{

    [BepInPlugin("org.sov.phlegmShrink", nameof(squeezeThrough), "0.1.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        // Entry point for the plugin.
        public void OnEnable()
        {
            PluginState state = new(Logger);

            _ = new Hooks(state);
        }

        public static OptionInterface LoadOI()
        {
            return new MyOI();
        }
    }
}