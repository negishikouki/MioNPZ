using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NumSharp;

namespace MioNPZ
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DataTable Source { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static DataTable LoadMioNpz(string file)
        {
            NpzDictionary<Array> dic = np.LoadMatrix_Npz(file);
            DataTable dt = new DataTable();
            for (int i = 0; i < dic["ColumnNames"].Length; i++)
            {
                dt.Columns.Add(dic["ColumnNames"].GetValue(i).ToString());
            }
            int rowCount = dic.Values.Max(r => r.Length);
            for (int i = 0; i < rowCount; i++)
            {
                DataRow row = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    string key = col.ColumnName;
                    row[key] = dic[key].GetValue(i);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //Source = LoadMioNpz(@"E:\negis\Documents\Forex\V2\SeparateCrossover\ByMonths\Data\20170102-20170131.npz");
        }

        private void MainForm_Drop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData("FileName");
            Source = LoadMioNpz(file[0]);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
            Title = file[0];
        }
    }
}
