using System.Diagnostics;
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
            
            if (commandToExecute != null) {
                commandToExecute.Run(command.Skip(1).ToArray());
                continue;
            }
            
            if (TryRunExternalCommand(command)) continue;
            
            CommandNotFound(command);
        }
    }

    private static bool TryRunExternalCommand(string[] command) {
        var paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator);

        if (paths == null) return false;

        foreach (var path in paths) {
            if (!Directory.Exists(path)) continue;
            
            var files = Directory.GetFiles(path, $"{command[0]}");
            
            if (files.Length > 0) {
                using Process p = new Process();
                
                ProcessStartInfo info = new ProcessStartInfo(command[0]);

                info.Arguments = string.Join(" ", command.Skip(1));
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                p.StartInfo = info;
                p.Start();

                string output = p.StandardOutput.ReadToEnd();

                Console.WriteLine(output);

                return true;
            }
        }

        return false;
    }

    private static void CommandNotFound(string[]? command) {
        Console.WriteLine($"{command?[0]} command not found.");
    }
}