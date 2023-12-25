using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bdkurs
{
    public partial class AdminForm : Form
    {
        DB db = new DB();
        public AdminForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("login", "Логин");
            dataGridView1.Columns.Add("password", "Пароль");
            dataGridView1.Columns.Add("name", "Имя");
            dataGridView1.Columns.Add("surname", "Фамилия");
            var checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "IsAdmin";
            dataGridView1.Columns.Add(checkColumn);
        }

        private void ReadSingleRow(IDataRecord record)
        {
            dataGridView1.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record. GetString(4), record.GetBoolean(5));
        }


        private void RefreshDataGrid()
        {
            dataGridView1.Rows.Clear();

            string queryString = $"select * from users";

            SqlCommand command = new SqlCommand(queryString, db.getConnection());

            db.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(reader);
            }
            reader.Close();
            db.closeConnection();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            db.openConnection();

            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            var deleteQuery = $"DELETE FROM users WHERE id = '{id}'";

            var command = new SqlCommand(deleteQuery, db.getConnection());
            command.ExecuteNonQuery();

            db.closeConnection();

            RefreshDataGrid();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            db.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                var isadmin = dataGridView1.Rows[index].Cells[5].Value.ToString();

                var changeQuery = $"UPDATE users SET isadmin = '{isadmin}' WHERE id = '{id}'";
                var command = new SqlCommand(changeQuery, db.getConnection());
                command.ExecuteNonQuery();
            }
            db.closeConnection();
            RefreshDataGrid();
        }
    }
}
