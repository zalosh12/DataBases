using Google.Protobuf.WellKnownTypes;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DAL
{
    public class AgentDAL
    {
        private string connectionString = "" +
            "server=localhost;" +
            "user=root;" +
            "database=eagleeyedb;" +
            "port=3306;"
            ;


        public List<Agent> SearchAgentsByCode(string partialCode){
            List<Agent> res = new List<Agent>();

            MySqlConnection conn = new MySqlConnection(connectionString);

            conn.Open();

            string query = "Select * FROM agents WHERE codeName LIKE @partialCode";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@partialCode", "%" + partialCode + "%");

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Agent agent = new Agent
                {
                    Id = reader.GetInt32("id"),
                    CodeName = reader.GetString("codeName"),
                    RealName = reader.GetString("realName"),
                    Location = reader.GetString("location"),
                    Status = reader.GetString("status"),
                    MissionsCompleted = reader.GetInt32("missionsCompleted")
                };
                res.Add(agent);
            }
            reader.Close();
            conn.Close();

            return res;

        }
        public Dictionary<string, int> CountAgentsByStatus()
        {
            Dictionary<string,int> res = new Dictionary<string,int>();

            MySqlConnection con = new MySqlConnection(connectionString);

            con.Open();

            //שולף מהטבלה כל סטטוס ומספר הסוכנים באותו סטטוס
            string query = "SELECT status,COUNT(status) AS cnt " +
                "FROM agents " +
                "GROUP BY status";

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                res.Add(reader.GetString("status"), reader.GetInt32("cnt"));
            }
            reader.Close();
            con.Close();

            return res;
        }
            

        public void AddAgent(Agent agent)
        {
            
            MySqlConnection conn = new MySqlConnection(connectionString);

        
            conn.Open();


            string query = @"INSERT INTO agents (codeName, realName, location, status, missionsCompleted)
                  VALUES (@codeName, @realName, @location, @status, @missionsCompleted)";

            
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@codeName", agent.CodeName);
            cmd.Parameters.AddWithValue("@realName", agent.RealName);
            cmd.Parameters.AddWithValue("@location", agent.Location);
            cmd.Parameters.AddWithValue("@status", agent.Status);
            cmd.Parameters.AddWithValue("@missionsCompleted", agent.MissionsCompleted);

            cmd.ExecuteNonQuery();
        }


        public List<Agent> GetAllAgents()
        {
            List<Agent> agents = new List<Agent>();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM agents";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Agent agent = new Agent
                {
                    Id = reader.GetInt32("id"),
                    CodeName = reader.GetString("codeName"),
                    RealName = reader.GetString("realName"),
                    Location = reader.GetString("location"),
                    Status = reader.GetString("status"),
                    MissionsCompleted = reader.GetInt32("missionsCompleted")
                };

                agents.Add(agent);
            }

            reader.Close();
            conn.Close();  

            return agents;
        }


        public void UpdateAgentLocation(int agentId, string newLocation)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE agents SET location = @location WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@location", newLocation);
            cmd.Parameters.AddWithValue("@id", agentId);
            cmd.ExecuteNonQuery();
        }

        public void AddMissionCount(int agentId, int missionsToAdd) 
        { 
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE agents SET missionsCompleted = missionsCompleted + @missionsToAdd WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand( query, conn);
            cmd.Parameters.AddWithValue("@missionsToAdd", missionsToAdd);
            cmd.Parameters.AddWithValue("@id", agentId);
            cmd.ExecuteNonQuery();


        }

        public void DeleteAgent(int agentId)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "DELETE FROM agents WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", agentId);
            cmd.ExecuteNonQuery();
        }






    }
}
