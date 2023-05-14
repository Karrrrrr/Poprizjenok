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

		public static Dictionary<Agent, int> GetAgentsDiscount(List<Agent> agents)
		{
			Dictionary<Agent, int> agentsDiscount = new Dictionary<Agent, int>();
			using (DBContext db = new DBContext())
			{
				foreach (Agent agent in db.Agent)
				{
					if (agents.Find(x => x.Id == agent.Id) != null)
					{
						agentsDiscount.Add(agent, GetDiscount(agent));
					}
				}
			}
			return agentsDiscount;
		}

		public static void ChangePriority(int priority)
		{
			using (DBContext db = new DBContext())
			{
				foreach (Agent agent in MainWindow.highlighted)
				{
					db.Agent.Find(agent.Id).Priority = priority;
				}
				db.SaveChanges();
			}
			MainWindow.highlighted = new List<Agent>();
		}

		public static List<Realization> GetRealizationHistory(Agent agent)
		{
			List<Realization> realizationHistory = new List<Realization>();
			using (DBContext db = new DBContext())
			{
				foreach (Realization realization in db.Realization)
				{
					if (realization.AgentId == agent.Id)
					{
						realizationHistory.Add(realization);
					}
				}
			}
			return realizationHistory;
		}

		public static void RemoveRealization(Realization realization)
		{
			using (DBContext db = new DBContext())
			{
				db.Realization.Remove(db.Realization.Find(realization.Id));
				db.SaveChanges();
			}
		}

		public static void CreateRealization(Realization realization)
		{
			using (DBContext db = new DBContext())
			{
				realization.Agent = db.Agent.Find(CreateEditAgent.currentAgent.Id);
				realization.AgentId = realization.Agent.Id;
				db.Realization.Add(realization);
				db.SaveChanges();
			}
		}

		public static Agent CreateAgent(Agent agent)
		{
			using (DBContext db = new DBContext())
			{
				db.Agent.Add(agent);
				db.SaveChanges();
				return agent;
			}
		}

		public static void EditAgent(Agent agent)
		{
			using (DBContext db = new DBContext())
			{
				Agent newAgent = db.Agent.Find(agent.Id);
				newAgent.CompanyName = agent.CompanyName;
				newAgent.AgentType = agent.AgentType;
				newAgent.Logo = agent.Logo;
				newAgent.Priority = agent.Priority;
				newAgent.Adress = agent.Adress;
				newAgent.Itn = agent.Itn;
				newAgent.CoIe = agent.CoIe;
				newAgent.Phone = agent.Phone;
				newAgent.Email = agent.Email;
				newAgent.DirectorName = agent.DirectorName;
				db.SaveChanges();
			}
		}

		public static void DeleteAgent(Agent agent)
		{
			using (DBContext db = new DBContext())
			{
				db.Agent.Remove(db.Agent.Find(agent.Id));
				db.SaveChanges();
			}
		}
	}
}
