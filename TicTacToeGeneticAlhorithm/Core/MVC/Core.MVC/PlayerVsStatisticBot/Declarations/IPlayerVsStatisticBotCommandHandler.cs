using Core.MVC.PlayerVsStatisticBot.Models;

namespace Core.MVC.PlayerVsStatisticBot.Declarations
{
    public interface IPlayerVsStatisticBotCommandHandler
    {
        PlayerVsStatisticBotNewGameCommandResult ExecuteCommand(PlayerVsStatisticBotNewGameCommand command);

        PlayerVsStatisticBotMakeStepCommandResult ExecuteCommand(PlayerVsStatisticBotMakeStepCommand command);

        PlayerVsStatisticBotMakeBotStepCommandResult ExecuteCommand(PlayerVsStatisticBotMakeBotStepCommand command);

        PlayerVsStatisticBotTakeDrawCommandResult ExecuteCommand(PlayerVsStatisticBotTakeDrawCommand command);

        PlayerVsStatisticBotWinGameCommandResult ExecuteCommand(PlayerVsStatisticBotWinGameCommand command);
    }
}
