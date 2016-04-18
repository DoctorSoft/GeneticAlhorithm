using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicTacToe.MVC.Controllers
{
    using Core.MVC.PlayerVsMonkeyBot.Declarations;
    using Core.MVC.PlayerVsMonkeyBot.Models;
    using Core.MVC.PlayerVsMonkeyBot.Declarations;
    using Core.MVC.PlayerVsMonkeyBot.Models;
    using Core.TicTacToe.Constants;

    public class MonkeyBotVsMonkeyBotController : Controller
    {
        // GET: MonkeyBotVsMonkeyBot
        private readonly IPlayerVsMonkeyBotCommandHandler playerVsMonkeyBotCommandHandler;

        public MonkeyBotVsMonkeyBotController(IPlayerVsMonkeyBotCommandHandler playerVsMonkeyBotCommandHandler)
        {
            this.playerVsMonkeyBotCommandHandler = playerVsMonkeyBotCommandHandler;
        }

        public ActionResult Index()
        {
            var answer = this.playerVsMonkeyBotCommandHandler.ExecuteCommand(new PlayerVsMonkeyBotNewGameCommand());

            return this.RedirectToAction("MakeMonkeyBotStep", new PlayerVsMonkeyBotMakeBotStepCommand { GameId = answer.GameId });
        }

        public ActionResult MakeMonkeyBotStep(PlayerVsMonkeyBotMakeBotStepCommand command)
        {
            var answer = this.playerVsMonkeyBotCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                if (answer.GameProcessStatistic.GameStatus == GameStatus.Draw)
                {
                    return RedirectToAction("TakeDraw", new PlayerVsMonkeyBotTakeDrawCommand { GameId = command.GameId });
                }
                else
                {
                    return RedirectToAction("WinGame", new PlayerVsMonkeyBotWinGameCommand { GameId = command.GameId, Message = "Monkey bot won" });
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