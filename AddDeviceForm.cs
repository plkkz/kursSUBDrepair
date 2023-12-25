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
    public partial class AddDeviceForm : Form
    {
        DB db = new DB();
        public AddDeviceForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            db.openConnection();
            string mark = textBoxMarkDevice.Text;
            string model = textBoxModelDevice.Text;
            string serialnumber = textBoxSeriarNumDevice.Text;
            DateTime duedate = dateTimePickerDueDate.Value;


            string addQuery = $"insert into devices (mark, model, serialnumber, duedate) values ('{mark}', '{model}', '{serialnumber}', '{duedate}')";
            SqlCommand command = new SqlCommand(addQuery, db.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно добавлена!");

            db.closeConnection();
        }
    }
}
