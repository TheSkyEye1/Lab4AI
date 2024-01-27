using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace Lab4
{
    public class DataSet
    {
        public List<List<string>> data;
        public int targetColumn = -1;
        public List<bool> columnType = new List<bool>();
        public List<int> ignoreList = new List<int>();

        public void ignore(int id)
        {
            ignoreList.Add(id);
        }

        public DataSet()
        {
            data = new List<List<string>>();
        }

        public void AddRow(List<string> row)
        {
            data.Add(row);
        }

        public void ClassifyColumns()
        {
            for(int i = 0; i<GetColumnCount(); i++)
            {
                bool st = false;
                double o = 0;

                foreach(List<string> row in data)
                {
                    if (!double.TryParse(row[i], NumberStyles.Number, CultureInfo.InvariantCulture, out o))
                    {
                        st = true;
                        break;
                    }
                }

                columnType.Add(st);
            }
        }

        public Dictionary<string,int> valueCounter()
        {
            Dictionary<string,int> result = new Dictionary<string, int> ();

            foreach (List<string> row in data)
            {
                if (result.ContainsKey(row[targetColumn]))
                    result[row[targetColumn]]++;
                else
                    result.Add(row[targetColumn], 1);
            }
            return result;
        }

        public double enthropy()
        {
            double H = 0;

            Dictionary<string, int> vc = valueCounter();

            for(int i = 0; i < vc.Count; i++)
            {
                double p = vc.ElementAt(i).Value / (double)data.Count;
                H = H - (p * Math.Log(p, 2));
            }

            return H;
        }

        public List<string> GetUniqueValues(int column)
        {
            List<string> uniques = new List<string>();

            foreach(List<string> row in data)
            {
                if (!uniques.Contains(row[column])) uniques.Add(row[column]);
            }

            return uniques;
        }

        public int GetColumnCount()
        {
            return data[0].Count;
        }

        public int GetLength()
        {
            return data.Count;
        }

        public void delNull()
        {
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    if (data[i][j] == "")
                    {
                        data[i][j] = "0";
                    }
                }
            }
        }
        public double variance()
        {
            List<double> values = new List<double>();
            double mean = 0;
            double v = 0;

            foreach (List<string> row in data)
            {
                double val;
                double.TryParse(row[targetColumn], System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out val);
                values.Add(val);
                mean += val;
            }
            mean /= values.Count;

            foreach (double d in values)
            {
                v += (d - mean) * (d - mean);
            }
            return v / values.Count;
        }
    }
}
