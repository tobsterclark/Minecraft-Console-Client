using MinecraftClient.CustomBots.BotUtils;
using MinecraftClient.ChatBots;
using System;
using MinecraftClient.Scripting;
using Microsoft.VisualBasic;
using MinecraftClient.Mapping;

namespace MinecraftClient.CustomBots.Bots
{
    public class SpiderFarmBot : AFKBot
    {
        private readonly AutoAttack AutoAttackScript = new();

        public override string Name => "Spider";

        public override string[] NeededItems => throw new NotImplementedException();

        public override void Start()
        {
            SendText($"/home {Name.ToLower()}");
            Location loc = new();

            loc.X = 8431;
            loc.Y = 212;
            loc.Z = 1390;

            LookAtLocation(loc);

            AutoAttack.Config.Mode = AutoAttack.Configs.AttackMode.single;
            AutoAttack.Config.Priority = AutoAttack.Configs.PriorityType.distance;
            AutoAttack.Config.Cooldown_Time = new(true, 2.0);
            AutoAttack.Config.Attack_Range = 4.0;
            AutoAttack.Config.Attack_Hostile = true;
            AutoAttack.Config.Attack_Passive = false;
            AutoAttack.Config.List_Mode = AutoAttack.Configs.ListType.whitelist;
            AutoAttack.Config.Entites_List.Clear();
            AutoAttack.Config.Entites_List.Add(EntityType.Spider);

            LoadBot(AutoAttackScript);
        }

        public override void Stop()
        {
            UnLoadBot(AutoAttackScript);
            UnloadBot();
        }
    }
}