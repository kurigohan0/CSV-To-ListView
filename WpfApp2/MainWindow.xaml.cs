/* 3-1-10
В текстовом файле хранится информация в формате CSV, выведите ее в ListView. Запрещено исполь-
зовать специализированные функции для формата CSV.
Насонов Е. гр. 205
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using Binding = System.Windows.Data.Binding;
using System.IO;
using MessageBox = System.Windows.MessageBox;
using Button = System.Windows.Controls.Button;
using System.Text.RegularExpressions;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private char delimiter; //Знак, через который идет разделение

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик нажатия кнопок
        /// </summary>
        /// <param name="sender">Обьект кнопки</param>
        /// <param name="e">Информация события</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            try
            {
                if (button.Tag.ToString() == "BrowseButton") //Кнопка обзора
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog
                    {
                        InitialDirectory = Environment.CurrentDirectory,
                        Filter = "CSV files (*.csv)|*.CSV",
                        FilterIndex = 2
                    };
                    openFileDialog1.ShowDialog();
                    textBox1.Text = openFileDialog1.FileName;
                }
                else //Кнопка отображения таблицы
                {
                    MainListView.Items.Clear();

                    delimiter = char.Parse(delimiter_box.Text);
                    string fileRow;
                    string[] fileDataField;

                    var gridView = new GridView();
                    List<string> list = new List<string>();

                    System.IO.StreamReader fileReader = new StreamReader(textBox1.Text);
                    while (fileReader.Peek() != -1)
                    {

                        fileRow = fileReader.ReadLine();
                        
                        fileDataField = Regex.Split(fileRow, "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
                        list = fileDataField.ToList<string>();
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            list.Remove("");
                        }

                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i] = list[i].Trim(new char[] { '"' });
                            if (gridView.Columns.Count != list.Count)
                            {
                                this.MainListView.View = gridView;
                                gridView.Columns.Add(new GridViewColumn
                                {
                                    Header = i,
                                    DisplayMemberBinding = new Binding(".[" + i + "]") //господи это самая не очивидная вещь что я нашел
                                });
                            }
                        }
                        MainListView.Items.Add(list);
                    }
                    fileReader.Close();
                } 
            }
            catch (FormatException)
            {
                MessageBox.Show("Заполните все поля корректно.");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Выберите файл.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Выберите файл.");
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Путь до выбранного вами файла неверен.");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Файл не может быть прочитан.");
            }
        }
    }
}
