using MinecraftClient.Scripting;
using System.Numerics;
using System.Text.RegularExpressions;

namespace MinecraftClient.CustomBots.BotUtils
{
    public class ChatUtils : ChatBot
    {
        public static void CleanText(string text, ref ChatType type, ref string player, ref string message)
        {
            text = GetVerbatim(text);
            Match privateRegex = Regex.Match(text, "\\[(?>\\[.*?\\])? ?(.*?) -> me\\] (.*)");

            if (IsChatMessage(text, ref message, ref player))
            {
                type = ChatType.general;
            }
            else if (privateRegex.Success)
            {
                player = privateRegex.Groups[1].Value;
                message = privateRegex.Groups[2].Value;
                type = ChatType.personal;
            }
            else if (IsTeleportRequest(text, ref player))
            {
                type = ChatType.teleport;
            }
            else
            {
                message = text;
                type = ChatType.server;
            }
        }

        public void PrintError(string err, ChatType printType, string player)
        {
            if (printType == ChatType.personal || printType == ChatType.teleport)
            {
                SendPrivateMessage(player, $"{TextColours.Red}{TextFormatting.bold}Error: {err}");
            } else {
                SendText($"{TextColours.Red}{TextFormatting.bold}Error: {err}, Player: {player}");   
            }
        }

        public void PrintSuccess(string msg, ChatType printType, string player)
        {
            if (printType == ChatType.personal || printType == ChatType.teleport)
            {
                SendPrivateMessage(player, $"{TextColours.Blue}{TextFormatting.underline}{msg}");
            }
            else
            {
                SendText($"{TextColours.Blue}{TextFormatting.underline}{msg}, Player: {player}");
            }
        }

        public void PrintInfo(string msg, ChatType printType, string player)
        {
            if (printType == ChatType.personal || printType == ChatType.teleport)
            {
                SendPrivateMessage(player, $"{TextColours.Yellow}{TextFormatting.underline}{msg}");
            } else
            {
                SendText($"{TextColours.Yellow}{TextFormatting.underline}{msg}");
            }
        }

        public enum ChatType
        {
            server,
            personal,
            general,
            teleport,
            none,
        }
    }
}
