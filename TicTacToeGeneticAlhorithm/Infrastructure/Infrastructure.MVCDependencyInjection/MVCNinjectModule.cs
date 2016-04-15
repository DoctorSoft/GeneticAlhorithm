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
        }
    }
}
