using System.CommandLine;

namespace Pomni;

class Program
{
    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Nix dependency pinning");
        
        var initCommand = new Command("init");
        rootCommand.Add(initCommand);
        
        var updateCommand = new Command("update");
        rootCommand.Add(updateCommand);
        
        var botCommand = new Command("bot");
        rootCommand.Add(botCommand);
        
        return rootCommand.Parse(args).Invoke();
    }
}
