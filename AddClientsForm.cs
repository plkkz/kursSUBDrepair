using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace bdkurs
{
    public partial class AddClientsForm : Form
    {
        DB db = new DB();   
        public AddClientsForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            db.openConnection();
            string fio = textBoxfio.Text;
            string phone = textBoxphone.Text;

            string addQuery = $"insert into clients (fio, phone) values ('{fio}', '{phone}')";
            SqlCommand command = new SqlCommand(addQuery, db.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно добавлена!");

            db.closeConnection();
        }
    }
}
