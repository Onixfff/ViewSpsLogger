using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewSpsLogger
{
    public partial class Form1 : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["local"].ConnectionString);
        ISqlCode SqlCode = new SqlCode();
        Autoclave autoclave = new Autoclave();

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.DataSource = Enum.GetValues(typeof(Autoclave));
        }



        public async Task GetInfo(string sql)
        {
            try
            {
                using (var con = new MySqlConnection(mCon.ConnectionString))
                {
                    await con.OpenAsync();

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var dataTable = new DataTable();
                            dataTable.Load(reader);
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { await mCon.CloseAsync(); }
        }

        private async void dateTimePicker1_ValueChanged(object sender, System.EventArgs e)
        {
            autoclave = (Autoclave)comboBox1.SelectedValue;
            await GetInfo(SqlCode.GetSqlCode(autoclave, dateTimePicker1.Value));

        }

        private async void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            autoclave = (Autoclave)comboBox1.SelectedValue;
            await GetInfo(SqlCode.GetSqlCode(autoclave, dateTimePicker1.Value));
        }
    }
}
