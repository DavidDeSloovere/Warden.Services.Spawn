namespace Warden.Services.Spawn.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => "Welcome to the Warden.Services.Spawn API!");
        }
    }
}