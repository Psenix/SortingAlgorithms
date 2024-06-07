using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        List<int> toSort = new List<int> { };
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

        private async void BubbleSort(List<int> toSort)
        {
            for(int i = 0; i < toSort.Count; ++i)
            {
                for(int j = 0; j < toSort.Count - i - 1; j++) 
                {
                    if (toSort[j] > toSort[j + 1])
                    {
                        myVisualizer(toSort[j], j + 1);
                        myVisualizer(toSort[j + 1], j);
                        await Task.Delay(1);
                        int saveVal = toSort[j];
                        toSort[j] = toSort[j + 1];
                        toSort[j + 1] = saveVal;
                        whiten(toSort[j]);
                        whiten(toSort[j + 1]);
                        //Task task = new Task(visualizeSorting, toSort[j]);
                        //task.Start();
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

        private async void insertionSort(List<int> toSort)
        {
            for(int i = 1; i < toSort.Count; i++)
            {
                int j = i - 1;
                while(j > -1 && toSort[j] > toSort[i])
                {
                    //myVisualizer(toSort[j], i);
                    //myVisualizer(toSort[i], j);
                    //await Task.Delay(1);
                    int saveVal = toSort[j];
                    toSort[j] = toSort[i];
                    toSort[i] = saveVal;
                    //whiten(toSort[j]);
                    //whiten(toSort[i]);
                    Task task = new Task(visualizeSorting, toSort[j]);
                    task.Start();
                    j--;
                }
            }
        }

        private async void selectionSort(List<int> toSort)
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

                    myVisualizer(toSort[min], i);
                    myVisualizer(toSort[i], min);
                    await Task.Delay(1);

                    int saveVal = toSort[i];
                    toSort[i] = toSort[min];
                    toSort[min] = saveVal;

                    whiten(toSort[i]);
                    whiten(toSort[min]);
                }
            }
        }

        private List<int> mergeSort(List<int> toSort)
        {
            int mid = toSort.Count / 2;

            if (toSort.Count < 2)
            {
                return toSort;
            }

            List<int> leftHalf = toSort.Take(mid).ToList();
            toSort = toSort.Skip(mid).ToList();
            return merge(mergeSort(leftHalf), mergeSort(toSort));
        }

        private List<int> merge(List<int> left, List<int> right)
        {
            List<int> merged = new List<int> { };
            while (left.Count > 0 && right.Count > 0)
            {
                if (left[0] < right[0])
                {
                        merged.AddRange(left);
                }
                else
                {
                        merged.AddRange(right);
                }
            }
            return merged;
        }

        private List<int> quickSort(List<int> toSort, int left, int right)
        {
            //int left = 0;
            //int right = toSort.Count - 1;

            if(left > right)
            {
                return null;
            }

            int partitionIndx = partition(toSort, left, right);
            quickSort(toSort, left, partitionIndx - 1);
            quickSort(toSort, partitionIndx + 1, right);
            Task task = new Task(visualizeSorting, left);
            task.Start();
            Task task2 = new Task(visualizeSorting, right);
            task2.Start();

            return toSort;
        }

        private int partition(List<int> toSort, int left, int right)
        {
            int pivotVal = toSort[right];
            int partitionIndx = left - 1;
            
            for(int i = left; i < right; i++)
            {
                if (toSort[i] <= pivotVal)
                {         
                    partitionIndx++;
                    (toSort[partitionIndx], toSort[i]) = (toSort[i], toSort[partitionIndx]);
                }
            }

            //int saveValOut = toSort[right];
            //toSort[right] = toSort[partitionIndx];
            //toSort[partitionIndx] = saveValOut;
            (toSort[right], toSort[partitionIndx + 1]) = (toSort[partitionIndx + 1], toSort[right]);
            return partitionIndx + 1;
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

        private void myVisualizer(int num, int i)
        {
            Rectangle rectangle = null;

            rectangle = visualValues.Where(item => item.Uid == "Rectangle_" + num).FirstOrDefault();
            rectangle.Fill = Brushes.Red;
            Grid.SetColumn(rectangle, i);
        }

        private void whiten(int num)
        {
            Rectangle rectangle = null;

            rectangle = visualValues.Where(item => item.Uid == "Rectangle_" + num).FirstOrDefault();
            rectangle.Fill = Brushes.White;
        }

        private void ResetVariables()
        {
            Grid.Children.Clear();
            Grid.RowDefinitions.Clear();
            Grid.ColumnDefinitions.Clear();
            toSort.Clear();
            visualValues.Clear();
        }

        private void show(List<int> toSort)
        {
            foreach (Rectangle r in visualValues)
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
                case "Merge Sort":
                    MessageBox.Show("Will now be sorted (Merge)");
                    List<int> finished = mergeSort(toSort);
                    show(finished);
                    break;
                case "Quick Sort":
                    MessageBox.Show("Will now be sorted (Quick)");
                    quickSort(toSort, 0, toSort.Count - 1);
                    break;
            }

            sortBtn.IsEnabled = true;
        }
    }
}
