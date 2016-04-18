namespace Core.Bot.Genetic.Implementation.Declaration
{
    using Data.Migration.Contexts;

    public interface IGeneticBotDeveloper
    {
        void GenerateNextGeneration(TicTacToeContext context);
    }
}
