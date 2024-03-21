using P3R.BattleStuff.Reloaded.Battles.Modules;
using P3R.BattleStuff.Reloaded.Configuration;
using P3R.BattleStuff.Reloaded.Game;
using P3R.BattleStuff.Reloaded.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System.Diagnostics;
using System.Drawing;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3R.BattleStuff.Reloaded;

public unsafe class Mod : ModBase
{
    private readonly IModLoader modLoader;
    private readonly IReloadedHooks? hooks;
    private readonly ILogger log;
    private readonly IMod owner;

    private Config config;
    private readonly IModConfig modConfig;

    private readonly GameService gameService;
    private readonly BattleService battleService;
    private readonly IGameModule[] gameModules;

    public Mod(ModContext context)
    {
        this.modLoader = context.ModLoader;
        this.hooks = context.Hooks!;
        this.log = context.Logger;
        this.owner = context.Owner;
        this.config = context.Configuration;
        this.modConfig = context.ModConfig;

        Log.Initialize("P3R.BattleStuff", this.log, Color.White);
        Log.LogLevel = this.config.LogLevel;

#if DEBUG
        Debugger.Launch();
#endif

        this.modLoader.GetController<IStartupScanner>().TryGetTarget(out var scanner);
        this.modLoader.GetController<IDataTables>().TryGetTarget(out var dataTables);
        this.gameService = new();
        this.gameModules =
        [
            new TartarusHpSpModule(),
            new BtlCoreModule(dataTables!),
        ];

        this.battleService = new(this.gameService, this.gameModules);
        this.ApplyConfig();

        ScanHooks.Initialize(scanner!, this.hooks);
    }

    private void ApplyConfig()
    {
        Log.LogLevel = this.config.LogLevel;
        foreach (var module in this.gameModules)
        {
            module.ApplyConfig(this.config);
        }
    }

    #region Standard Overrides
    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        config = configuration;
        log.WriteLine($"[{modConfig.ModId}] Config Updated: Applying");
        this.ApplyConfig();
    }
    #endregion

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion
}
