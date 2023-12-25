using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bdkurs
{
    public partial class AddEmployes : Form
    {
        DB db = new DB();
        public AddEmployes()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void Insert(byte[] image)
        {
            string addQuery = $"insert into employes (fio, jobtitle, photoemploye) values (@fio, @jobtitle, @photoemploye)";
            db.openConnection();
            using (SqlCommand cmd = new SqlCommand(addQuery, db.getConnection()))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@fio", textBoxFIOEmploye.Text);
                cmd.Parameters.AddWithValue("@jobtitle", textBoxJTEmploye.Text);
                cmd.Parameters.AddWithValue("@photoemploye", image);
                cmd.ExecuteNonQuery();
            }
            db.closeConnection();
        }

        byte[] ConvertImageToBytes(Image img)
        {
            if (img == null)
            {

                return null;
            }
            using(MemoryStream ms = new MemoryStream())
            {
                
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                return ms.ToArray();
             
            }
        }

        public Image ConvertByteArrayToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            db.openConnection();
            try
            {
            Insert(ConvertImageToBytes(pictureBoxPhotoEmploye.Image));
                MessageBox.Show("Запись успешно добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Загрузите фотографию!");
            }
            
            db.closeConnection();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter= "Image files(*.jpg;*.jpeg)|*.jpg;.jpeg", Multiselect = false }) 
            {
                if(ofd.ShowDialog()==DialogResult.OK)
                {
                    pictureBoxPhotoEmploye.Image = Image.FromFile(ofd.FileName);
                    textBoxFileName.Text = ofd.FileName;
                    //Insert(textBoxFIOEmploye.Text, textBoxJTEmploye.Text, ConvertImageToBytes(pictureBoxPhotoEmploye.Image));
                }
            }
        }
    }
}
