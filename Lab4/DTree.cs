using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class DTree
    {
        public Node root;

        public DSets dataSplit(DataSet dataSet, int col, string value)
        {
            DSets result = new DSets();

            double val = 0;

            if(double.TryParse(value, System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out val))
            {
                foreach(List<string> st in dataSet.data)
                {
                    if (double.Parse(st[col], CultureInfo.InvariantCulture) >= val)
                        result.set1.AddRow(st);
                    else
                        result.set2.AddRow(st);
                }
            }
            else
            {
                foreach(List<string> st in dataSet.data)
                {
                    if (st[col] == value)
                        result.set1.AddRow(st);
                    else
                        result.set2.AddRow(st);
                }
            }

            result.set1.targetColumn = result.set2.targetColumn = dataSet.targetColumn;

            return result;
        }

        public void treeBuilder(DataSet dataSet)
        {
            Stack<Node> stack = new Stack<Node>();

            root = new Node();
            root.results= dataSet;
            stack.Push(root);

            do
            {
                Node current = stack.Pop();

                double best_gain = 0;
                string best_value = "";
                int best_col = -1;
                DSets best_sets = null;

                double currentH = current.results.enthropy();

                int count = current.results.GetColumnCount();

                for (int i = 0; i < count; i++)
                {
                    if (i == current.results.targetColumn) continue;
                    List<string> uniqueValues = current.results.GetUniqueValues(i);

                    foreach (string value in uniqueValues)
                    {
                        DSets sets = new DSets();
                        sets = dataSplit(current.results, i, value); 
                        double p = sets.set1.GetLength() / (double)current.results.GetLength();
                        double H1 = sets.set1.enthropy();
                        double H2 = sets.set2.enthropy();
                        double gain = (currentH - (p * H1)) - ((1 - p) * H2);

                        if(gain>best_gain && sets.set1.GetLength() > 0 && sets.set2.GetLength() > 0)
                        {
                            best_gain = gain;
                            best_sets = sets;
                            best_col = i;
                            best_value = value;
                        }
                    }
                }

                if(best_gain > 0)
                {
                    current.col = best_col;
                    current.value = best_value;
                    current.results = null;

                    current.tbranch = new Node();
                    current.tbranch.results = best_sets.set1;

                    current.fbranch = new Node();
                    current.fbranch.results = best_sets.set2;

                    stack.Push(current.tbranch);
                    stack.Push(current.fbranch);
                }
            }
            while(stack.Count > 0);
        }

        public string getResult(List<string> request)
        {
            Node current = root;

            while (current.results == null)
            {
                double val = 0;
                if (double.TryParse(current.value, NumberStyles.Number, CultureInfo.InvariantCulture, out val))
                {
                    if (double.Parse(request[current.col], CultureInfo.InvariantCulture) >= val)
                        current = current.tbranch;
                    else
                        current = current.fbranch;
                }
                else
                    if (request[current.col] == current.value)
                        current = current.tbranch;
                else
                    current = current.fbranch;

            }

            return current.results.data[0][current.results.targetColumn];

        }

        

    }
}
