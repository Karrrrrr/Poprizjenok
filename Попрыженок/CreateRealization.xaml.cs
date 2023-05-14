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
	/// Логика взаимодействия для CreateRealization.xaml
	/// </summary>
	public partial class CreateRealization : Window
	{
		public CreateRealization()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (nameTB.Text != "")
			{
				int price = 0;
				if (int.TryParse(priceTB.Text, out price))
				{
					if (price > 0)
					{
						int count = 0;
						if (int.TryParse(countTB.Text, out count))
						{
							if (count > 0)
							{
								if (datePicker.SelectedDate != null)
								{
									Realization realization = new Realization() { Product = nameTB.Text + " " + price * count, Count = count, Date = (DateTime)datePicker.SelectedDate  };
									DatabaseConnection.CreateRealization(realization);
									Close();
								}
								else
								{
									MessageBox.Show("Выберите дату");
								}
							}
							else
							{
								MessageBox.Show("Количество дожно быть больше 0");
							}
						}
						else
						{
							MessageBox.Show("Количество должно быть числом");
						}
					}
					else
					{
						MessageBox.Show("Цена должна быть больше 0");
					}
				}
				else
				{
					MessageBox.Show("Цена должна быть числом");
				}
			}
			else
			{
				MessageBox.Show("Введите название товара");
			}
        }
    }
}
