using FlareTechnicalTest.Isaac.Models;

namespace FlareTechnicalTest.Isaac.Services
{
    public interface IGameService
    {
        void CreateGame();

        bool AddBattleShip(CoordinateModel startingCoord, CoordinateModel endingCoord);

        ResultModel<AttackResult> Attack(CoordinateModel attackCoords);

        bool IsGameOver();
    }
}
