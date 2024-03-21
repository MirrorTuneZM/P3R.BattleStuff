using System.Runtime.InteropServices;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.BattleStuff.Reloaded.Game;

[StructLayout(LayoutKind.Explicit, Size = 0x250E0)]
public unsafe struct UGlobalWork
{
    [FieldOffset(0x0000)] private UObject baseObj;
    [FieldOffset(0x0464)] public PlayerParty Party;
}

public unsafe class GameStateSnapshot
{
    public UGlobalWork PreviousState { get; init; }

    public UGlobalWork* CurrentState { get; init; }
}

[System.Runtime.CompilerServices.InlineArray(10)]
public unsafe struct PlayerParty
{
    public const int LENGTH = 10;

    public FDatUnitWork Member;
}

[StructLayout(LayoutKind.Explicit, Size = 0x2B4)]
public unsafe struct FDatUnitWork
{
    [FieldOffset(0x0000)] public uint flags;
    [FieldOffset(0x0004)] public ushort genus;
    [FieldOffset(0x0008)] public uint ID;
    [FieldOffset(0x000C)] public FDatUnitStatus Status;
    //[FieldOffset(0x002C)] public FDatUnitSupport support;
    //[FieldOffset(0x0048)] public FDatUnitPersona persona;
    //[FieldOffset(0x028C)] public FDatUnitItem Item;
    [FieldOffset(0x0296)] public ushort Operation;
    [FieldOffset(0x0298)] public ushort Message;
    [FieldOffset(0x029A)] public short maxHpUp;
    [FieldOffset(0x029C)] public short maxSpUp;
    //[FieldOffset(0x029E)] public FDatUnitSpecialSkill specialSkill;
}

[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct FDatUnitStatus
{
    [FieldOffset(0x0000)] public int Hp;
    [FieldOffset(0x0004)] public int Sp;
    [FieldOffset(0x0008)] public int tp;
    [FieldOffset(0x000C)] public uint bad;
    [FieldOffset(0x0010)] public short Level;
    [FieldOffset(0x0014)] public uint Exp;
    [FieldOffset(0x0018)] public ushort affinity;
    [FieldOffset(0x001C)] public uint personalSkill;
}

public enum Character
{
    NONE,
    Player,
    Yukari,
    Stupei,
    Akihiko,
    Mitsuru,
    Fuuka,
    Aigis,
    Ken,
    Koromaru,
    Shinjiro,
}