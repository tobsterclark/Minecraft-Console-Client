using MinecraftClient.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClient.CustomBots.BotUtils
{
    abstract public class AFKBot : ChatBot
    {
        public abstract string Name { get; }
        public abstract string[] NeededItems { get; }


        public abstract void Start();

        public abstract void Stop();

    }

}
