namespace Shell;

internal class Program {
    private static void Main() {
        Run();
    }

    private static void Run() {
        Console.Write("$ ");
        var command = Console.ReadLine()?.Split(" ");

        switch (command?[0]) {
            case "exit":
                HandleExit(command);
                break;
            case "echo":
                HandleEcho(command);
                break;
            default:
                CommandNotFound(command);
                break;
        }

        Run();
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