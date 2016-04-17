using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.MVC.PlayerVsMonkeyBot.Declarations;
using Core.MVC.PlayerVsMonkeyBot.Models;
using Core.MVC.PlayerVsStatisticBot.Declarations;
using Core.MVC.PlayerVsStatisticBot.Models;
using Core.TicTacToe.Constants;

namespace TicTacToe.MVC.Controllers
{
    public class MonkeyBotVsStatisticBotController : Controller
    {
        // GET: MonkeyBotVsStatisticBot
        private readonly IPlayerVsMonkeyBotCommandHandler playerVsMonkeyBotCommandHandler;

        private readonly IPlayerVsStatisticBotCommandHandler playerVsStatisticBotCommandHandler;

        public MonkeyBotVsStatisticBotController(IPlayerVsMonkeyBotCommandHandler playerVsMonkeyBotCommandHandler,
            IPlayerVsStatisticBotCommandHandler playerVsStatisticBotCommandHandler)
        {
            this.playerVsMonkeyBotCommandHandler = playerVsMonkeyBotCommandHandler;
            this.playerVsStatisticBotCommandHandler = playerVsStatisticBotCommandHandler;
        }

        public ActionResult Index()
        {
            var answer = this.playerVsMonkeyBotCommandHandler.ExecuteCommand(new PlayerVsMonkeyBotNewGameCommand());

            if (answer.IsBotTurn)
            {
                return this.RedirectToAction("MakeStatisticBotStep",
                    new PlayerVsStatisticBotMakeBotStepCommand {GameId = answer.GameId});
            }

            return this.RedirectToAction("MakeMonkeyBotStep", new PlayerVsMonkeyBotMakeBotStepCommand { GameId = answer.GameId });
        }

        public ActionResult MakeMonkeyBotStep(PlayerVsMonkeyBotMakeBotStepCommand command)
        {
            var answer = this.playerVsMonkeyBotCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                if (answer.GameProcessStatistic.GameStatus == GameStatus.Draw)
                {
                    return RedirectToAction("TakeDraw", new PlayerVsMonkeyBotTakeDrawCommand {GameId = command.GameId});
                }
                else
                {
                    return RedirectToAction("WinGame", new PlayerVsMonkeyBotWinGameCommand { GameId = command.GameId, Message = "Monkey bot won" });
                }
            }

            return this.View(answer);
        }

        public ActionResult MakeStatisticBotStep(PlayerVsStatisticBotMakeBotStepCommand command)
        {
            var answer = this.playerVsStatisticBotCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                if (answer.GameProcessStatistic.GameStatus == GameStatus.Draw)
                {
                    return RedirectToAction("TakeDraw", new PlayerVsMonkeyBotTakeDrawCommand {GameId = command.GameId});
                }
                else
                {
                    return RedirectToAction("WinGame", new PlayerVsMonkeyBotWinGameCommand {GameId = command.GameId, Message = "Statistic bot won" });
                }
            }

            return this.View(answer);
        }

        public ActionResult TakeDraw(PlayerVsMonkeyBotTakeDrawCommand command)
        {
            var answer = this.playerVsMonkeyBotCommandHandler.ExecuteCommand(command);
            return this.View(answer);
        }

        public ActionResult WinGame(PlayerVsMonkeyBotWinGameCommand command)
        {
            var answer = this.playerVsMonkeyBotCommandHandler.ExecuteCommand(command);
            answer.Message = command.Message;
            return this.View(answer);
        }
    }
}