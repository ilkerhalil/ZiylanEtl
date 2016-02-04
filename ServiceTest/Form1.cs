using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceTest.ZiylanETL;

namespace ServiceTest
{
    public partial class Form1 : Form
    {
        private BindingList<DataItem> _dataSource;
        public Form1()
        {
            InitializeComponent();
            BindDataSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (MessageBox.Show("SERVİSİ TETİKLEMEK İSTİYORMUSUNUZ?", "DİKKAT", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                button1.Enabled = true;
                return;
            }
            InvokeService();
            MessageBox.Show("Servis Tetiklendi");
            button1.Enabled = true;
        }

        void InvokeService()
        {
            var selectedTables = _dataSource.Where(x => x.IsSelected).Select(x => x.ParameterValue).ToArray();
            EtlServiceClient client = new EtlServiceClient();
            var req = new EtlServiceRequest();
            req.ServiceName = "Peraport ETL Service";
            req.ServiceParameter = new Dictionary<string, object>();
            req.ServiceParameter.Add("Erdat", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            req.ServiceParameter.Add("Tables", string.Join(",", selectedTables));
            req.ServiceParameter.Add("Insert", checkBox2.Checked);
            client.StartChildService(req);
        }

        void BindDataSource()
        {
            _dataSource = new BindingList<DataItem>();
            dataGridView1.AutoGenerateColumns = false;

            string[] tableNames = new string[]
            {
                "GtZinventAsorti", "GtZinventFyt", "GtZinventHrk", "GtZinventMlz", "GtZinventStok", "GtZinventStokA",
                "GtZinventTrn", "GtZinventUy", "GtZinventTes", "GtZinventSas", "GtLfa1", "GtT134t", "GtWrfBrandsT",
                "GtT023t", "GtT001", "GtT6wst"
            };

            for (int i = 0; i < tableNames.Length; i++)
            {
                string tableName = tableNames[i];
                DataItem dataItem = new DataItem();
                dataItem.Name = tableName;
                dataItem.ParameterValue = "C" + (i + 1);
                dataItem.IsSelected = true;
                _dataSource.Add(dataItem);
            }
            dataGridView1.DataSource = _dataSource;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < _dataSource.Count; i++)
            {
                _dataSource[i].IsSelected = checkBox1.Checked;
            }
            dataGridView1.Refresh();
        }
    }

    public class DataItem
    {
        public string Name { get; set; }
        public string ParameterValue { get; set; }
        public bool IsSelected { get; set; }
    }
}
