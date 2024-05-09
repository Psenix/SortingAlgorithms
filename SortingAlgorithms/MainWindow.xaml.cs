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
                        Task task = new Task(visualizeSorting, toSort[j]);
                        task.Start();
                    }
                }
            }
            //foreach (Rectangle r in visualValues)
            //{
            //    Grid.Children.Remove(r);
            //}
            //for (int i = 0; i < toSort.Count; i++)
            //{
            //    Rectangle rectangle = new Rectangle()
            //    {
            //        Fill = Brushes.White,
            //        VerticalAlignment = VerticalAlignment.Stretch,
            //        Uid = "Rectangle_" + (toSort[i])
            //    };
            //    Grid.Children.Add(rectangle);
            //    Grid.SetColumn(rectangle, i);
            //    Grid.SetRow(rectangle, toSort.Count - toSort[i]);
            //    Grid.SetRowSpan(rectangle, toSort[i] + 1);
            //}
        }

        private void insertionSort(List<int> toSort)
        {
            for(int i = 1; i < toSort.Count; i++)
            {
                int j = i - 1;
                while(j > -1 && toSort[j] > toSort[i])
                {
                    int saveVal = toSort[j];
                    toSort[j] = toSort[i];
                    toSort[i] = saveVal;
                    Task task = new Task(visualizeSorting, toSort[j]);
                    task.Start();
                    j--;
                }
            }
        }

        private void selectionSort(List<int> toSort)
        {
            for(int i = 0; i < toSort.Count; i++)
            {
                int min = i;
                for(int j = i + 1; j < toSort.Count; j++)
                {
                    if (toSort[j] < toSort[min])
                    {
                        min = j;
                    }
                }
                if(min != i)
                {
                    int saveVal = toSort[i];
                    toSort[i] = toSort[min];
                    toSort[min] = saveVal;
                    Task task = new Task(visualizeSorting, toSort[i]);
                    task.Start();
                }
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

        public async void visualizeSorting(object _num)
        {
            await visualizer((int)_num);
            await visualizer((int)_num + 1);
        }

        private async Task visualizer(int _num)
        {
            try
            {
                int num = (int)_num;
                Rectangle rectangle = null;

                await Task.Delay(2000);
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    rectangle = visualValues.Where(item => item.Uid == "Rectangle_" + num).FirstOrDefault();
                    rectangle.Fill = Brushes.Red;
                    Grid.SetColumn(rectangle, num - 1);
                }));
                await Dispatcher.BeginInvoke(new Action(() => rectangle.Fill = Brushes.White));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetVariables()
        {
            Grid.Children.Clear();
            Grid.RowDefinitions.Clear();
            Grid.ColumnDefinitions.Clear();
            toSort.Clear();
            visualValues.Clear();
        }


        private void sort_Click(object sender, RoutedEventArgs e)
        {
            sortBtn.IsEnabled = false;
            ResetVariables();
            string selectedAlgorithm = SelectAlgorithm.Text;
            ScrambleData();
            createVisuals();

            switch(selectedAlgorithm)
            {
                case "Bubble Sort":
                    MessageBox.Show("Will now be sorted (Bubble)");
                    BubbleSort(toSort);
                    break;
                case "Insertion Sort":
                    MessageBox.Show("Will now be sorted (Insertion)");
                    insertionSort(toSort);
                    break;
                case "Selection Sort":
                    MessageBox.Show("Will now be sorted (Selection)");
                    selectionSort(toSort);
                    break;
            }

            sortBtn.IsEnabled = true;
        }
    }
}
