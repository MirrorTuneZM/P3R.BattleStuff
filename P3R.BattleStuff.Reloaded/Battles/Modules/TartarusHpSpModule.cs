using P3R.BattleStuff.Reloaded.Configuration;
using P3R.BattleStuff.Reloaded.Game;
using Reloaded.Hooks.Definitions.X64;

namespace P3R.BattleStuff.Reloaded.Battles.Modules;

internal unsafe class TartarusHpSpModule : IGameModule
{
    [Function(CallingConventions.Microsoft)]
    private delegate int GetUnitMaxHP_SP_TP(FDatUnitWork* unit);
    private GetUnitMaxHP_SP_TP? getMaxHp;
    private GetUnitMaxHP_SP_TP? getMaxSp;

    [Function(CallingConventions.Microsoft)]
    private delegate int DungeonGetFloorNo();
    private DungeonGetFloorNo? getFloorNo;

    private DateTime prevSpDotTime = DateTime.Now;

    private bool isEnabled = true;
    private int dotTimeMs = Config.TartarusHpSp_DotTimeSeconds_DEFAULT * 1000;
    private double maxHpDotRatio = (double)Config.TartarusHpSp_MaxHpDotRatio_DEFAULT / 100;
    private double maxSpDotRatio = (double)Config.TartarusHpSp_MaxSpDotRatio_DEFAULT / 100;

    public TartarusHpSpModule()
    {
        ScanHooks.Add(
            nameof(DungeonGetFloorNo),
            "4C 8B 05 ?? ?? ?? ?? 4D 85 C0 74 ?? 41 8B 40 ?? 3B 05 ?? ?? ?? ?? 7D ?? 99 0F B7 D2 03 C2 8B " +
            "C8 0F B7 C0 2B C2 48 98 C1 F9 10 48 63 C9 48 8D 14 ?? 48 8B 05 ?? ?? ?? ?? 48 8B 0C ?? 48 8D " +
            "04 ?? EB ?? 33 C0 8B 48 ?? C1 E9 1D F6 C1 01 75 ?? 41 8B 80 ?? ?? ?? ?? C3 33 C0 C3 ?? ?? ?? " +
            "?? ?? ??",
            (hooks, result) => this.getFloorNo = hooks.CreateWrapper<DungeonGetFloorNo>(result, out _));

        ScanHooks.Add(
            $"{nameof(GetUnitMaxHP_SP_TP)}: Use of GetUnitMaxHP",
            "48 89 74 24 ?? 57 48 83 EC 30 41 80 B8 ?? ?? ?? ?? 00 49 8B F8 48 8B F1 74 ?? 41 81 B8 ?? ?? ?? ?? 61 01 00 00",
            (hooks, result) =>
            {
                var currentResult = result + 0x4d;
                currentResult = currentResult + *(int*)currentResult + 4;
                Log.Information($"GetUnitMaxSP: {currentResult:X}");
                this.getMaxHp = hooks.CreateWrapper<GetUnitMaxHP_SP_TP>(currentResult, out _);
            });

        ScanHooks.Add(
            $"{nameof(GetUnitMaxHP_SP_TP)}: Use of GetUnitMaxSP", "40 55 48 83 EC 30 8B 8A",
            (hooks, result) =>
            {
                var currentResult = result + 0xEC; // call GetUnitMaxSP (thunk)
                currentResult = currentResult + *(int*)currentResult + 4;

                Log.Information($"GetUnitMaxSP: {currentResult:X}");
                this.getMaxSp = hooks.CreateWrapper<GetUnitMaxHP_SP_TP>(currentResult, out _);
            });
    }

    public void UpdateGameState(GameStateSnapshot snapshot)
    {
        if (this.isEnabled && this.getFloorNo!() > 0)
        {
            var currentTime = DateTime.Now;
            var delta = currentTime - prevSpDotTime;
            if (delta.TotalMilliseconds >= this.dotTimeMs)
            {
                Log.Debug($"Tartarus HP/SP DOT || HP: -{this.maxHpDotRatio:P0} Max HP || SP: -{this.maxSpDotRatio:P0} Max SP");
                for (int i = 0; i < PlayerParty.LENGTH; i++)
                {
                    var unit = &snapshot.CurrentState->Party[i];
                    var spDamage = int.Max(1, (int)(this.getMaxSp!(unit) * this.maxSpDotRatio));
                    var hpDamage = int.Max(1, (int)(this.getMaxHp!(unit) * this.maxHpDotRatio));

                    var currentSp = unit->Status.Sp;
                    var currentHp = unit->Status.Hp;
                    var newSp = int.Clamp(currentSp - spDamage, 0, currentSp);
                    var newHp = int.Clamp(currentHp - hpDamage, 0, currentHp);

                    unit->Status.Sp = newSp;
                    unit->Status.Hp = newHp;

                    Log.Debug($"{unit->ID} || -{hpDamage} HP || -{spDamage} SP");
                }

                this.prevSpDotTime = currentTime;
            }
        }
    }

    public void ApplyConfig(Config config)
    {
        this.isEnabled = config.TartarusHpSp_Enabled;
        this.dotTimeMs = config.TartarusHpSp_DotTimeSeconds * 1000;
        this.maxHpDotRatio = (double)config.TartarusHpSp_MaxHpDotRatio / 100;
        this.maxSpDotRatio = (double)config.TartarusHpSp_MaxSpDotRatio / 100;

        Log.Debug($"Tartarus HP/SP DOT || HP: -{this.maxHpDotRatio:P0} Max HP || SP: -{this.maxSpDotRatio:P0} Max SP");
    }
}
