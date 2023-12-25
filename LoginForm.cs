using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bdkurs
{
    public partial class LoginForm : Form
    {
        DB db = new DB();
        public LoginForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen; 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            String loginUser = loginField.Text;
            String passUser = passField.Text;


            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            string quearystring = $"select id, login, password,name, surname, isadmin from users where login = '{loginUser}' and password = '{passUser}'";


            SqlCommand command = new SqlCommand(quearystring, db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                var user = new checkUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[5]));
                MessageBox.Show("Вы успешно вошли!");
                MainForm mainForm = new MainForm(user);
                this.Hide();//не закрывается прога с хайдом, без хайда висит авторизация
                mainForm.ShowDialog();
                
            }

            else
                MessageBox.Show("Такого аккаунта не существует!");
        }

        private void registerLabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }
    }
}
