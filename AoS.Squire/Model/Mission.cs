﻿namespace AoS.Squire.Model;

public class Mission
{
    public int Table { get; set; }
    public int BattlePlan { get; set; }
    public string Name { get; set; }
    public List<VictoryPoint> VictoryPoints { get; set; }

}