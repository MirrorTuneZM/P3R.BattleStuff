using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;

namespace P3R.BattleStuff.Reloaded.Game;

internal unsafe class GameService
{
    [Function(CallingConventions.Microsoft)]
    private delegate UGlobalWork* GetGlobalWork();
    private IHook<GetGlobalWork>? getGlobalWorkHook;

    private DateTime prevStateTime;
    private UGlobalWork prevState;
    private bool invokingEvent;

    public GameService()
    {
        ScanHooks.Add(
            nameof(GetGlobalWork),
            "48 89 5C 24 ?? 57 48 83 EC 20 48 8B 0D ?? ?? ?? ?? 33 DB",
            (hooks, result) => this.getGlobalWorkHook = hooks.CreateHook<GetGlobalWork>(this.GetGlobalWorkImpl, result).Activate());
    }

    public Action<GameStateSnapshot>? GameStateLoading;

    private UGlobalWork* GetGlobalWorkImpl()
    {
        var result = this.getGlobalWorkHook!.OriginalFunction();
        if (result == null || this.invokingEvent)
        {
            return result;
        }

        var currentTime = DateTime.Now;
        var deltaTime = currentTime - this.prevStateTime;
        if (deltaTime.TotalMilliseconds > 100)
        {
            this.invokingEvent = true;
            this.GameStateLoading?.Invoke(new()
            {
                CurrentState = result,
                PreviousState = this.prevState,
            });
            this.invokingEvent = false;

            this.prevStateTime = currentTime;
            this.prevState = *result;
        }

        return result;
    }
}
