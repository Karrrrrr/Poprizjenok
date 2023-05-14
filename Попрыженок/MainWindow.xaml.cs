using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Попрыженок.Models;

namespace Попрыженок
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<Agent> currentAgents;
		int pagesCount = 0;
		int currentPage = 0;
		List<Agent> sortedAgents;
		List<Agent> filteredAgents;
		List<Agent> searchedAgents;
		public static List<Agent> highlighted = new List<Agent>();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (Visibility == Visibility.Visible)
			{
				currentAgents = DatabaseConnection.GetAgents();
				sortedAgents = currentAgents.ToList();
				filteredAgents = currentAgents.ToList();
				searchedAgents = currentAgents.ToList();
				PagesCheck(currentAgents);
			}
		}

		private void PagesCheck(List<Agent> agents)
		{
			pagesCount = agents.Count / 10;
			if (agents.Count < 11)
			{
				pagesPanel.Children.Clear();
				LoadAgents(agents);
			}
			else
			{
				currentPage = 1;
				LoadAgents(agents.GetRange(0, 10));
				LoadPagesNumbers(pagesCount, 1);
			}
		}

		private void LoadAgents(List<Agent> agents)
		{
			agentsPanel.Children.Clear();
			foreach (Agent agent in agents)
			{
				bool isHighlighted = false;
				StackPanel agentPanel = new StackPanel() { Orientation = Orientation.Horizontal };
				if (highlighted.Find(x => x.Id == agent.Id) != null)
				{
					agentPanel.Background = new SolidColorBrush(Colors.LightGray);
					isHighlighted = true;
				}

				Image image = new Image() { Source = new BitmapImage(new Uri(@"D:\\Документы\\Шарага\\Демо\\Попрыженок\\DEMO2021\\Session2\\picture.png")), Width = 100, Height = 100 };

				StackPanel inf = new StackPanel() { Margin = new Thickness(10), Width = 300 };
				TextBlock typeName = new TextBlock() { Text = agent.AgentType + " | " + agent.CompanyName, FontSize = 14 };
				TextBlock sales = new TextBlock() { Text = DatabaseConnection.GetSales(agent).ToString() + " продаж за год" };
				TextBlock phone = new TextBlock() { Text = agent.Phone };
				TextBlock priority = new TextBlock() { Text = "Приоритетность: " + agent.Priority };
				inf.Children.Add(typeName);
				inf.Children.Add(sales);
				inf.Children.Add(phone);
				inf.Children.Add(priority);

				int discount = DatabaseConnection.GetDiscount(agent);
				TextBlock discountTB = new TextBlock() { Text = discount.ToString() + "%", FontSize = 20, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
				if (discount >= 25)
				{
					discountTB.Background = new SolidColorBrush(Colors.LightGreen);
				}

				agentPanel.Children.Add(image);
				agentPanel.Children.Add(inf);
				agentPanel.Children.Add(discountTB);

				ContextMenu cm = new ContextMenu();
				MenuItem highlight = new MenuItem();
				if (isHighlighted)
				{
					highlight.Header = "Убрать выделение";
				}
				else
				{
					highlight.Header = "Выделить";
				}
				cm.Items.Add(highlight);
				agentPanel.ContextMenu = cm;

				highlight.Click += (o, e) =>
				{
					if (isHighlighted)
					{
						agentPanel.Background = new SolidColorBrush(Colors.White);
						isHighlighted = false;
						highlight.Header = "Выделить";
						highlighted.Remove(agent);
						if (highlighted.Count == 0)
						{
							changePriorityButton.Visibility = Visibility.Hidden;
						}
					}
					else
					{
						agentPanel.Background = new SolidColorBrush(Colors.LightGray);
						isHighlighted = true;
						highlight.Header = "Убрать выделение";
						highlighted.Add(agent);
						if (highlighted.Count == 1)
						{
							changePriorityButton.Visibility = Visibility.Visible;
						}
					}
				};

				agentPanel.MouseLeftButtonUp += (o, e) =>
				{
					CreateEditAgent.isCreate = false;
					CreateEditAgent.currentAgent = agent;
					CreateEditAgent cea = new CreateEditAgent();
					cea.ShowDialog();
				};

				agentsPanel.Children.Add(agentPanel);
			}
		}

		private void LoadPagesNumbers(int count, int current)
		{
			pagesPanel.Children.Clear();
			TextBlock back = new TextBlock() { Text = "<", Margin = new Thickness(2) };
			if (current != 1)
			{
				back.MouseUp += backClick;
			}
			pagesPanel.Children.Add(back);
			for (int i = 1; i <= count; i++)
			{
				TextBlock page = new TextBlock() { Text = i.ToString(), Margin = new Thickness(2) };
				if (i == current)
				{
					page.TextDecorations = TextDecorations.Underline;
				}
				else
				{
					page.MouseUp += pageClick;
				}
				pagesPanel.Children.Add(page);
			}
			TextBlock forward = new TextBlock() { Text = ">", Margin = new Thickness(2) };
			if (current != count)
			{
				forward.MouseUp += forwardClick;
			}
			pagesPanel.Children.Add(forward);
		}

		private void forwardClick(object sender, MouseButtonEventArgs e)
		{
			currentPage++;
			LoadAgents(sortedAgents.GetRange(10 * (currentPage - 1), 10));
			LoadPagesNumbers(pagesCount, currentPage);
		}

		private void pageClick(object sender, MouseButtonEventArgs e)
		{
			currentPage = int.Parse((sender as TextBlock).Text);
			LoadAgents(sortedAgents.GetRange(10 * (currentPage - 1), 10));
			LoadPagesNumbers(pagesCount, currentPage);
		}

		private void backClick(object sender, MouseButtonEventArgs e)
		{
			currentPage--;
			LoadAgents(sortedAgents.GetRange(10 * (currentPage - 1), 10));
			LoadPagesNumbers(pagesCount, currentPage);
		}

		private void sortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (currentAgents != null)
			{
				Sort();
			}
		}

		private void Sort()
		{
			if (sortCB.SelectedIndex == 0)
			{
				sortedAgents = filteredAgents.ToList();
			}
			else if (sortCB.SelectedIndex == 1)
			{
				sortedAgents = filteredAgents.OrderBy(x => x.CompanyName).ToList();
			}
			else if (sortCB.SelectedIndex == 2)
			{
				sortedAgents = filteredAgents.OrderByDescending(x => x.CompanyName).ToList();
			}
			else if (sortCB.SelectedIndex == 3)
			{
				Dictionary<Agent, int> agentDiscount = DatabaseConnection.GetAgentsDiscount(filteredAgents);
				sortedAgents = agentDiscount.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
			}
			else if (sortCB.SelectedIndex == 4)
			{
				Dictionary<Agent, int> agentDiscount = DatabaseConnection.GetAgentsDiscount(filteredAgents);
				sortedAgents = agentDiscount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
			}
			else if (sortCB.SelectedIndex == 5)
			{
				sortedAgents = filteredAgents.OrderBy(x => x.Priority).ToList();
			}
			else
			{
				sortedAgents = filteredAgents.OrderByDescending(x => x.Priority).ToList();
			}
			PagesCheck(sortedAgents);
		}

		private void filterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (currentAgents != null)
			{
				Filter();
			}
		}

		private void Filter()
		{
			if (filterCB.SelectedIndex == 0)
			{
				filteredAgents = searchedAgents.ToList();
			}
			else
			{
				filteredAgents = new List<Agent>();
				foreach (Agent agent in searchedAgents)
				{
					if ((filterCB.SelectedIndex == 1) && (agent.AgentType == "МКК") || (filterCB.SelectedIndex == 2) && (agent.AgentType == "ОАО") || (filterCB.SelectedIndex == 3) && (agent.AgentType == "ООО") || (filterCB.SelectedIndex == 4) && (agent.AgentType == "ЗАО") || (filterCB.SelectedIndex == 5) && (agent.AgentType == "МФО") || (filterCB.SelectedIndex == 6) && (agent.AgentType == "ПАО"))
					{
						filteredAgents.Add(agent);
					}
				}
			}
			Sort();
		}

		private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (currentAgents != null)
			{
				Search();
			}
		}

		private void Search()
		{
			if ((searchTB.Text.Trim() == "") || (searchTB.Text == "Введите для поиска"))
			{
				searchedAgents = currentAgents.ToList();
				Filter();
			}
			else
			{
				searchedAgents = new List<Agent>();
				foreach (Agent agent in currentAgents)
				{
					if (agent.CompanyName.Contains(searchTB.Text) || agent.Phone.Contains(searchTB.Text) || agent.Email.Contains(searchTB.Text))
					{
						searchedAgents.Add(agent);
					}
				}
				Filter();
			}
		}

		private void changePriorityButton_Click(object sender, RoutedEventArgs e)
		{
			ChangePriority cp = new ChangePriority();
			cp.ShowDialog();
		}

		private void Window_Activated(object sender, EventArgs e)
		{
			if (currentAgents != null)
			{
				currentAgents = DatabaseConnection.GetAgents();
				Search();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			CreateEditAgent.isCreate = true;
			CreateEditAgent cea = new CreateEditAgent();
			cea.ShowDialog();
        }
    }
}
