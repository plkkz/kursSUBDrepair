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
    public partial class RegisterForm : Form
    {          
        DB db = new DB();
        public RegisterForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            userNameField.Text = "Введите имя";
            userNameField.ForeColor = Color.Gray;
            userSurnameField.Text = "Введите фамилию";
            userSurnameField.ForeColor = Color.Gray;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void passField_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (userNameField.Text == "Введите имя")
            {
                MessageBox.Show("Введите имя!");
                return;
            }
            if (userSurnameField.Text == "Введите фамилию")
            {
                MessageBox.Show("Введите фамилию!");
                return;
            }
            if (loginField.Text == "")
            {
                MessageBox.Show("Введите логин!");
                return;
                
            }
            
            if (passField.Text == "")
            {
                MessageBox.Show("Введите пароль!");
                return;
            }
            if (isUserExists())
                return;

            String login = loginField.Text;
            String password = passField.Text;
            String name = userNameField.Text;
            String surname = userSurnameField.Text;

            String querystring = $"insert into users (login, password, name, surname, isadmin) values('{login}','{password}','{name}', '{surname}' , 0)";
            SqlCommand command = new SqlCommand(querystring, db.getConnection());

            db.openConnection();



            if (command.ExecuteNonQuery() == 1)//проверка на успешность создания записи
            {
                MessageBox.Show("Аккаунт успешно создан!");
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }

            else
                MessageBox.Show("Аккаунт не был создан!");

            db.closeConnection();
        }

        public Boolean isUserExists()
        {

            String login = loginField.Text;

            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            string querystring = $"select id, login, password, name, surname, isadmin from users where login = '{login}'";

            SqlCommand command = new SqlCommand(querystring, db.getConnection());
          
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Логин уже занят, введите другой!");
            return true;
            }
            else
                return false;
        }

        private void userNameField_Enter(object sender, EventArgs e)
        {
            if (userNameField.Text == "Введите имя")
            {
                userNameField.Text = "";
                userNameField.ForeColor = Color.Black;
            }
            
        }

        private void userSurnameField_Enter(object sender, EventArgs e)
        {
            if (userSurnameField.Text == "Введите фамилию")
            {
                userSurnameField.Text = "";
                userSurnameField.ForeColor = Color.Black;
            }
        }

        private void userNameField_Leave(object sender, EventArgs e)
        {
            if (userNameField.Text == "")
            {
                userNameField.Text = "Введите имя";
                userNameField.ForeColor = Color.Gray;
            }
                

        }

        private void userSurnameField_Leave(object sender, EventArgs e)
        {
            if (userSurnameField.Text == "")
            {
                userSurnameField.Text = "Введите фамилию";
                userSurnameField.ForeColor = Color.Gray;
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}
