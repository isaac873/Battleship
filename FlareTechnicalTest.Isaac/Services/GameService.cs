using System.Linq;
using FlareTechnicalTest.Isaac.Models;

namespace FlareTechnicalTest.Isaac.Services
{
    public class GameService : IGameService
    {
        protected GameBoard _gameBoard;

        public void CreateGame()
        {
            if (_gameBoard == null)
            {
                _gameBoard = new GameBoard();
            }
        }

        public bool AddBattleShip(CoordinateModel startingCoord, CoordinateModel endingCoord)
        {
            if (_gameBoard == null)
            {
                return false;
            }

            if (startingCoord.XCoord != endingCoord.XCoord
                && startingCoord.YCoord != endingCoord.YCoord)
            {
                return false;
            }

            // Determine if the ship is horizontally or vertically placed.
            if (startingCoord.XCoord == endingCoord.XCoord)
            {
                PlotShip(startingCoord, endingCoord.YCoord, false);
            }
            else
            {
                PlotShip(startingCoord, endingCoord.XCoord, true);
            }

            return true;
        }

        public ResultModel<AttackResult> Attack(CoordinateModel attackCoords)
        {
            if (_gameBoard == null)
            {
                return null;
            }

            var square = _gameBoard.Squares.FirstOrDefault(p =>
                p.Coordinates.YCoord == attackCoords.YCoord &&
                p.Coordinates.XCoord == attackCoords.XCoord);

            if (square == null)
            {
                return new ResultModel<AttackResult>
                {
                    Success = false,
                    Message = "Coordinate not contained within grid."
                };
            }

            // Flag ship as hit or miss.
            square.Status = square.Status == SquareStatus.Ship ? SquareStatus.Hit : SquareStatus.Miss;

            return new ResultModel<AttackResult>
            {
                Success = true,
                Result = new AttackResult { IsHit = square.Status == SquareStatus.Hit }
            };
        }

        public bool IsGameOver()
        {
            // If there are any "ship" types left, the game is ongoing.
            return !(_gameBoard.Squares.Any(p => p.Status == SquareStatus.Ship));
        }

        private void PlotShip(CoordinateModel startingCoord, int end, bool incrementX)
        {
            var xRow = startingCoord.XCoord;
            var yCol = startingCoord.YCoord;

            // We'll loop through and plot the ship position from the start position to the end.
            for (var i = yCol; i <= end; i++)
            {
                var square = _gameBoard.Squares.FirstOrDefault(p =>
                    p.Coordinates.YCoord == yCol &&
                    p.Coordinates.XCoord == xRow);

                square.Status = SquareStatus.Ship;

                // We'll either increment "vertically" or "horizontally"
                if (incrementX)
                {
                    xRow++;
                }
                else
                {
                    yCol++;
                }
            }
        }
    }
}
