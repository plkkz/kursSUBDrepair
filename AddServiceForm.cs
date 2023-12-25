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
    public partial class AddServiceForm : Form
    {
        DB db = new DB();
        public AddServiceForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            db.openConnection();
            var nameservice = textBoxNameService.Text;
            int price;

            if(int.TryParse(textBoxPriceService.Text, out price))
            {


                string addQuery = $"insert into service (nameservice, price) values ('{nameservice}', '{price}')";
                SqlCommand command = new SqlCommand(addQuery, db.getConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запись успешно добавлена!");

                
            }
            else
            {
                MessageBox.Show("Цена должна иметь числовой формат!");
            }    
                
            db.closeConnection();
        }
    }
}
