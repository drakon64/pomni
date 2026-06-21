using System.CommandLine;
using Pomni.Commands;
using Pomni.Commands.Update;
using Pomni.Model;

namespace Pomni;

class Program
{
    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Nix dependency pinning");

        var initCommand = new Command("init");
        initCommand.SetAction(_ => Init.InitPomniJson());
        rootCommand.Add(initCommand);

        var nameArgument = new Argument<string>("name");
        var forgeArgument = new Argument<Forge>("forge");
        var repositoryArgument = new Argument<string>("repository");

        var branchOption = new Option<string>("-b", "--branch");
        var referenceTypeOption = new Option<ReferenceType?>("-t", "--reference-type");
        var frozenOption = new Option<bool?>("-f", "--frozen");

        var addCommand = new Command("add");
        addCommand.Arguments.Add(nameArgument);
        addCommand.Arguments.Add(forgeArgument);
        addCommand.Arguments.Add(repositoryArgument);

        addCommand.Options.Add(branchOption);
        addCommand.Options.Add(referenceTypeOption);
        addCommand.Options.Add(frozenOption);

        addCommand.SetAction(parseResult =>
            Add.AddRepository(
                parseResult.GetRequiredValue(nameArgument),
                parseResult.GetRequiredValue(forgeArgument),
                parseResult.GetRequiredValue(repositoryArgument),
                parseResult.GetValue(branchOption),
                parseResult.GetValue(referenceTypeOption),
                parseResult.GetValue(frozenOption)
            )
        );
        rootCommand.Add(addCommand);

        var updateCommand = new Command("update");
        updateCommand.SetAction(_ => Update.UpdateRepositories());
        rootCommand.Add(updateCommand);

        var botCommand = new Command("bot");
        rootCommand.Add(botCommand);

        return rootCommand.Parse(args).Invoke();
    }
}
