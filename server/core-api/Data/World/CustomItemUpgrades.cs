﻿namespace server.Data.World
{
    public class CustomItemUpgrades
    {
        public int Id { get; set; }
        public byte StatType { get; set; }
        public int StatValue { get; set; }
        public byte StatLevel { get; set; }
        public int RequiredLevel { get; set; }
        public int ConditionId { get; set; }
        public string AllowableInvTypes { get; set; }
    }
}
