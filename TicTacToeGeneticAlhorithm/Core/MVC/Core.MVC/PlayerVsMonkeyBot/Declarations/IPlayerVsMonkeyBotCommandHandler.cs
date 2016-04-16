using Core.MVC.PlayerVsMonkeyBot.Models;

namespace Core.MVC.PlayerVsMonkeyBot.Declarations
{
    public interface IPlayerVsMonkeyBotCommandHandler
    {
        PlayerVsMonkeyBotNewGameCommandResult ExecuteCommand(PlayerVsMonkeyBotNewGameCommand command);

        PlayerVsMonkeyBotMakeStepCommandResult ExecuteCommand(PlayerVsMonkeyBotMakeStepCommand command);

        PlayerVsMonkeyBotMakeBotStepCommandResult ExecuteCommand(PlayerVsMonkeyBotMakeBotStepCommand command);

        PlayerVsMonkeyBotTakeDrawCommandResult ExecuteCommand(PlayerVsMonkeyBotTakeDrawCommand command);

        PlayerVsMonkeyBotWinGameCommandResult ExecuteCommand(PlayerVsMonkeyBotWinGameCommand command);
    }
}
