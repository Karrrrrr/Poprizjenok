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
				PagesCheck(currentAgents);
			}
		}

		private void PagesCheck(List<Agent> agents)
		{
			pagesCount = agents.Count / 10;
			if (agents.Count < 11)
			{
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
				StackPanel agentPanel = new StackPanel() { Orientation = Orientation.Horizontal };
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
				if (sortCB.SelectedIndex == 0)
				{
					sortedAgents = currentAgents.ToList();
				}
				else if (sortCB.SelectedIndex == 1)
				{

					sortedAgents = currentAgents.OrderBy(x => x.CompanyName).ToList();
				}
				else if (sortCB.SelectedIndex == 2)
				{
					sortedAgents = currentAgents.OrderByDescending(x => x.CompanyName).ToList();
				}
				else if (sortCB.SelectedIndex == 3)
				{
					Dictionary<Agent, int> agentDiscount = DatabaseConnection.GetAgentsDiscount();
					sortedAgents = agentDiscount.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
				}
				else if (sortCB.SelectedIndex == 4)
				{
					Dictionary<Agent, int> agentDiscount = DatabaseConnection.GetAgentsDiscount();
					sortedAgents = agentDiscount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
				}
				else if (sortCB.SelectedIndex == 5)
				{
					sortedAgents = currentAgents.OrderBy(x => x.Priority).ToList();
				}
				else
				{
					sortedAgents = currentAgents.OrderByDescending(x => x.Priority).ToList();
				}
				PagesCheck(sortedAgents);
			}
		}
	}
}
