using P3R.BattleStuff.Reloaded.Configuration;
using P3R.BattleStuff.Reloaded.Game;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.BattleStuff.Reloaded.Battles.Modules;

internal unsafe class BtlCoreModule : IGameModule
{
    [Function(CallingConventions.Microsoft)]
    private delegate void InitBattle(UBtlCoreComponent* btlCore);
    private IHook<InitBattle>? initBattleHook;

    private double btlNormal_ExpMuti = Config.BtlNormal_ExpMuti_DEFAULT;
    private double btlNormal_EnemyDamageMuti = Config.BtlNormal_EnemyDamageMuti_DEFAULT;

    private double btlAdvantage_ExpMuti = Config.BtlAdvantage_ExpMuti_DEFAULT;
    private double btlAdvantage_EnemyDamageMuti = Config.BtlAdvantage_EnemyDamageMuti_DEFAULT;

    private double btlDisadvantage_ExpMuti = Config.BtlDisadvantage_ExpMuti_DEFAULT;
    private double btlDisadvantage_EnemyDamageMuti = Config.BtlDisadvantage_EnemyDamageMuti_DEFAULT;

    private DataTable? dtBtlDiffParams;
    private FBtlCalcParam[]? btlCalcParams;

    public BtlCoreModule(IDataTables dataTables)
    {
        ScanHooks.Add(
            nameof(InitBattle),
            "40 53 57 41 56 48 83 EC 30 48 89 74 24",
            (hooks, result) => this.initBattleHook = hooks.CreateHook<InitBattle>(this.InitBattleImpl, result).Activate());

        dataTables.FindDataTable("DT_BtlDifficultyParam", table =>
        {
            this.btlCalcParams = table.Rows.Select(x => *(FBtlCalcParam*)x.Self).ToArray();
            this.dtBtlDiffParams = table;
        });
    }

    private void InitBattleImpl(UBtlCoreComponent* btlCore)
    {
        this.initBattleHook!.OriginalFunction(btlCore);
        var btlContext = btlCore->EncountParameter.Preemptive;
        this.SetBtlCalcParams(btlContext);
    }

    public void UpdateGameState(GameStateSnapshot snapshot)
    {
    }

    private void SetBtlCalcParams(EBtlEncountPreemptive btlContext)
    {
        if (this.dtBtlDiffParams == null || this.btlCalcParams == null)
        {
            Log.Warning("DT_BtlDifficultyParam not set.");
            return;
        }

        for (int i = 0; i < this.dtBtlDiffParams.Rows.Length; i++)
        {
            var row = this.dtBtlDiffParams.Rows[i];

            var btlCalcParam = (FBtlCalcParam*)row.Self;
            var ogParam = this.btlCalcParams[i];
            switch (btlContext)
            {
                case EBtlEncountPreemptive.Normal:
                    btlCalcParam->ExpRate = ogParam.ExpRate * (float)this.btlNormal_ExpMuti;
                    btlCalcParam->DamageRateToEnemy = ogParam.DamageRateToEnemy * (float)this.btlNormal_EnemyDamageMuti;
                    break;
                case EBtlEncountPreemptive.Ally:
                    btlCalcParam->ExpRate = ogParam.ExpRate * (float)this.btlAdvantage_ExpMuti;
                    btlCalcParam->DamageRateToEnemy = ogParam.DamageRateToEnemy * (float)this.btlAdvantage_EnemyDamageMuti;
                    break;
                case EBtlEncountPreemptive.Enemy:
                    btlCalcParam->ExpRate = ogParam.ExpRate * (float)this.btlDisadvantage_ExpMuti;
                    btlCalcParam->DamageRateToEnemy = ogParam.DamageRateToEnemy * (float)this.btlDisadvantage_EnemyDamageMuti;
                    break;
                default:
                    break;
            }
        }
    }

    public void ApplyConfig(Config config)
    {
        this.btlNormal_ExpMuti = config.BtlNormalExpMulti;
        this.btlNormal_EnemyDamageMuti = config.BtlNormalDamageMulti;
        this.btlAdvantage_ExpMuti = config.BtlAdvantageExpMulti;
        this.btlAdvantage_EnemyDamageMuti = config.BtlAdvantageDamageMulti;
        this.btlDisadvantage_ExpMuti = config.BtlDisadvantageExpMulti;
        this.btlDisadvantage_EnemyDamageMuti = config.BtlDisadvantageDamageMulti;
    }
}


