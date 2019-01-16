using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using FlareTechnicalTest.Isaac.Services;
using FlareTechnicalTest.Isaac.Models;
using System.Linq;

namespace FlareTechnicalTest.Isaac.Tests.Services
{
    [TestClass]
    public class GameServiceTests
    {
        [TestMethod]
        public void AddBattleShip_FailsToAddBattleship_ReturnsFalse()
        {
            var target = new Mock<TestGameService>(false);

            var result = target.Object.AddBattleShip(new CoordinateModel
            {
                XCoord = 15,
                YCoord = 15
            }, new CoordinateModel
            {
                XCoord = 12,
                YCoord = 44
            });

            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 4)]
        [DataRow(1, 0, 1, 7)]
        [DataRow(7, 7, 7, 4)]
        public void AddBattleShip_CorrectlyAddsShipToGameBoard(int startX, int startY, int endX, int endY)
        {
            var target = new Mock<TestGameService>(false);

            var result = target.Object.AddBattleShip(new CoordinateModel
            {
                XCoord = startX,
                YCoord = startY
            }, new CoordinateModel
            {
                XCoord = endX,
                YCoord = endY
            });

            Assert.IsTrue(result);

            var squares = target.Object.GB.Squares.Where(x => 
                x.Coordinates.XCoord >= startX && 
                x.Coordinates.YCoord >= startY &&
                x.Coordinates.XCoord <= endX && 
                x.Coordinates.YCoord <= endY).ToList();

            foreach (var square in squares)
            {
                Assert.AreEqual(SquareStatus.Ship, square.Status);
            }
        }

        [TestMethod]
        public void Attack_Fails_WhenSquareNotFound()
        {
            var target = new Mock<TestGameService>(true);

            var result = target.Object.Attack(new CoordinateModel
            {
                XCoord = 999,
                YCoord = 999
            });

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void Attack_ReturnsTrue_WhenBattleshipHit()
        {
            var target = new Mock<TestGameService>(true);

            var result = target.Object.Attack(new CoordinateModel
            {
                XCoord = 0,
                YCoord = 0
            });

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result.IsHit);
        }

        [TestMethod]
        public void Attack_ReturnsFalse_WhenBattleshipMissed()
        {
            var target = new Mock<TestGameService>(true);

            var result = target.Object.Attack(new CoordinateModel
            {
                XCoord = 7,
                YCoord = 7
            });

            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.Result.IsHit);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void IsGameOver_ReturnsGameStatus(bool shipsRemaining)
        {
            var target = new Mock<TestGameService>(shipsRemaining);

            var result = target.Object.IsGameOver();

            Assert.IsTrue(shipsRemaining != result);
        }

        public class TestGameService : GameService
        {
            public GameBoard GB { get { return _gameBoard; } }

            public TestGameService(bool addShip)
            {
                _gameBoard = new GameBoard();
                if (addShip)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        // This will plot a battle ship at '0,0', '0,1', '0,2', '0,3'
                        _gameBoard.Squares[i].Status = SquareStatus.Ship;
                    }
                }
            }
        }
    }
}
