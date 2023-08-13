using System;
using MinecraftClient.Scripting;
using MinecraftClient.ChatBots;
using MinecraftClient.CustomBots.BotUtils;
using System.Linq;
using MinecraftClient.Commands;
using System.Collections.Generic;

namespace MinecraftClient.CustomBots
{
    class BotManager : ChatBot
    {
        private AFKBot[] AvailableBots = {
            new Bots.ExampleChatBot(), 
            new Bots.RaidFarmBot(), 
            new Bots.WitherSkeletonFarmBot(),
            new Bots.SpiderFarmBot(),
        };

        private bool locked = false;
        private DateTime? lockedTime;
        private AFKBot? runningBot;
        private string[] BotOwners = {"Toby", "Jamie"};
        private ChatUtils printing = new();

        public override void Initialize()
        {
            LoadBot(printing);
            LogToConsole("AFK bot manager initialized!");
        }

        // This is a function that will be run when we get a chat message from a server
        // In this example it just detects the type of the message and prints it out
        public override void GetText(string text)
        {
            ChatUtils.ChatType type = ChatUtils.ChatType.none;
            string player = "";
            string message = "";

            ChatUtils.CleanText(text, ref type, ref player, ref message);

            if (type == ChatUtils.ChatType.general && !message.StartsWith("~")) { return; }
            else if (type == ChatUtils.ChatType.teleport && BotOwners.Contains(player))
            {
                if (runningBot != null)
                {
                    printing.PrintError("Bot is currently AFK, stop it before teleporting", type, player);
                    return;
                }
                printing.PrintSuccess("Accepting teleport", type, player);
                SendText("/tpyes");
                return;
            } else if (type == ChatUtils.ChatType.server || type == ChatUtils.ChatType.teleport) { return; }

            if (message.StartsWith("~")) { message = message.Substring(1); }

            LogToConsole($"Recieved {message} from {player} in a {type} message.");

            if (locked && lockedTime < DateTime.UtcNow.AddHours(3) && !BotOwners.Contains(player)) 
            {
                printing.PrintError("Bot is currently locked at afk position", type, player);
                return;
            } else if (locked && lockedTime >= DateTime.UtcNow.AddHours(3)) 
            {
                locked = false;
                lockedTime = null;
            }

            if (message.StartsWith("start-"))
            {
                string botName = message.Split("-")[1];
                CreateBot(botName, player, type);
            } else if (message == "stop")
            {
                if (runningBot != null) { StopBot(runningBot, player, type); }
            } else if (message == "quit" && BotOwners.Contains(player))
            {
                PerformInternalCommand("quit");
            } else if (message == "lock" && BotOwners.Contains(player))
            {
                locked = true;
                lockedTime = DateTime.UtcNow;
            } else if (message == "unlock" && BotOwners.Contains(player))
            {
                locked = false;
                lockedTime = null;
            } else if (message == "farms")
            {
                List<string> botList = new();

                foreach(AFKBot bot in AvailableBots)
                {
                    botList.Add(bot.Name);
                }
                LogToConsole(string.Join(", ", botList));

                printing.PrintInfo(string.Join(", ", botList), type, player);
            } else
            {
                printing.PrintError("Command not known", type, player);
            }
        }

        void CreateBot(string botName, string player, ChatUtils.ChatType type)
        {
            // Stop running bot
            if (runningBot != null) { StopBot(runningBot, player, type); }
            SendText("/home chest");

            // Dump resources

            LogToConsole(botName);

            foreach(AFKBot bot in AvailableBots) 
            { 
                if (bot.Name.ToLower() == botName.ToLower())
                {
                    printing.PrintSuccess($"Starting AFK session at {bot.Name}", type, player);
                    //CollectResources(bot.NeededItems)
                    BotLoad(bot);
                    bot.Start();
                    runningBot = bot;
                }
            }
        }

        void StopBot(AFKBot bot, string player, ChatUtils.ChatType type) 
        {
            if (runningBot != bot) { return; }

            bot.Stop();

            printing.PrintInfo($"Stopping {bot.Name}", type, player);
            runningBot = null;
        }
    }
}