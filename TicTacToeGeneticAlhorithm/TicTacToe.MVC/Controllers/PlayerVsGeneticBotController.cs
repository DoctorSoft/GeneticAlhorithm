using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicTacToe.MVC.Controllers
{
    using Core.MVC.PlayerVsGeneticBot.Declarations;
    using Core.MVC.PlayerVsGeneticBot.Models;
    using Core.TicTacToe.Constants;

    public class PlayerVsGeneticBotController : Controller
    {
        private readonly IPlayerVsGeneticBotCommandHandler playerVsGeneticBotCommandHandler;

        public PlayerVsGeneticBotController(IPlayerVsGeneticBotCommandHandler playerVsGeneticBotCommandHandler)
        {
            this.playerVsGeneticBotCommandHandler = playerVsGeneticBotCommandHandler;
        }

        // GET: PlayerVsGeneticBot
        public ActionResult Index()
        {
            var answer = this.playerVsGeneticBotCommandHandler.ExecuteCommand(new PlayerVsGeneticBotNewGameCommand());

            if (answer.IsBotTurn)
            {
                return this.RedirectToAction("MakeBotStep", new PlayerVsGeneticBotMakeBotStepCommand { GameId = answer.GameId });
            }

            return this.View(answer);
        }

        public ActionResult MakeStep(PlayerVsGeneticBotMakeStepCommand command)
        {
            var answer = this.playerVsGeneticBotCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                if (answer.GameProcessStatistic.GameStatus == GameStatus.Draw)
                {
                    return RedirectToAction("TakeDraw", new PlayerVsGeneticBotTakeDrawCommand { GameId = command.GameId });
                }
                else
                {
                    return RedirectToAction("WinGame", new PlayerVsGeneticBotWinGameCommand { GameId = command.GameId });
                }
            }

            return this.View(answer);
        }

        public ActionResult MakeBotStep(PlayerVsGeneticBotMakeBotStepCommand command)
        {
            var answer = this.playerVsGeneticBotCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                if (answer.GameProcessStatistic.GameStatus == GameStatus.Draw)
                {
                    return RedirectToAction("TakeDraw", new PlayerVsGeneticBotTakeDrawCommand { GameId = command.GameId });
                }
                else
                {
                    return RedirectToAction("WinGame", new PlayerVsGeneticBotWinGameCommand { GameId = command.GameId });
                }
            }

            return this.View(answer);
        }

        public ActionResult TakeDraw(PlayerVsGeneticBotTakeDrawCommand command)
        {
            var answer = this.playerVsGeneticBotCommandHandler.ExecuteCommand(command);
            return this.View(answer);
        }

        public ActionResult WinGame(PlayerVsGeneticBotWinGameCommand command)
        {
            var answer = this.playerVsGeneticBotCommandHandler.ExecuteCommand(command);
            return this.View(answer);
        }
    }
}