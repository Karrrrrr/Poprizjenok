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
using System.Windows.Shapes;
using Попрыженок.Models;

namespace Попрыженок
{
	/// <summary>
	/// Логика взаимодействия для CreateEditAgent.xaml
	/// </summary>
	public partial class CreateEditAgent : Window
	{
		public static bool isCreate = true;
		public static Agent currentAgent;
		public CreateEditAgent()
		{
			InitializeComponent();
		}

		private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (isCreate)
			{
				Title = "Добавление агента";
				deleteButton.Visibility = Visibility.Hidden;
				NoHistory();
			}
			else
			{
				Title = "Изменение агента";
				deleteButton.Visibility = Visibility.Visible;

				nameTB.Text = currentAgent.CompanyName;
				typeCB.SelectedItem = currentAgent.AgentType;
				priorityTB.Text = currentAgent.Priority.ToString();
				logoTB.Text = currentAgent.Logo;
				adressTB.Text = currentAgent.Adress;
				ITNTB.Text = currentAgent.Itn;
				CoIETB.Text = currentAgent.CoIe;
				directorTB.Text = currentAgent.DirectorName;
				phoneTB.Text = currentAgent.Phone;
				emailTB.Text = currentAgent.Email;

				LoadRealization();
			}
		}

		private void NoHistory()
		{
			realizationPanel.Children.Clear();
			TextBlock noHistory = new TextBlock() { Text = "Ничего не найдено", HorizontalAlignment = HorizontalAlignment.Center };
			realizationPanel.Children.Add(noHistory);
		}

		private void LoadRealization()
		{
			List<Realization> realization = DatabaseConnection.GetRealizationHistory(currentAgent);
			if (realization.Count == 0)
			{
				NoHistory();
			}
			else
			{
				realizationPanel.Children.Clear();
				foreach (Realization r in realization)
				{
					TextBlock rel = new TextBlock() { Text = r.Product + ", количество: " + r.Count + ", дата: " + r.Date.ToString("d") + " | Удалить", HorizontalAlignment = HorizontalAlignment.Center };
					realizationPanel.Children.Add(rel);
					rel.MouseUp += (o, ev) =>
					{
						realizationPanel.Children.Remove(rel);
						DatabaseConnection.RemoveRealization(r);
						if (realizationPanel.Children.Count == 0)
						{
							NoHistory();
						}
					};
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (CheckFields())
			{
				if (isCreate)
				{
					Agent agent = new Agent() { CompanyName = nameTB.Text, AgentType = ((ComboBoxItem)typeCB.SelectedItem).Content.ToString(), Priority = int.Parse(priorityTB.Text), Logo = logoTB.Text, Adress = adressTB.Text, Itn = ITNTB.Text, CoIe = CoIETB.Text, DirectorName = directorTB.Text, Phone = phoneTB.Text, Email = emailTB.Text };
					currentAgent = DatabaseConnection.CreateAgent(agent);
				}
				CreateRealization cr = new CreateRealization();
				cr.ShowDialog();
			}
		}

		private void Window_Activated(object sender, EventArgs e)
		{
			if (currentAgent != null)
			{
				LoadRealization();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (currentAgent == null)
			{
				if (CheckFields())
				{
					Agent agent = new Agent() { CompanyName = nameTB.Text, AgentType = ((ComboBoxItem)typeCB.SelectedItem).Content.ToString(), Priority = int.Parse(priorityTB.Text), Logo = logoTB.Text, Adress = adressTB.Text, Itn = ITNTB.Text, CoIe = CoIETB.Text, DirectorName = directorTB.Text, Phone = phoneTB.Text, Email = emailTB.Text };
					DatabaseConnection.CreateAgent(agent);
					Close();
				}
			}
			else
			{
				if (CheckFields())
				{
					Agent agent = new Agent() { Id = currentAgent.Id, CompanyName = nameTB.Text, AgentType = ((ComboBoxItem)typeCB.SelectedItem).Content.ToString(), Priority = int.Parse(priorityTB.Text), Logo = logoTB.Text, Adress = adressTB.Text, Itn = ITNTB.Text, CoIe = CoIETB.Text, DirectorName = directorTB.Text, Phone = phoneTB.Text, Email = emailTB.Text };
					DatabaseConnection.EditAgent(agent);
					Close();
				}
			}
		}

		private bool CheckFields()
		{
			if (nameTB.Text != "")
			{
				int priroity = 0;
				if (int.TryParse(priorityTB.Text, out priroity))
				{
					if (priroity > 0)
					{
						if (adressTB.Text != "")
						{
							if (ITNTB.Text.All(char.IsDigit))
							{
								if (CoIETB.Text.All(char.IsDigit))
								{
									if (directorTB.Text != "")
									{
										if (phoneTB.Text != "")
										{
											if (emailTB.Text != "")
											{
												return true;
											}
											else
											{
												MessageBox.Show("Введите email");
												return false;
											}
										}
										else
										{
											MessageBox.Show("Введите телефон");
											return false;
										}	
									}
									else
									{
										MessageBox.Show("Введите имя директора");
										return false;
									}
								}
								else
								{
									MessageBox.Show("КПП должен состоять из 9 цифр");
									return false;
								}
							}
							else
							{
								MessageBox.Show("ИНН должен состоять из 12 цифр");
								return false;
							}
						}
						else
						{
							MessageBox.Show("Введите адрес");
							return false;
						}
					}
					else
					{
						MessageBox.Show("Приоритет должен быть больше 0");
						return false;
					}
				}
				else
				{
					MessageBox.Show("Приоритет должен быть числом");
					return false;
				}
			}
			else
			{
				MessageBox.Show("Введите название");
				return false;
			}
		}

		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			if ((realizationPanel.Children[0] as TextBlock).Text == "Ничего не найдено")
			{
				DatabaseConnection.DeleteAgent(currentAgent);
				Close();
			}
			else
			{
				MessageBox.Show("Нельзя удалить агента, у которого есть история реализации");
			}
		}
	}
}
