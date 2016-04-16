using Core.Bot.Main.Implementation;
using Core.Bot.Main.Implementation.Declaration;
using Core.MVC.Implementation.PlayerVsMonkeyBot;
using Core.MVC.PlayerVsMonkeyBot.Declarations;

namespace Infrastructure.MVCDependencyInjection
{
    using Core.MVC.Implementation.PlayerVsPlayer;
    using Core.MVC.PlayerVsPlayer.Declarations;

    using Ninject.Modules;

    public class MVCNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IPlayerVsPlayerGameCommandHandler>().To<PlayerVsPlayerGameCommandHandler>();
            this.Bind<IPlayerVsMonkeyBotCommandHandler>().To<PlayerVsMonkeyBotCommandHandler>();

            this.Bind<IMonkeyBot>().To<MonkeyBot>();
        }
    }
}
