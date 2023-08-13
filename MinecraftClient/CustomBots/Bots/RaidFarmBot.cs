using MinecraftClient.CustomBots.BotUtils;
using MinecraftClient.ChatBots;
using System;
using MinecraftClient.Scripting;
using MinecraftClient.Mapping;

namespace MinecraftClient.CustomBots.Bots
{
    public class RaidFarmBot : AFKBot
    {
        private readonly ChatBot AutoAttackScript = new AutoAttack();

        public override string Name => "Raid";

        public override string[] NeededItems => throw new NotImplementedException();

        public override void Start()
        {
            SendText($"/home {Name}");

            AutoAttack.Config.Mode = AutoAttack.Configs.AttackMode.single;
            AutoAttack.Config.Priority = AutoAttack.Configs.PriorityType.distance;
            AutoAttack.Config.Cooldown_Time = new(true, 1.5);
            AutoAttack.Config.Attack_Range = 4.0;
            AutoAttack.Config.Attack_Hostile = true;
            AutoAttack.Config.Attack_Passive = true;
            AutoAttack.Config.List_Mode = AutoAttack.Configs.ListType.whitelist;
            AutoAttack.Config.Entites_List.Clear();
            AutoAttack.Config.Entites_List.Add(EntityType.ArmorStand);

            LoadBot(AutoAttackScript);
        }

        public override void Stop()
        {
            UnLoadBot(AutoAttackScript);
            UnloadBot();
        }
    }
}
