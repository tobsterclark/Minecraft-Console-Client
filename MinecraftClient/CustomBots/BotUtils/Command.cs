using MinecraftClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClient.CustomBots.BotUtils
{
    public interface CommandTemplate
    {
        static string id = String.Empty;
        static abstract void Help(string player);
        static abstract void Run(string command, string player, AFKBot[] availableBots, ref bool locked, ref DateTime? lockTime, Action<string, string> CreateBot, Action<string, string> StartBot, Action<string, string> StopBot);
        static abstract bool Check(string command);
    }

    public class CommandHelp : CommandTemplate
    {
        private CommandHelp() { }
        private static ChatUtils printing = new();
        public const string id = "help";

        public static bool Check(string command)
        {
            if (string.IsNullOrEmpty(command)) return false;
            else if (command == "help") return true;
            else return false;
        }

        public static void Help(string player)
        {
            printing.PrintInfo("you really need help on how to use the help command?", ChatUtils.ChatType.personal, player);
        }

        public static void Run(string command, string player, AFKBot[] availableBots, ref bool locked, ref DateTime? lockTime, Action<string, string> CreateBot, Action<string, string> StartBot, Action<string, string> StopBot)
        {
            printing.PrintInfo("Available commands: \n - help\n - start-{bot}\n - create\n - stop\n - quit\n - lock \n - bots", ChatUtils.ChatType.personal, player);
        }
    }

    public class CommandStart : CommandTemplate
    {
        private CommandStart() { }
        private static ChatUtils printing = new();
        public const string id = "start_bot";

        public static bool Check(string command)
        {
            return command.StartsWith("start-");
        }

        public static void Help(string player)
        {
            printing.PrintInfo("Usage: start-{bot}\nTo see available bots use the `bots` command", ChatUtils.ChatType.personal, player);
        }

        public static void Run(string command, string player, AFKBot[] availableBots, ref bool locked, ref DateTime? lockTime, Action<string, string> CreateBot, Action<string, string> StartBot, Action<string, string> StopBot)
        {
            string botName = command.Split("-")[1];
            StartBot(botName, player);
        }
    }
}
