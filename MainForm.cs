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
using System.IO;

namespace bdkurs
{
    enum RowState
    {
        Existed,
        Row,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class MainForm : Form
    {
        private readonly checkUser _user;
        DB db = new DB();
        int selectedRow;
        public MainForm(checkUser user)
        {
            _user = user;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            
        }

        private void isAdmin()
        {
            управлениеToolStripMenuItem.Enabled = _user.IsAdmin;
            newClient.Enabled = _user.IsAdmin;
            editClient.Enabled = _user.IsAdmin;
            deleteClient.Enabled = _user.IsAdmin;
            saveClient.Enabled = _user.IsAdmin;
            файлToolStripMenuItem.Enabled = _user.IsAdmin;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            textBoxUserStatus.Text = $"{_user.Login}: {_user.Status}";
            isAdmin();
            CreateColumnsClients();
            CreateColumnsDevices();
            CreateColumnsService();
            CreateColumnsEmploye();
            CreateColumnsRepair();
            RefreshDataGridClients(dataGridView1);
            RefreshDataGridDevices(dataGridViewDevice);
            RefreshDataGridService(dataGridViewService);
            RefreshDataGridEmploye(dataGridViewEmploye);
            RefreshDataGridRepair(dataGridViewRepair);

        }
        private void управлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.Show();
        }
        /// <summary>
        /// clients!!!
        /// </summary>
        private void CreateColumnsClients()
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("fio", "ФИО");
            dataGridView1.Columns.Add("phone", "Номер телефона");
            dataGridView1.Columns.Add("EsNew", String.Empty);
        }

        private void ClearFieldsClients()
        {
            textBoxID.Text = "";
            fiotextBox.Text = "";
            phonetextBox.Text = "";
        }


