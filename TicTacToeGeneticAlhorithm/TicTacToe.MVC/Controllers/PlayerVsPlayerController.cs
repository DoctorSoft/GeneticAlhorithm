using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicTacToe.MVC.Controllers
{
    using Core.MVC.PlayerVsPlayer.Declarations;
    using Core.MVC.PlayerVsPlayer.Models;
    using Core.TicTacToe.Constants;

    public class PlayerVsPlayerController : Controller
    {
        private readonly IPlayerVsPlayerGameCommandHandler playerVsPlayerGameCommandHandler;

        public PlayerVsPlayerController(IPlayerVsPlayerGameCommandHandler playerVsPlayerGameCommandHandler)
        {
            this.playerVsPlayerGameCommandHandler = playerVsPlayerGameCommandHandler;
        }

        // GET: PlayerVsPlayer
        public ActionResult Index()
        {
            var answer = this.playerVsPlayerGameCommandHandler.ExecuteCommand(new PlayerVsPlayerNewGameCommand());
            return this.View(answer);
        }

        public ActionResult MakeStep(PlayerVsPlayerMakeStepCommand command)
        {
            var answer = this.playerVsPlayerGameCommandHandler.ExecuteCommand(command);

            if (answer.GameProcessStatistic.GameStatus != GameStatus.InProgress)
            {
                
            }

            return this.View(answer);
        }
    }
}