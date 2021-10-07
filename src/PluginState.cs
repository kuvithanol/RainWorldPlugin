using BepInEx.Logging;

namespace squeezeThrough
{

    public sealed class PluginState
    {
        public PluginState(ManualLogSource logger)
        {
            Logger = logger;
        }

        public ManualLogSource Logger { get; }
    }
}