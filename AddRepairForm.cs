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
        //    DB db = new DB();
        //    private BindingSource client;
        //    private BindingSource device;
        //    private BindingSource employe;
        //    private BindingSource service;
        //    public AddRepairForm()
        //    {
        //        InitializeComponent();

        //        StartPosition = FormStartPosition.CenterScreen;

        //        client = new BindingSource();
        //        client.DataSource = typeof(ComboItem);
        //        comboBoxClient.DataSource = client;
        //        comboBoxClient.DisplayMember = nameof(ComboItem.Text);

        //        device = new BindingSource();
        //        device.DataSource = typeof(ComboItem);
        //        comboBoxDevice.DataSource = device;
        //        comboBoxDevice.DisplayMember = nameof(ComboItem.Text);

        //        employe = new BindingSource();
        //        employe.DataSource = typeof(ComboItem);
        //        comboBoxEmploye.DataSource = employe;
        //        comboBoxEmploye.DisplayMember = nameof(ComboItem.Text);

        //        service = new BindingSource();
        //        service.DataSource = typeof(ComboItem);
        //        comboBoxService.DataSource = service;
        //        comboBoxService.DisplayMember = nameof(ComboItem.Text);

        //        this.Load += FormMain_Load;
    }

        //private async void FormMain_Load(object sender, EventArgs e)
        //{
        //    //извлекаем из БД типы комнат, заполняем комбобокс
        //    await LoadClientsAsync();
        //    //подписка на событие выбора типа комнаты
        //    _bsTypes.CurrentChanged += OnRoomTypeSelected;
        //}

        ///// <summary>
        ///// После выбора типа комнат
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private async void OnRoomTypeSelected(object sender, EventArgs e)
        //{
        //    //определяем Id выбранного пункта
        //    int selectedId = (_bsTypes.Current as ComboItem).Id;
        //    //очищаем комбобокс
        //    _bsNumbers.Clear();

        //    //в случае выбора "Любой"
        //    if (selectedId == 0)
        //    {
        //        _bsNumbers.Add(new ComboItem { Text = "Нет выбора" });
        //        return;
        //    }

        //    await LoadRoomNumbersByTypeIdAsync(selectedId);
        //}

        ///// <summary>
        ///// Заполение комбобкса типов комнат
        ///// </summary>
        ///// <returns></returns>
        //private async Task LoadClientsAsync()
        //{
        //    db.openConnection();

        //    try
        //    {
        //        using (var con = new SqlConnection(_conString))
        //        using (var cmd = con.CreateCommand())
        //        {
        //            cmd.CommandText = @"SELECT Id
        //                         , Name
        //                        FROM RoomTypes
        //                        ORDER BY Name;";
        //            await con.OpenAsync();
        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var ci = new ComboItem
        //                    {
        //                        Id = reader.GetInt32(0),
        //                        Text = reader.GetString(1)
        //                    };
        //                    _bsTypes.Add(ci);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Ошибка",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        }




    

