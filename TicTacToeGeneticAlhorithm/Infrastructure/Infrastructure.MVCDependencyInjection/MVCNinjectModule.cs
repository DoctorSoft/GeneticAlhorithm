using Core.Bot.Main.Implementation;
using Core.Bot.Main.Implementation.Declaration;
using Core.Bot.Statistic.Implementation;
using Core.Bot.Statistic.Implementation.Declaration;
using Core.MVC.Implementation.PlayerVsMonkeyBot;
using Core.MVC.Implementation.PlayerVsStatisticBot;
using Core.MVC.PlayerVsMonkeyBot.Declarations;
using Core.MVC.PlayerVsStatisticBot.Declarations;

namespace Infrastructure.MVCDependencyInjection
{
    using Core.Bot.Genetic.Implementation;
    using Core.Bot.Genetic.Implementation.Declaration;
    using Core.MVC.Implementation.PlayerVsGeneticBot;
    using Core.MVC.Implementation.PlayerVsPlayer;
    using Core.MVC.PlayerVsGeneticBot.Declarations;
    using Core.MVC.PlayerVsPlayer.Declarations;

    using Ninject.Modules;

    public class MVCNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IPlayerVsPlayerGameCommandHandler>().To<PlayerVsPlayerGameCommandHandler>();
            this.Bind<IPlayerVsMonkeyBotCommandHandler>().To<PlayerVsMonkeyBotCommandHandler>();
            this.Bind<IPlayerVsStatisticBotCommandHandler>().To<PlayerVsStatisticBotCommandHandler>();
            this.Bind<IPlayerVsGeneticBotCommandHandler>().To<PlayerVsGeneticBotCommandHandler>();

            this.Bind<IMonkeyBot>().To<MonkeyBot>();
            this.Bind<IStatisticBot>().To<StatisticBot>();
            this.Bind<IGeneticBot>().To<GeneticBot>();
            this.Bind<ILogicBot>().To<LogicBot>();

            this.Bind<IGeneticBotDeveloper>().To<GeneticBotDeveloper>();
        }
    }
}
