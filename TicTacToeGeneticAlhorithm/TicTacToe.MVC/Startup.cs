using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TicTacToe.MVC.Startup))]
namespace TicTacToe.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
