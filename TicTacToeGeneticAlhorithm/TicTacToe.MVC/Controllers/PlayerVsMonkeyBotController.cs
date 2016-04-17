using System.Web.Mvc;
using Core.MVC.PlayerVsMonkeyBot.Declarations;
using Core.MVC.PlayerVsMonkeyBot.Models;
using Core.TicTacToe.Constants;

namespace TicTacToe.MVC.Controllers
{
    public class PlayerVsMonkeyBotController : Controller
    {
        private readonly IPlayerVsMonkeyBotCommandHandler playerVsMonkeyBotCommandHandler;

        public PlayerVsMonkeyBotController(IPlayerVsMonkeyBotCommandHandler playerVsMonkeyBotCommandHandler)
        {
            this.playerVsMonkeyBotCommandHandler = playerVsMonkeyBotCommandHandler;
        }

        // GET: PlayerVsMonkeyBot
        public ActionResult Index()
        {
            var answer = this.playerVsMonkeyBotCommandHandler.ExecuteCommand(new PlayerVsMonkeyBotNewGameCommand());

            if (answer.IsBotTurn)
            {
                return this.RedirectToAction("MakeBotStep", new PlayerVsMonkeyBotMakeBotStepCommand { GameId = answer.GameId });
            }

            return this.View(answer);
        }

        public ActionResult MakeStep(PlayerVsMonkeyBotMakeStepCommand command)
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
                    return RedirectToAction("WinGame", new PlayerVsMonkeyBotWinGameCommand { GameId = command.GameId });
                }
            }

            return this.View(answer);
        }

        public ActionResult MakeBotStep(PlayerVsMonkeyBotMakeBotStepCommand command)
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
                    return RedirectToAction("WinGame", new PlayerVsMonkeyBotWinGameCommand { GameId = command.GameId });
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
            return this.View(answer);
        }
    }
}