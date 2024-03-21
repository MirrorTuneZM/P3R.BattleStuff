using P3R.BattleStuff.Reloaded.Battles.Modules;

namespace P3R.BattleStuff.Reloaded.Game;

internal unsafe class BattleService
{
    public BattleService(GameService game, IGameModule[] modules)
    {
        game.GameStateLoading += (snapshot) =>
        {
            foreach (var module in modules)
            {
                module.UpdateGameState(snapshot);
            }
        };
    }
}
