namespace Shell.Commands;

internal sealed class EchoCommand : BuiltinCommand {
    internal override void Run(string[] args) {
        var message = string.Join(" ", args);
        
        Console.WriteLine(message);
    }
}