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
    public partial class AddRepairForm : Form
    {


        DB db = new DB();

        private void LoadComboBoxClients()
        {
            db.openConnection();

            string query = "select * from clients";
            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
            command.CommandType = CommandType.Text;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            comboBoxClient.DisplayMember = "fio";
            comboBoxClient.ValueMember = "id";
            comboBoxClient.DataSource = table;
            }

            db.closeConnection();
        }
        private void LoadComboBoxDevices()
        {
            string query = "select * from devices";
            SqlCommand command = new SqlCommand(query, db.getConnection());

            db.openConnection();

            command.CommandType = CommandType.Text;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            comboBoxDevice.DisplayMember = "duedate";
            comboBoxDevice.ValueMember = "iddevice";
            comboBoxDevice.DataSource = table;

            db.closeConnection();
        }
        private void LoadComboBoxServices()
        {
            string query = "select * from service";
            SqlCommand command = new SqlCommand(query, db.getConnection());

            db.openConnection();

            command.CommandType = CommandType.Text;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            comboBoxService.DisplayMember = "nameservice";
            comboBoxService.ValueMember = "idservice";
            comboBoxService.DataSource = table;

            db.closeConnection();
        }

        private void LoadComboBoxEmployes()
        {
            string query = "select * from employes";
            SqlCommand command = new SqlCommand(query, db.getConnection());

            db.openConnection();

            command.CommandType = CommandType.Text;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            comboBoxEmploye.DisplayMember = "fio";
            comboBoxEmploye.ValueMember = "idemploye";
            comboBoxEmploye.DataSource = table;

            db.closeConnection();
        }

        private void LoadComboBoxSerialNum()
        {
            var duedate = comboBoxDevice.Text;
            string queryDevices = $"select * from devices where duedate = '{duedate}'";
            SqlCommand commandDevices = new SqlCommand(queryDevices, db.getConnection());

            db.openConnection();

            commandDevices.CommandType = CommandType.Text;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(commandDevices);
            adapter.Fill(table);
            comboBoxSerialNum.DisplayMember = "serialnumber";
            comboBoxSerialNum.ValueMember = "iddevice";
            comboBoxSerialNum.DataSource = table;

            db.closeConnection();
        }

        private void LoadComboBoxPrice()
        {
            var nameservice = comboBoxService.Text;
            string queryDevices = $"select * from service where nameservice = '{nameservice}'";
            SqlCommand commandDevices = new SqlCommand(queryDevices, db.getConnection());

            db.openConnection();

            commandDevices.CommandType = CommandType.Text;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(commandDevices);
            adapter.Fill(table);

            
            comboBoxPrice.DisplayMember = "price";
            comboBoxPrice.ValueMember = "idservice";
            comboBoxPrice.DataSource = table;

            db.closeConnection();

        }

        public AddRepairForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void AddRepairForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxClients();
            LoadComboBoxDevices();
            LoadComboBoxServices();
            LoadComboBoxEmployes();
            
            


        }

        private void comboBoxDevice_DisplayMemberChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxService_TextChanged(object sender, EventArgs e)
        {
            LoadComboBoxPrice();
        }

        private void comboBoxDevice_TextChanged(object sender, EventArgs e)
        {
            LoadComboBoxSerialNum();
        }  
        private void buttonSave_Click(object sender, EventArgs e)
        {
            db.openConnection();
            string fioclient = comboBoxClient.Text;
            string duedate = comboBoxDevice.Text;  
            string nameservice = comboBoxService.Text;
            string fioemploye = comboBoxEmploye.Text;   
            string serialnumber = comboBoxSerialNum.Text;
            int price = int.Parse(comboBoxPrice.Text);
            DateTime daterepair = dateTimePicker.Value;

            string addQuery = $"insert into repair (fioclient, duedate, nameservice, fioemploye, serialnumber, price, daterepair) values ('{fioclient}', '{duedate}', '{nameservice}', '{fioemploye}', '{serialnumber}', '{price}', '{daterepair}')";
            SqlCommand command = new SqlCommand(addQuery, db.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно добавлена!");

            db.closeConnection();
        }
    }     
}




    