        private void ReadSingleRowClients(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), RowState.ModifiedNew);
        }
        private void RefreshDataGridClients(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from clients";

            SqlCommand command = new SqlCommand(queryString, db.getConnection());

            db.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                ReadSingleRowClients(dgw, reader);
            }
            reader.Close();
            db.closeConnection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBoxID.Text = row.Cells[0].Value.ToString();
                fiotextBox.Text = row.Cells[1].Value.ToString();
                phonetextBox.Text = row.Cells[2].Value.ToString();
            }
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from clients where concat (id, fio, phone) like '%" + textBoxSearch.Text + "%'";

            SqlCommand command = new SqlCommand(searchString, db.getConnection());

            db.openConnection();

            SqlDataReader read = command.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRowClients(dgw, read);
            }
            read.Close();
            db.closeConnection();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridClients(dataGridView1);
            ClearFieldsClients();
        }

        private void deleteRowClients()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;
            dataGridView1.Rows[index].Cells[3].Value = RowState.Deleted;

        }
        private void UpdateClients()
        {
            db.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[3].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value.ToString());
                    string deleteQuery = $"delete from clients where id = '{id}'";

                    SqlCommand command = new SqlCommand(deleteQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var fio = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var phone = dataGridView1.Rows[index].Cells[2].Value.ToString();

                    string changeQuery = $"update clients set fio = '{fio}', phone = '{phone}' where id = '{id}'";

                    SqlCommand command = new SqlCommand(changeQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            db.closeConnection();
        }
        private void ChangeClients()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = textBoxID.Text;
            var fio = fiotextBox.Text;
            var phone = phonetextBox.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, fio, phone);
                dataGridView1.Rows[selectedRowIndex].Cells[3].Value = RowState.Modified;
            }
        }


        private void newClient_Click(object sender, EventArgs e)
        {
            AddClientsForm addClientsForm = new AddClientsForm();
            addClientsForm.Show();
        }


        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }



        private void deleteClient_Click(object sender, EventArgs e)
        {
            deleteRowClients();
            ClearFieldsClients();

        }



        private void saveClient_Click(object sender, EventArgs e)
        {
            UpdateClients();
        }


        private void editClient_Click(object sender, EventArgs e)
        {
            ChangeClients();
            ClearFieldsClients();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            ClearFieldsClients();
        }




        /// <summary>
        /// Devices!!!
        /// </summary>


        private void CreateColumnsDevices()
        {
            dataGridViewDevice.Columns.Add("id", "id");
            dataGridViewDevice.Columns.Add("mark", "Марка");
            dataGridViewDevice.Columns.Add("model", "Модель");
            dataGridViewDevice.Columns.Add("serialnumber", "Серийный номер");
            dataGridViewDevice.Columns.Add("duedate", "Дата сдачи устройства");
            dataGridViewDevice.Columns.Add("EsNew", String.Empty);
        }

        private void ClearFieldsDevices()
        {
            textBoxIDDevice.Text = "";
            textBoxMarkDevice.Text = "";
            textBoxModelDevice.Text = "";
            textBoxSerialNumDevice.Text = "";
            dateTimePickerDueDate.Value = DateTime.Now;
        }


        private void ReadSingleRowDevices(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetDateTime(4), RowState.ModifiedNew);
        }
        private void RefreshDataGridDevices(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from devices";

            SqlCommand command = new SqlCommand(queryString, db.getConnection());

            db.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRowDevices(dgw, reader);
            }
            reader.Close();
            db.closeConnection();
        }


        private void ChangeDevices()
        {
            var selectedRowIndex = dataGridViewDevice.CurrentCell.RowIndex;
            var id = textBoxIDDevice.Text;
            string mark = textBoxMarkDevice.Text;
            string model = textBoxModelDevice.Text;
            string serialnumber = textBoxSerialNumDevice.Text;
            DateTime duedate = dateTimePickerDueDate.Value;

            if (dataGridViewDevice.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridViewDevice.Rows[selectedRowIndex].SetValues(id, mark, model,serialnumber, duedate);
                dataGridViewDevice.Rows[selectedRowIndex].Cells[5].Value = RowState.Modified;
            }
        }

        private void deleteRowDevice()
        {
            int index = dataGridViewDevice.CurrentCell.RowIndex;
            dataGridViewDevice.Rows[index].Visible = false;
            dataGridViewDevice.Rows[index].Cells[5].Value = RowState.Deleted;

        }

        private void UpdateDevice()
        {
            db.openConnection();

            for (int index = 0; index < dataGridViewDevice.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridViewDevice.Rows[index].Cells[5].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridViewDevice.Rows[index].Cells[0].Value.ToString());
                    string deleteQuery = $"delete from devices where iddevice = '{id}'";

                    SqlCommand command = new SqlCommand(deleteQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var iddevice = dataGridViewDevice.Rows[index].Cells[0].Value.ToString();
                    var mark = dataGridViewDevice.Rows[index].Cells[1].Value.ToString();
                    var model = dataGridViewDevice.Rows[index].Cells[2].Value.ToString();
                    var serialnumber = dataGridViewDevice.Rows[index].Cells[3].Value.ToString();
                    DateTime duedate = Convert.ToDateTime(dataGridViewDevice.Rows[index].Cells[4].Value);


                    string changeQuery = $"update devices set mark = '{mark}', model = '{model}', serialnumber = '{serialnumber}', duedate = '{duedate}'  where iddevice = '{iddevice}'";

                    SqlCommand command = new SqlCommand(changeQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            db.closeConnection();
        }

        private void SearchDevice(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from devices where concat (iddevice, mark, model, serialnumber, duedate) like '%" + textBoxSearchDevice.Text + "%'";

            SqlCommand command = new SqlCommand(searchString, db.getConnection());

            db.openConnection();

            SqlDataReader read = command.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRowDevices(dgw, read);
            }
            read.Close();
            db.closeConnection();
        }
        private void dataGridViewDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewDevice.Rows[selectedRow];
                textBoxIDDevice.Text = row.Cells[0].Value.ToString();
                textBoxMarkDevice.Text = row.Cells[1].Value.ToString();
                textBoxModelDevice.Text = row.Cells[2].Value.ToString();
                textBoxSerialNumDevice.Text = row.Cells[3].Value.ToString();
                dateTimePickerDueDate.Value = Convert.ToDateTime(row.Cells[4].Value);
            }
        }


        private void buttonNewDevice_Click(object sender, EventArgs e)
        {
            AddDeviceForm addDeviceForm = new AddDeviceForm();
            addDeviceForm.Show();
        }


        private void buttonEditDevice_Click(object sender, EventArgs e)
        {
            ChangeDevices();
            ClearFieldsDevices();
        }


        private void buttonDeleteDevice_Click(object sender, EventArgs e)
        {
            deleteRowDevice();
            ClearFieldsDevices();
        }

        private void buttonSaveDevice_Click(object sender, EventArgs e)
        {
            UpdateDevice();
        }

        private void pictureBoxRefreshDevice_Click(object sender, EventArgs e)
        {
            RefreshDataGridDevices(dataGridViewDevice);
            ClearFieldsDevices();
        }


        private void pictureBoxEracerDevice_Click(object sender, EventArgs e)
        {
            ClearFieldsDevices();
        }



        private void textBoxSearchDevice_TextChanged(object sender, EventArgs e)
        {
            SearchDevice(dataGridViewDevice);
        }


        /// <summary>
        /// УСЛУГИ!!!
        /// </summary>
        /// 

        private void CreateColumnsService()
        {
            dataGridViewService.Columns.Add("id", "id");
            dataGridViewService.Columns.Add("nameservice", "Наименование услуги");
            dataGridViewService.Columns.Add("price", "Цена");
            dataGridViewService.Columns.Add("EsNew", String.Empty);

        }

        private void ClearFieldsService()
        {
            textBoxIDService.Text = "";
            textBoxNameService.Text = "";
            textBoxCoastService.Text = "";
        }


        private void ReadSingleRowService(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), RowState.ModifiedNew);
        }
        private void RefreshDataGridService(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from service";

            SqlCommand command = new SqlCommand(queryString, db.getConnection());

            db.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRowService(dgw, reader);
            }
            reader.Close();
            db.closeConnection();
        }


        private void ChangeService()
        {
            var selectedRowIndex = dataGridViewService.CurrentCell.RowIndex;
            var id = textBoxIDService.Text;
            string nameservice = textBoxNameService.Text;
            int price;

            if (dataGridViewService.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBoxCoastService.Text, out price))
                {
                    dataGridViewService.Rows[selectedRowIndex].SetValues(id, nameservice, price);
                    dataGridViewService.Rows[selectedRowIndex].Cells[3].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Цена должна иметь числовой формат!");
                }
            }
        }

        private void deleteRowService()
        {
            int index = dataGridViewService.CurrentCell.RowIndex;
            dataGridViewService.Rows[index].Visible = false;
            dataGridViewService.Rows[index].Cells[3].Value = RowState.Deleted;

        }

        private void UpdateService()
        {
            db.openConnection();

            for (int index = 0; index < dataGridViewService.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridViewService.Rows[index].Cells[3].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridViewService.Rows[index].Cells[0].Value.ToString());
                    string deleteQuery = $"delete from service where idservice = '{id}'";

                    SqlCommand command = new SqlCommand(deleteQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idservice = dataGridViewService.Rows[index].Cells[0].Value.ToString();
                    var nameservice = dataGridViewService.Rows[index].Cells[1].Value.ToString();
                    var price = dataGridViewService.Rows[index].Cells[2].Value.ToString();



                        string changeQuery = $"update service set nameservice = '{nameservice}', price = '{price}'  where idservice = '{idservice}'";

                    SqlCommand command = new SqlCommand(changeQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            db.closeConnection();
        }

        private void SearchService(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from service where concat (idservice, nameservice, price) like '%" + textBoxSearchService.Text + "%'";

            SqlCommand command = new SqlCommand(searchString, db.getConnection());

            db.openConnection();

            SqlDataReader read = command.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRowService(dgw, read);
            }
            read.Close();
            db.closeConnection();
        }
        private void dataGridViewService_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewService.Rows[selectedRow];
                textBoxIDService.Text = row.Cells[0].Value.ToString();
                textBoxNameService.Text = row.Cells[1].Value.ToString();
                textBoxCoastService.Text = row.Cells[2].Value.ToString();

            }
        }


        private void buttonNewService_Click(object sender, EventArgs e)
        {
            AddServiceForm addServiceForm = new AddServiceForm();
            addServiceForm.Show();
        }


        private void buttonEditService_Click(object sender, EventArgs e)
        {
            ChangeService();
            ClearFieldsService();
        }


        private void buttonDeleteService_Click(object sender, EventArgs e)
        {
            deleteRowService();
            ClearFieldsService();
        }

        private void buttonSaveService_Click(object sender, EventArgs e)
        {
            UpdateService();
        }

        private void pictureBoxRefreshService_Click(object sender, EventArgs e)
        {
            RefreshDataGridService(dataGridViewService);
            ClearFieldsService();
        }


        private void pictureBoxEracerService_Click(object sender, EventArgs e)
        {
            ClearFieldsService();
        }



        private void textBoxSearchService_TextChanged(object sender, EventArgs e)
        {
            SearchService(dataGridViewService);
        }


        /// <summary>
        /// employes!!!
        /// </summary>

        private void buttonNewEmploye_Click(object sender, EventArgs e)
        {
            AddEmployes addEmployes = new AddEmployes();
            addEmployes.Show();
        }
        private void CreateColumnsEmploye()
        {
            dataGridViewEmploye.Columns.Add("id", "id");
            dataGridViewEmploye.Columns.Add("fio", "ФИО");
            dataGridViewEmploye.Columns.Add("jobtitle", "Должность");
            dataGridViewEmploye.Columns.Add("EsNew", String.Empty);

        }

        private void ClearFieldsEmploye()
        {
            textBoxIDEmploye.Text = "";
            textBoxFIOEmploye.Text = "";
            textBoxJobTitleEmploye.Text = "";
            pictureBoxPhoto.Image = null;
        }


        private void ReadSingleRowEmploye(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), RowState.ModifiedNew);
        }
        private void RefreshDataGridEmploye(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from employes";

            SqlCommand command = new SqlCommand(queryString, db.getConnection());

            db.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRowEmploye(dgw, reader);
            }
            reader.Close();
            db.closeConnection();
        }


        private void ChangeEmploye()
        {
            var selectedRowIndex = dataGridViewEmploye.CurrentCell.RowIndex;
            var id = textBoxIDEmploye.Text;
            string fio = textBoxFIOEmploye.Text;
            string jobtitle = textBoxJobTitleEmploye.Text;

            if (dataGridViewEmploye.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridViewEmploye.Rows[selectedRowIndex].SetValues(id, fio, jobtitle);
                dataGridViewEmploye.Rows[selectedRowIndex].Cells[3].Value = RowState.Modified;

            }
        }

        private void deleteRowEmploye()
        {
            int index = dataGridViewEmploye.CurrentCell.RowIndex;
            dataGridViewEmploye.Rows[index].Visible = false;
            dataGridViewEmploye.Rows[index].Cells[3].Value = RowState.Deleted;

        }

        private void UpdateEmploye()
        {
            db.openConnection();

            for (int index = 0; index < dataGridViewEmploye.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridViewEmploye.Rows[index].Cells[3].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridViewEmploye.Rows[index].Cells[0].Value.ToString());
                    string deleteQuery = $"delete from employes where idemploye = '{id}'";

                    SqlCommand command = new SqlCommand(deleteQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idemploye = dataGridViewEmploye.Rows[index].Cells[0].Value.ToString();
                    var fio = dataGridViewEmploye.Rows[index].Cells[1].Value.ToString();
                    var jobtitle = dataGridViewEmploye.Rows[index].Cells[2].Value.ToString();



                    string changeQuery = $"update employes set fio = '{fio}', jobtitle = '{jobtitle}'  where idemploye = '{idemploye}'";

                    SqlCommand command = new SqlCommand(changeQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            db.closeConnection();
        }

        private void SearchEmploye(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from employe where concat (idemploye, fio, jobtitle) like '%" + textBoxSearchEmploye.Text + "%'";

            SqlCommand command = new SqlCommand(searchString, db.getConnection());

            db.openConnection();

            SqlDataReader read = command.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRowEmploye(dgw, read);
            }
            read.Close();
            db.closeConnection();
        }
        public Image ConvertByteArrayToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }

        private void dataGridViewEmploye_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            db.openConnection();
            selectedRow = e.RowIndex;
            DataTable dt = dataGridViewEmploye.DataSource as DataTable;
            if (dt != null)
            {
                DataRow row = dt.Rows[selectedRow];
                pictureBoxPhoto.Image = ConvertByteArrayToImage((byte[])row["Фото"]);
                
            }
            
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewEmploye.Rows[selectedRow];
                textBoxIDEmploye.Text = row.Cells[0].Value.ToString();
                textBoxFIOEmploye.Text = row.Cells[1].Value.ToString();
                textBoxJobTitleEmploye.Text = row.Cells[2].Value.ToString();
                var idemploye = dataGridViewEmploye.Rows[e.RowIndex].Cells[0].Value.ToString();
            }

            
            string queryString = $"select photoemploye from employes where idemploye = {textBoxIDEmploye.Text}";

            SqlCommand command = new SqlCommand(queryString, db.getConnection());

            try
            {

                //Retrieve BLOB from database into DataSet.
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "employes");
                int c = ds.Tables["employes"].Rows.Count;
                try
                {
                if (c > 0)
                    {

                        //BLOB is read into Byte array, then used to construct MemoryStream,
                        ////then passed to PictureBox.
                        Byte[] bytepic = new Byte[0];
                        bytepic = (Byte[])(ds.Tables["employes"].Rows[c-1]["photoemploye"]);
                        MemoryStream stmpic = new MemoryStream(bytepic);
                        pictureBoxPhoto.Image = Image.FromStream(stmpic);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            command.ExecuteNonQuery();

            db.closeConnection();
        }




        private void buttonEditEmploye_Click(object sender, EventArgs e)
        {
            ChangeEmploye();
            ClearFieldsEmploye();
        }


        private void buttonDeleteEmploye_Click(object sender, EventArgs e)
        {
            deleteRowEmploye();
            ClearFieldsEmploye();
        }

        private void buttonSaveEmploye_Click(object sender, EventArgs e)
        {
            UpdateEmploye();
        }

        private void pictureBoxRefreshEmploye_Click(object sender, EventArgs e)
        {
            RefreshDataGridEmploye(dataGridViewEmploye);
            ClearFieldsEmploye();
        }


        private void pictureBoxEracerEmploye_Click(object sender, EventArgs e)
        {
            ClearFieldsEmploye();

        }


        private void textBoxSearchEmploye_TextChanged(object sender, EventArgs e)
        {
            SearchEmploye(dataGridViewEmploye);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerDueDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// репаир (отчет)
        /// </summary>

        private void buttonNewRepair_Click(object sender, EventArgs e)
        {
            AddRepairForm addRepairForm = new AddRepairForm();
            addRepairForm.Show();
        }

        
        private void CreateColumnsRepair()
        {
            dataGridViewRepair.Columns.Add("idrepair", "id");
            dataGridViewRepair.Columns.Add("fioclient", "ФИО Клиента");
            dataGridViewRepair.Columns.Add("duedate", "Дата сдачи устройства");
            dataGridViewRepair.Columns.Add("nameservice", "Название ремонтных работ");
            dataGridViewRepair.Columns.Add("fioemploye", "ФИО Сотрудника");
            dataGridViewRepair.Columns.Add("serialnumber", "Серийный номер");
            dataGridViewRepair.Columns.Add("price", "Стоимость");
            dataGridViewRepair.Columns.Add("daterepair", "Дата окончания ремонта");
            dataGridViewRepair.Columns.Add("EsNew", String.Empty);
        }

        private void ClearFieldsRepair()
        {
            textBoxIDRepair.Text = "";
            textBoxClientRepair.Text = "";
            textBoxDueDateRepair.Text = "";
            textBoxEmployeRepair.Text = "";
            textBoxServiceRepair.Text = "";
            textBoxSerialNumRepair.Text = "";
            textBoxPriceRepair.Text = "";
            dateTimePickerDateRepair.Value = DateTime.Now;
        }


        private void ReadSingleRowRepair(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDateTime(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetInt32(6), record.GetDateTime(7), RowState.ModifiedNew);
        }
        private void RefreshDataGridRepair(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from repair";

            SqlCommand command = new SqlCommand(queryString, db.getConnection());

            db.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRowRepair(dgw, reader);
            }
            reader.Close();
            db.closeConnection();
        }


        private void ChangeRepair()
        {
            var selectedRowIndex = dataGridViewRepair.CurrentCell.RowIndex;
            var id = textBoxIDRepair.Text;
            string fioclient = textBoxClientRepair.Text;
            string serialnumber = textBoxSerialNumRepair.Text;
            var price = int.Parse(textBoxPriceRepair.Text);
            DateTime duedate = DateTime.Parse(textBoxDueDateRepair.Text);
            string nameservice = textBoxServiceRepair.Text;
            string fioemploye = textBoxEmployeRepair.Text;
            DateTime daterepair = dateTimePickerDateRepair.Value;

            if (dataGridViewRepair.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridViewRepair.Rows[selectedRowIndex].SetValues(fioclient, duedate, nameservice, fioemploye, serialnumber, price, daterepair);
                dataGridViewRepair.Rows[selectedRowIndex].Cells[8].Value = RowState.Modified;
            }
        }

        private void deleteRowRepair()
        {
            int index = dataGridViewRepair.CurrentCell.RowIndex;
            dataGridViewRepair.Rows[index].Visible = false;
            dataGridViewRepair.Rows[index].Cells[5].Value = RowState.Deleted;

        }

        private void UpdateRepair()
        {
            db.openConnection();

            for (int index = 0; index < dataGridViewRepair.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridViewRepair.Rows[index].Cells[8].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridViewRepair.Rows[index].Cells[0].Value.ToString());
                    string deleteQuery = $"delete from repair where idrepair = '{id}'";

                    SqlCommand command = new SqlCommand(deleteQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
                if (rowState == RowState.Modified)
                {
                    var idrepair = dataGridViewRepair.Rows[index].Cells[0].Value.ToString();
                    //string fioclient = dataGridViewRepair.Rows[index].Cells[1].Value.ToString();
                    //string serialnumber = dataGridViewRepair.Rows[index].Cells[2].Value.ToString();
                    //var price = dataGridViewRepair.Rows[index].Cells[3].Value.ToString();
                    //DateTime duedate = Convert.ToDateTime(dataGridViewRepair.Rows[index].Cells[4].Value);
                    //string nameservice = dataGridViewRepair.Rows[index].Cells[5].Value.ToString();
                    //string fioemploye = dataGridViewRepair.Rows[index].Cells[6].Value.ToString();
                    DateTime daterepair = Convert.ToDateTime(dataGridViewRepair.Rows[index].Cells[7].Value);
                    


                    string changeQuery = $"update repair set  daterepair = '{daterepair}'  where idrepair = '{idrepair}'";

                    SqlCommand command = new SqlCommand(changeQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            db.closeConnection();
        }

        private void SearchRepair(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from repair where concat (idrepair, fioclient, duedate, nameservice, fioemploye, serialnumber, price, daterepair) like '%" + textBoxSearchDevice.Text + "%'";

            SqlCommand command = new SqlCommand(searchString, db.getConnection());

            db.openConnection();

            SqlDataReader read = command.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRowRepair(dgw, read);
            }
            read.Close();
            db.closeConnection();
        }
        private void dataGridViewRepair_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewRepair.Rows[selectedRow];

                textBoxIDRepair.Text = row.Cells[0].Value.ToString();
                textBoxClientRepair.Text = row.Cells[1].Value.ToString();
                textBoxDueDateRepair.Text = row.Cells[2].Value.ToString();                
                textBoxServiceRepair.Text = row.Cells[3].Value.ToString();
                textBoxEmployeRepair.Text = row.Cells[4].Value.ToString();
                textBoxSerialNumRepair.Text = row.Cells[5].Value.ToString();
                textBoxPriceRepair.Text = row.Cells[6].Value.ToString();
                
                dateTimePickerDateRepair.Value = Convert.ToDateTime(row.Cells[7].Value);

            }
        }





        private void buttonEditRepair_Click(object sender, EventArgs e)
        {
            ChangeRepair();
            ClearFieldsRepair();
        }


        private void buttonDeleteRepair_Click(object sender, EventArgs e)
        {
            deleteRowRepair();
            ClearFieldsRepair();
        }

        private void buttonSaveRepair_Click(object sender, EventArgs e)
        {
            UpdateRepair();
        }

        private void pictureBoxRefreshRepair_Click(object sender, EventArgs e)
        {
            RefreshDataGridRepair(dataGridViewRepair);
            ClearFieldsRepair();
        }


        private void pictureBoxEracerRepair_Click(object sender, EventArgs e)
        {
            ClearFieldsDevices();
        }



        private void textBoxSearchRepair_TextChanged(object sender, EventArgs e)
        {
            SearchDevice(dataGridViewDevice);
        }

        private void сохранитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateClients();
            UpdateDevice();
            UpdateEmploye();
            UpdateService();
            UpdateRepair();
        }
    }
}
