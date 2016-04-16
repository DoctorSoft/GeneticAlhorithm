namespace Core.MVC.PlayerVsPlayer.Declarations
{
    using Core.MVC.PlayerVsPlayer.Models;

    public interface IPlayerVsPlayerGameCommandHandler
    {
        PlayerVsPlayerNewGameCommandResult ExecuteCommand(PlayerVsPlayerNewGameCommand command);

        PlayerVsPlayerMakeStepCommandResult ExecuteCommand(PlayerVsPlayerMakeStepCommand command);

        PlayerVsPlayerWinGameCommandResult ExecuteCommand(PlayerVsPlayerWinGameCommand command);

        PlayerVsPlayerTakeDrawCommandResult ExecuteCommand(PlayerVsPlayerTakeDrawCommand command);
    }
}
