namespace Core.MVC.Implementation.PlayerVsPlayer
{
    using System.Linq;

    using Core.MVC.PlayerVsPlayer.Declarations;
    using Core.MVC.PlayerVsPlayer.Models;
    using Core.TicTacToe.Declaration;

    using Data.Migration.Contexts;
    using Data.Model.TicTacToe;

    public class PlayerVsPlayerGameCommandHandler : IPlayerVsPlayerGameCommandHandler
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        public PlayerVsPlayerGameCommandHandler(INewGameFieldCreator newGameFieldCreator, IFieldStateConverter fieldStateConverter)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
        }

        public PlayerVsPlayerNewGameCommandResult ExecuteCommand(PlayerVsPlayerNewGameCommand command)
        {
            PlayerVsPlayerNewGameCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var gameField = this.newGameFieldCreator.CreateNewGameField();
                var fieldCode = this.fieldStateConverter.GameFieldToString(gameField);

                var field = context.Set<Field>().FirstOrDefault(field1 => field1.FirstVariant == fieldCode
                                                                          || field1.SecondVariant == fieldCode
                                                                          || field1.ThirdVariant == fieldCode
                                                                          || field1.FourthVariant == fieldCode);
                var number = this.GetCodeNumber(fieldCode, field);

                var game = new Game { Field = field, FieldNumber = number };
                context.Set<Game>().Add(game);
                context.SaveChanges();

                var gameId = game.GameId;

                result = new PlayerVsPlayerNewGameCommandResult { GameField = gameField, GameId = gameId };
            }

            return result;
        }

        private int GetCodeNumber(string fieldCode, Field field)
        {
            if (field.FirstVariant == fieldCode)
            {
                return 1;
            }

            if (field.SecondVariant == fieldCode)
            {
                return 2;
            }

            if (field.ThirdVariant == fieldCode)
            {
                return 3;
            }

            return 4;
        }
    }
}
