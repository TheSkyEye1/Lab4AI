using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DTree tree;
        DataSet dataSet = new DataSet();
        List<string> headers;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawB_Click(object sender, RoutedEventArgs e)
        {
            DrawTree();
        }

        private void LoadB_Click(object sender, RoutedEventArgs e)
        {
            loadDataSet();
        }

        public void DrawTree()
        {
            tree = new DTree();
            tree.treeBuilder(dataSet);

            Stack<Node> stack = new Stack<Node>();
            Stack<TreeViewItem> stackTV = new Stack<TreeViewItem>();
            stack.Push(tree.root);
            TreeViewItem tvi = new TreeViewItem();
            TV.Items.Add(tvi);
            stackTV.Push(tvi);

            do
            {
                Node current = stack.Pop();
                TreeViewItem ctvi = stackTV.Pop();

                if(current.results == null)
                {
                    double o = 0;

                    if (double.TryParse(current.value, NumberStyles.Number, CultureInfo.InvariantCulture, out o))
                        ctvi.Header = headers[current.col] + " >= " + current.value;
                    else
                        ctvi.Header = headers[current.col] + " = " + current.value;

                    TreeViewItem tp = new TreeViewItem();
                    TreeViewItem fp = new TreeViewItem();

                    ctvi.Items.Add(tp);
                    ctvi.Items.Add(fp);
                    stackTV.Push(tp);
                    stackTV.Push(fp);

                    stack.Push(current.tbranch);
                    stack.Push(current.fbranch);

                }
                else
                {
                    ctvi.Header = current.results.data[0][current.results.targetColumn];
                }

            }
            while(stack.Count > 0);
        }

        public void loadDataSet()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            using (var reader = new StreamReader(dlg.FileName))
            {
                headers = reader.ReadLine().Split('\t').ToList<string>();
                dataSet.targetColumn = 9;
                List<int> ignore = new List<int>{0, 2, 3, 7};
                dataSet.ignoreList = ignore;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('\t').ToList<string>();
                    if(!values.Contains("")) dataSet.AddRow(values);
                }

                dataSet.ClassifyColumns();

                foreach (List<string> value in dataSet.data)
                {
                    string st = "";
                    foreach (string s in value) st += s + "   ";
                    LB.Items.Add(st);
                }
            }

        }

        private void DrawTF_Click(object sender, RoutedEventArgs e)
        {
            tree = new DTree();
            tree.treeBuilder(dataSet);
            tree.fuuny_function();
            Stack<Node> stack = new Stack<Node>();
            Stack<TreeViewItem> stackTV = new Stack<TreeViewItem>();
            stack.Push(tree.root);
            TreeViewItem tvi = new TreeViewItem();
            TVF.Items.Add(tvi);
            stackTV.Push(tvi);

            do
            {
                Node current = stack.Pop();
                TreeViewItem ctvi = stackTV.Pop();

                if (current.results == null)
                {
                    double o = 0;

                    if (double.TryParse(current.value, NumberStyles.Number, CultureInfo.InvariantCulture, out o))
                        ctvi.Header = headers[current.col] + " >= " + current.value;
                    else
                        ctvi.Header = headers[current.col] + " = " + current.value;

                    TreeViewItem tp = new TreeViewItem();
                    TreeViewItem fp = new TreeViewItem();

                    ctvi.Items.Add(tp);
                    ctvi.Items.Add(fp);
                    stackTV.Push(tp);
                    stackTV.Push(fp);

                    stack.Push(current.tbranch);
                    stack.Push(current.fbranch);

                }
                else
                {
                    ctvi.Header = current.results.data[0][current.results.targetColumn];
                }

            }
            while (stack.Count > 0);
        }

        private void DrawB_Copy_Click(object sender, RoutedEventArgs e)
        {
            tree = new DTree();
            tree.regressionTreeBuilder(dataSet);

            Stack<Node> stack = new Stack<Node>();
            Stack<TreeViewItem> stackTV = new Stack<TreeViewItem>();
            stack.Push(tree.root);
            TreeViewItem tvi = new TreeViewItem();
            TV.Items.Add(tvi);
            stackTV.Push(tvi);

            do
            {
                Node current = stack.Pop();
                TreeViewItem ctvi = stackTV.Pop();

                if (current.results == null)
                {
                    double o = 0;

                    if (double.TryParse(current.value, NumberStyles.Number, CultureInfo.InvariantCulture, out o))
                        ctvi.Header = headers[current.col] + " >= " + current.value;
                    else
                        ctvi.Header = headers[current.col] + " = " + current.value;

                    TreeViewItem tp = new TreeViewItem();
                    TreeViewItem fp = new TreeViewItem();

                    ctvi.Items.Add(tp);
                    ctvi.Items.Add(fp);
                    stackTV.Push(tp);
                    stackTV.Push(fp);

                    stack.Push(current.tbranch);
                    stack.Push(current.fbranch);

                }
                else
                {
                    ctvi.Header = current.results.data[0][current.results.targetColumn];
                }

            }
            while (stack.Count > 0);
        }
    }
}

    
