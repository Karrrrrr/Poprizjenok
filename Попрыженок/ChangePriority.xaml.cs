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

namespace Попрыженок
{
	/// <summary>
	/// Логика взаимодействия для ChangePriority.xaml
	/// </summary>
	public partial class ChangePriority : Window
	{
		public ChangePriority()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			int priority = 0;
			if (int.TryParse(priorityTB.Text, out priority))
			{
				if (priority > 0)
				{
					DatabaseConnection.ChangePriority(priority);
					Close();
				}
				else
				{
					MessageBox.Show("Приоритет должен быть больше 0");
				}
			}
			else
			{
				MessageBox.Show("Введите число");
			}
        }
    }
}
