using System;
using Models;
using DAL;
using MySql.Data.MySqlClient; // מאפשר עבודה עם מסדי נתונים מסוג MySQL (ספריית חיבור)

class Program
{
    static void Main()
    {

        var x = new AgentDAL();
        x.AddAgent(new Agent
        {
            CodeName = "Wolf",
            RealName = "Elazar",
            Location = "Gaza",
            Status = "Active",
            MissionsCompleted = 0,
        });

        x.AddMissionCount(1, 4);

        var d = x.CountAgentsByStatus();
        foreach(var i in d)
        {
            Console.WriteLine(i.Key + " " + i.Value);
        }

        var l = x.SearchAgentsByCode("in");

        foreach(Agent i in l)
        {
            Console.WriteLine(i.Status.ToString());
        }



    }
}
