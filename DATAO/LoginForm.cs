using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;
using Google.GData.Client;
using System.Net;
using Google.GData.Spreadsheets;
using System.Diagnostics;

namespace DATAO
{
    public partial class LoginForm : MaterialForm
    {
        ListFeed listFeed;
        public LoginForm(SpreadsheetsService service)
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, 
                Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.BLACK);

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = service.Query(query);

            if (feed.Entries.Count == 0)
            {
                MessageBox.Show("Пустая таблица!");
                Close();
            }

            //Берем первый лист
            SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
            WorksheetFeed wsFeed = spreadsheet.Worksheets;
            WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];
            
            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            listFeed = service.Query(listQuery);
            
        }
    

        private void enterButton_Click(object sender, EventArgs e)
        {
            foreach (ListEntry row in listFeed.Entries)
            {
                for (int i = 0; i < row.Elements.Count; i++)
                {
                    if (row.Elements[i].Value == loginTextField.Text)
                    {
                        if (row.Elements[i+1].Value == passwordTextField.Text)
                        {
                            switch (row.Elements[i+2].Value)
                            {
                                case ("admin"):
                                    {
                                        MessageBox.Show("На этом моменте открывается нарисованная форма Администратора");
                                        break;
                                    }
                                case ("worker"):
                                    {
                                        MessageBox.Show("На этом моменте открывается нарисованная форма Рабочего");
                                        break;
                                    }
                                default:
                                    {
                                        MessageBox.Show("Неизвестный тип работника предприятия.");
                                        break;
                                    }
                            }
                            Environment.Exit(0);
                        }
                        else
                        {
                            MessageBox.Show("Неправильный пароль!");
                            //Пароль оказался неверным, заканчиваем проверять ряд
                            i = row.Elements.Count;
                        }
                    }
                    else
                    {
                        //Не нашли такого логина в ряде => заканчиваем проверять ряд
                        i = row.Elements.Count;
                    }
                }
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
