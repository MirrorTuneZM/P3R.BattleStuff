using P3R.BattleStuff.Reloaded.Configuration;
using P3R.BattleStuff.Reloaded.Game;

namespace P3R.BattleStuff.Reloaded.Battles.Modules;

internal interface IGameModule
{
    void UpdateGameState(GameStateSnapshot snapshot);

    void ApplyConfig(Config config);
}