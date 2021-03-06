﻿using FlareTechnicalTest.Isaac.Models;
using FlareTechnicalTest.Isaac.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlareTechnicalTest.Isaac.Controllers
{
    [Route("game")]
    public class BattleshipController
    {
        private IGameService _gameService;

        public BattleshipController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [Route("create")]
        public void CreateGame()
        {
            _gameService.CreateGame();
        }

        [Route("add-ship")]
        public ResultModel AddBattleship(CoordinateModel startingCoord, CoordinateModel endingCoord)
        {
            var result = _gameService.AddBattleShip(startingCoord, endingCoord);

            if (!result)
            {
                return new ResultModel
                {
                    Success = false,
                    Message = "Provided Battleship is not in a horizontal or vertical line."
                };
            }

            return new ResultModel { Success = true };
        }

        [Route("attack")]
        public ResultModel<AttackResult> Attack(CoordinateModel coords)
        {
            var result = _gameService.Attack(coords);
            
            if (result == null || !result.Success)
            {
                return new ResultModel<AttackResult>
                {
                    Success = false,
                    Message = "An error occurred with your attack. Please try a different set of coordinates."
                };
            }

            // If a ship is hit, we should check if the game is over. 
            if (result.Result.IsHit)
            {
                result.Result.IsGameOver = _gameService.IsGameOver();
            }

            return result;            
        }
    }
}
