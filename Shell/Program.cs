namespace Shell;

internal class Program {
    private static readonly Dictionary<string, Action<string[]>> BuiltinCommands = new() {
        { "exit", HandleExit },
        { "echo", HandleEcho },
        { "type", HandleType }
    };

    private static void Main() {
        Run();
    }

    private static void Run() {
        while (true) {
            Console.Write("$ ");

            var command = Console.ReadLine()?.Split(" ");

            if (command == null || command.Length == 0) continue;

            BuiltinCommands.TryGetValue(command[0], out var action);

            action ??= CommandNotFound;
            action(command);
        }
    }

    private static void HandleType(string[] command) {
        if (command.Length <= 1) {
            Console.WriteLine("type: missing argument");
            return;
        }

        if (IsBuiltinCommand(command[1])) {
            Console.WriteLine($"{command[1]} is a shell builtin.");
            return;
        }

        if (TryFindInPath(command[1], out var filePath)) {
            Console.WriteLine($"{command[1]} is {filePath}");
        } else {
            Console.WriteLine($"{command[1]} not found");
        }
    }

    private static bool IsBuiltinCommand(string command) {
        return BuiltinCommands.ContainsKey(command);
    }

    private static bool TryFindInPath(string command, out string filePath) {
        var paths = Environment.GetEnvironmentVariable("PATH")?.Split(';');
        if (paths == null) {
            filePath = string.Empty;
            return false;
        }

        foreach (var path in paths) {
            if (!Directory.Exists(path)) continue;

            var files = Directory.GetFiles(path, $"{command}.exe");
            if (files.Length > 0) {
                filePath = files[0];
                return true;
            }
        }

        filePath = string.Empty;
        return false;
    }

    private static void HandleEcho(string[] command) {
        var message = string.Join(" ", command.Skip(1));
        Console.WriteLine(message);
    }

    private static void HandleExit(string[] command) {
        var exitCode = 0;

        if (command.Length > 1) exitCode = int.Parse(command[1]);

        Environment.Exit(exitCode);
    }

    private static void CommandNotFound(string[]? command) {
        Console.WriteLine($"{command?[0]} command not found.");
    }
}