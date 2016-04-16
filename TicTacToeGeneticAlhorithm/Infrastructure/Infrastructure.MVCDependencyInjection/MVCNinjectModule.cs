using Core.Bot.Main.Implementation;
using Core.Bot.Main.Implementation.Declaration;

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

            this.Bind<IMonkeyBot>().To<MonkeyBot>();
        }
    }
}
