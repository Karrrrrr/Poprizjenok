using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Попрыженок.Context;
using Попрыженок.Models;

namespace Попрыженок
{
	internal class DatabaseConnection
	{
		public static List<Agent> GetAgents()
		{
			using (DBContext db = new DBContext())
			{
				return db.Agent.ToList();
			}
		}

		public static int GetSales(Agent agent)
		{
			int sales = 0;
			using (DBContext db = new DBContext())
			{
				foreach (Realization realization in db.Realization)
				{
					if ((realization.AgentId == agent.Id) && (realization.Date.AddYears(1) >= DateTime.Now))
					{
						sales += realization.Count;
					}
				}
			}
			return sales;
		}

		public static int GetDiscount(Agent agent)
		{
			int sales = 0;
			using (DBContext db = new DBContext())
			{
				foreach (Realization realization in db.Realization)
				{
					if (realization.AgentId == agent.Id)
					{
						sales += int.Parse(realization.Product.Split(' ').Last());
					}
				}
			}
			if (sales <= 10000)
			{
				return 0;
			}
			else if (sales <= 50000)
			{
				return 5;
			}
			else if (sales <= 150000)
			{
				return 10;
			}
			else if (sales <= 500000)
			{
				return 20;
			}
			else
			{
				return 25;
			}
		}

		public static Dictionary<Agent, int> GetAgentsDiscount()
		{
			Dictionary<Agent, int> agentsDiscount = new Dictionary<Agent, int>();
			using (DBContext db = new DBContext())
			{
				foreach (Agent agent in db.Agent)
				{
					agentsDiscount.Add(agent, GetDiscount(agent));
				}
			}
			return agentsDiscount;
		}
	}
}
