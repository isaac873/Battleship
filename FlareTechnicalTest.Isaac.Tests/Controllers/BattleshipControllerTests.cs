using FlareTechnicalTest.Isaac.Controllers;
using FlareTechnicalTest.Isaac.Models;
using FlareTechnicalTest.Isaac.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlareTechnicalTest.Isaac.Tests.Controllers
{
    [TestClass]
    public class BattleshipControllerTests
    {
        [TestMethod]
        public void AddBattleship_AddsShipCorrectly_WhenValidCoordinates()
        {
            var gameService = new Mock<IGameService>();

            gameService.Setup(p => p.AddBattleShip(It.IsAny<CoordinateModel>(), It.IsAny<CoordinateModel>())).
                Returns(true);

            var target = new Mock<BattleshipController>(gameService.Object);

            var result = target.Object.AddBattleship(
                new CoordinateModel
                {
                    XCoord = 0,
                    YCoord = 0
                },
                new CoordinateModel
                {
                    XCoord = 0, 
                    YCoord = 5
                });

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddBattleship_DoesNotAddShip_WhenInvalidCoordinates()
        {
            var gameService = new Mock<IGameService>();

            gameService.Setup(p => p.AddBattleShip(It.IsAny<CoordinateModel>(), It.IsAny<CoordinateModel>())).
                Returns(false);

            var target = new Mock<BattleshipController>(gameService.Object);

            var result = target.Object.AddBattleship(
                new CoordinateModel
                {
                    XCoord = 50,
                    YCoord = 50
                },
                new CoordinateModel
                {
                    XCoord = 0,
                    YCoord = 0
                });

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Provided Battleship is not in a horizontal or vertical line.", result.Message);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Attack_PerformsAttack_DeterminesGameOutcome(bool isGameOver)
        {
            var gameService = new Mock<IGameService>();

            gameService.Setup(p => p.Attack(It.IsAny<CoordinateModel>())).
                Returns(new ResultModel<AttackResult>
                {
                    Success = true,
                    Result = new AttackResult { IsHit = true }
                });
            gameService.Setup(p => p.IsGameOver()).Returns(isGameOver);

            var target = new Mock<BattleshipController>(gameService.Object);

            var result = target.Object.Attack(new CoordinateModel
            {
                XCoord = 0,
                YCoord = 0
            });

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result.IsHit);
            Assert.IsTrue(result.Result.IsGameOver == isGameOver);
        }

        [TestMethod]
        public void Attack_ReturnsNull_WhenServiceReturnsFalse()
        {
            var gameService = new Mock<IGameService>();

            gameService.Setup(p => p.Attack(It.IsAny<CoordinateModel>())).
                Returns(new ResultModel<AttackResult>
                {
                    Success = false
                });

            var target = new Mock<BattleshipController>(gameService.Object);
            var result = target.Object.Attack(new CoordinateModel
            {
                XCoord = 0,
                YCoord = 0
            });

            Assert.IsNull(result);
        }
    }
}
