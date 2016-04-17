using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.MVC.PlayerVsStatisticBot.Declarations;
using Core.MVC.PlayerVsStatisticBot.Models;
using Core.TicTacToe.Constants;

namespace TicTacToe.MVC.Controllers
{
    public class PlayerVsStatisticBotController : Controller
    {
        private readonly IPlayerVsStatisticBotCommandHandler playerVsStatisticBotCommandHandler;

        public PlayerVsStatisticBotController(IPlayerVsStatisticBotCommandHandler playerVsStatisticBotCommandHandler)
        {
            this.playerVsStatisticBotCommandHandler = playerVsStatisticBotCommandHandler;
        }

        // GET: PlayerVsStatisticBot
        public ActionResult Index()
        {
            var answer = this.playerVsStatisticBotCommandHandler.ExecuteCommand(new PlayerVsStatisticBotNewGameCommand());

            if (answer.IsBotTurn)
            {
                return this.RedirectToAction("MakeBotStep", new PlayerVsStatisticBotMakeBotStepCommand { GameId = answer.GameId });
            }

            return this.View(answer);
        }

        public ActionResult MakeStep(PlayerVsStatisticBotMakeStepCommand command)
        {
            var answer = this.playerVsStatisticBotCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                if (answer.GameProcessStatistic.GameStatus == GameStatus.Draw)
                {
                    return RedirectToAction("TakeDraw", new PlayerVsStatisticBotTakeDrawCommand { GameId = command.GameId });
                }
                else
                {
                    return RedirectToAction("WinGame", new PlayerVsStatisticBotWinGameCommand { GameId = command.GameId });
                }
            }

            return this.View(answer);
        }

        public ActionResult MakeBotStep(PlayerVsStatisticBotMakeBotStepCommand command)
        {
            var answer = this.playerVsStatisticBotCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                if (answer.GameProcessStatistic.GameStatus == GameStatus.Draw)
                {
                    return RedirectToAction("TakeDraw", new PlayerVsStatisticBotTakeDrawCommand { GameId = command.GameId });
                }
                else
                {
                    return RedirectToAction("WinGame", new PlayerVsStatisticBotWinGameCommand { GameId = command.GameId });
                }
            }

            return this.View(answer);
        }

        public ActionResult TakeDraw(PlayerVsStatisticBotTakeDrawCommand command)
        {
            var answer = this.playerVsStatisticBotCommandHandler.ExecuteCommand(command);
            return this.View(answer);
        }

        public ActionResult WinGame(PlayerVsStatisticBotWinGameCommand command)
        {
            var answer = this.playerVsStatisticBotCommandHandler.ExecuteCommand(command);
            return this.View(answer);
        }
    }
}