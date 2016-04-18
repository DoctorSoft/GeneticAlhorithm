namespace Core.MVC.PlayerVsGeneticBot.Declarations
{
    using Core.MVC.PlayerVsGeneticBot.Models;

    public interface IPlayerVsGeneticBotCommandHandler
    {
        PlayerVsGeneticBotNewGameCommandResult ExecuteCommand(PlayerVsGeneticBotNewGameCommand command);

        PlayerVsGeneticBotMakeStepCommandResult ExecuteCommand(PlayerVsGeneticBotMakeStepCommand command);

        PlayerVsGeneticBotMakeBotStepCommandResult ExecuteCommand(PlayerVsGeneticBotMakeBotStepCommand command);

        PlayerVsGeneticBotTakeDrawCommandResult ExecuteCommand(PlayerVsGeneticBotTakeDrawCommand command);

        PlayerVsGeneticBotWinGameCommandResult ExecuteCommand(PlayerVsGeneticBotWinGameCommand command);
    }
}