[StructLayout(LayoutKind.Sequential)]
public unsafe struct FBtlCalcParam
{
    public float DamageRateToEnemy;
    public float DamageRateToPlayer;
    public float ExpRate;
    public float DamageRateToEnemyWeak;
    public float DamageRateToPlayerWeak;
    public float DamageRateToEnemyCritical;
    public float DamageRateToPlayerCritical;
    public float MoneyRateToMaterials;
    public float BadStatusHitRateFromEnemy;
    public float BadStatusHitRateFromPlayer;
}

[StructLayout(LayoutKind.Explicit, Size = 0x568)]
public unsafe struct UBtlCoreComponent
{
    [FieldOffset(0x0000)] public UObject baseObj;
    //[FieldOffset(0x0190)] public TWeakObjectPtr<AInitReadActor> InitReadActor;
    //[FieldOffset(0x0198)] public FSoftObjectPath FormationData;
    //[FieldOffset(0x01B0)] public UDataTable* FormationTable;
    [FieldOffset(0x025C)] public uint SummonEnemyCount;
    [FieldOffset(0x0290)] public float BattleElapsedTime;
    [FieldOffset(0x0294)] public float BattleDeltaTime;
    [FieldOffset(0x0298)] public FBtlEncountParam EncountParameter;
    [FieldOffset(0x02B8)] public int EncountIndex;
    [FieldOffset(0x02BC)] public EBtlFinishResult BattleResult;
    [FieldOffset(0x02E0)] public TArray<IntPtr> ActionList;
    [FieldOffset(0x02F0)] public TArray<IntPtr> PlayerList;
    [FieldOffset(0x0300)] public TArray<IntPtr> EnemyList;
    [FieldOffset(0x0348)] public TArray<short> PlayerDataAddedIDList;
    [FieldOffset(0x0390)] public TArray<short> EnemyDataSummonIDList;
    [FieldOffset(0x03C8)] public UBtlOrder* Order;
    [FieldOffset(0x03D0)] public ABtlPhase* CurrentPhase;
    [FieldOffset(0x03D8)] public bool RequestChangePhase;
    [FieldOffset(0x03E0)] public ABtlPhase* RequestedNextPhase;
    [FieldOffset(0x03E8)] public TArray<IntPtr> UtensilEffectList;
    [FieldOffset(0x03F8)] public FString HomeFormation;
}

public enum EBtlPhaseType
{
    None = 0,
    Fighting = 1,
    Victory = 2,
    Annihilation = 3,
    Escape = 4,
    EscapeSkill = 5,
    Others = 6,
    EBtlPhaseType_MAX = 7,
};

[StructLayout(LayoutKind.Explicit, Size = 0x280)]
public unsafe struct ABtlPhase
{
    [FieldOffset(0x0000)] public UObject baseObj;
    [FieldOffset(0x0278)] public EBtlPhaseType Type;
    [FieldOffset(0x0279)] public bool ImplementInBP;
}

[StructLayout(LayoutKind.Explicit, Size = 0x48)]
public unsafe struct UBtlOrder
{
    [FieldOffset(0x0000)] public UObject baseObj;
    [FieldOffset(0x0028)] public TArray<IntPtr> InterruptList;
    [FieldOffset(0x0038)] public TArray<IntPtr> StandbyList;
}

[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct FBtlEncountParam
{
    [FieldOffset(0x0000)] public int EncountID;
    [FieldOffset(0x0004)] public EBtlEncountPreemptive Preemptive;
    [FieldOffset(0x0005)] public EBtlEncountPreemptive PreemptiveOriginal;
    [FieldOffset(0x0010)] public int StageMajor;
    [FieldOffset(0x0014)] public int StageMinor;
    [FieldOffset(0x0018)] public int EnemyBadStatus;
    [FieldOffset(0x001C)] public bool CalledFromScript;
    [FieldOffset(0x001D)] public bool IsEventResult;
}

public enum EBtlEncountPreemptive
    : byte
{
    Normal = 0,
    Enemy = 1,
    Ally = 2,
    MAX = 3,
};

public enum EBtlFinishResult
{
    None = 0,
    Win = 1,
    Lose = 2,
    Escape = 3,
    Roundup = 4,
    Trafuli = 5,
    EneEscape = 6,
    EBtlFinishResult_MAX = 7,
};