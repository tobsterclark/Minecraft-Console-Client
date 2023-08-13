using System;
using System.Collections.Generic;
using MinecraftClient.Mapping;
using MinecraftClient.Scripting;
using MinecraftClient.ChatBots;
using Tomlet.Attributes;
using MinecraftClient.CustomBots.BotUtils;

namespace MinecraftClient.CustomBots.Bots
{
    class ExampleChatBot : AFKBot
    {
        public override string Name => "Example";
        public override string[] NeededItems => throw new NotImplementedException();

        // This method will be called when the script has been initialized for the first time, it's called only once
        // Here you can initialize variables, eg. Dictionaries. etc...
        public override void Initialize()
        {
            LogToConsole("An example Chat Bot has been initialized!");
        }

        // This is a function that will be run when we get a chat message from a server
        // In this example it just detects the type of the message and prints it out
        public override void GetText(string text)
        {
            string message = "";
            string username = "";
            text = GetVerbatim(text);

            if (IsPrivateMessage(text, ref message, ref username))
            {
                LogToConsole(username + " has sent you a private message: " + message);
            }
            else if (IsChatMessage(text, ref message, ref username))
            {
                LogToConsole(username + " has said: " + message);
            }
        }

        public override void Start()
        {
            LoadBot(this);
        }

        public override void Stop()
        {
            UnloadBot();
        }
    }
}
