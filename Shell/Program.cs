using Shell.Commands;

namespace Shell;

internal class Program {
    internal static readonly Dictionary<string, BuiltinCommand> BuiltinCommands = new() {
        { "exit", new ExitCommand() },
        { "echo", new EchoCommand() },
        { "type", new TypeCommand() }
    };

    private static void Main() {
        Run();
    }

    private static void Run() {
        while (true) {
            Console.Write("$ ");

            var command = Console.ReadLine()?.Split(" ");

            if (command == null || command.Length == 0) continue;

            BuiltinCommands.TryGetValue(command[0], out var commandToExecute);

            if (commandToExecute == null) {
                CommandNotFound(command);
                continue;
            }

            commandToExecute.Run(command.Skip(1).ToArray());
        }
    }

    private static void CommandNotFound(string[]? command) {
        Console.WriteLine($"{command?[0]} command not found.");
    }
}