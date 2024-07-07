namespace Shell.Commands;

internal sealed class TypeCommand : BuiltinCommand {
    internal override void Run(string[] args) {
        if (args.Length <= 0) {
            Console.WriteLine("type: missing argument");
            return;
        }

        foreach (var arg in args) {
            if (IsBuiltinCommand(arg)) {
                Console.WriteLine($"{arg} is a shell builtin.");
                continue;
            }

            Console.WriteLine(TryFindInPath(arg, out var filePath) ? $"{arg} is {filePath}" : $"{arg} not found");
        }
    }

    private static bool IsBuiltinCommand(string command) {
        return Program.BuiltinCommands.ContainsKey(command);
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
}