using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SortingAlgorithms
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<int> toSort = new List<int>();
        readonly List<Rectangle> visualValues = new List<Rectangle>();
        int amount = 100;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScrambleData()
        {
            for(int i = 1; i <= amount; i++)
            {
                toSort.Add(i);
                Grid.RowDefinitions.Add(new RowDefinition());
                Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            Random random = new Random();
            toSort = toSort.OrderBy(x => random.Next()).ToList();
        }

        private void BubbleSort(List<int> toSort)
        {
            for(int i = 0; i < toSort.Count; ++i)
            {
                for(int j = 0; j < toSort.Count - i - 1; j++) 
                {
                    if (toSort[j] > toSort[j + 1])
                    {
                        int saveVal = toSort[j];
                        toSort[j] = toSort[j + 1];
                        toSort[j + 1] = saveVal;
                    }
                }
            }
            //temperory to show if sorting worked
            foreach(Rectangle r in visualValues)
            {
                Grid.Children.Remove(r);
            }
            for (int i = 0; i < toSort.Count; i++)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Fill = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Uid = "Rectangle_" + (toSort[i])
                };
                Grid.Children.Add(rectangle);
                Grid.SetColumn(rectangle, i);
                Grid.SetRow(rectangle, toSort.Count - toSort[i]);
                Grid.SetRowSpan(rectangle, toSort[i] + 1);
            }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(this.IsLoaded)
            {
                if(Input.Text.Length > 4)
                {
                    Input.TextChanged -= Input_TextChanged;
                    Input.Text = Input.Text.Substring(0, 4);
                    Input.CaretIndex = Input.Text.Length;
                    Input.TextChanged += Input_TextChanged;
                    return;
                }

                try
                {
                    if(string.IsNullOrWhiteSpace(Input.Text) || StringOnlyZero(Input.Text))
                    {
                        amount = 1;
                        Input.TextChanged -= Input_TextChanged;
                        Input.Text = "1";
                        Input.CaretIndex = Input.Text.Length;
                        Input.TextChanged += Input_TextChanged;
                    } 
                    else
                        amount= Convert.ToInt32(Input.Text);


                } 
                catch
                {
                    MessageBox.Show("Not a valid input");
                }
            }
        }

        private bool StringOnlyZero(string str)
        {
            foreach (var c in str)
            {
                if (c != '0')
                {
                    return false;
                }
            }
            return true;
        }

        public void createVisuals()
        {
            for(int i = 0; i < toSort.Count; i++)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Fill = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Uid = "Rectangle_" + (toSort[i])
                };

                visualValues.Add(rectangle);
                Grid.Children.Add(rectangle);
                Grid.SetColumn(rectangle, i);
                Grid.SetRow(rectangle, toSort.Count - toSort[i]);
                Grid.SetRowSpan(rectangle, toSort[i] + 1);
            }
        }

        private void sort_Click(object sender, RoutedEventArgs e)
        {
            sortBtn.IsEnabled = false;
            string selectedAlgorithm = SelectAlgorithm.Text;
            ScrambleData();
            createVisuals();
            MessageBox.Show("Will now be sorted");
            BubbleSort(toSort);
        }
    }
}
