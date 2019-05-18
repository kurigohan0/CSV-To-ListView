using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Binding = System.Windows.Data.Binding;
using System.IO;
using MessageBox = System.Windows.MessageBox;
using Button = System.Windows.Controls.Button;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private char delimiter;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            try
            {
                if (button.Tag.ToString() == "BrowseButton")
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
                    openFileDialog1.Filter = "CSV files (*.csv)|*.CSV";
                    openFileDialog1.FilterIndex = 2;
                    openFileDialog1.ShowDialog();
                    textBox1.Text = openFileDialog1.FileName;
                }
                else
                {
                    MainListView.Items.Clear();

                    delimiter = char.Parse(delimiter_box.Text);
                    string fileRow;
                    string[] fileDataField;
                    List<string> list = new List<string>();

                    var gridView = new GridView();
                    System.IO.StreamReader fileReader = new StreamReader(textBox1.Text);
                    while (fileReader.Peek() != -1)
                    {
                        fileRow = fileReader.ReadLine();
                        fileDataField = fileRow.Split(delimiter);

                        list = fileDataField.ToList<string>();

                        for (int i = 0; i < list.Count; i++)
                        {
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
        }
    }
}
