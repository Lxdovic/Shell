namespace Shell.Commands;

internal sealed class ExitCommand : BuiltinCommand {
    internal override void Run(string[] args) {
        var exitCode = 0;

        if (args.Length > 0) exitCode = int.Parse(args[0]);

        Environment.Exit(exitCode);
    }
}