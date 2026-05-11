using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;
using System.Net;
using System.Net.Mail;

namespace Change_Point.Models
{
    public class Home : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BDAY { get; set; }
        public Boolean IsLayout { get; set; }
        public Boolean IsLogin { get; set; }
        public Boolean IsVisitor { get; set; }
        public string param_date { get; set; }
        public string param_id { get; set; }
        public object PM { get; set; }
        public object ROLE { get; set; }
        public object STATUS { get; set; }


        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here

        }
    }



    public class ssDataList
    {
        public string PERMISSION_ID { get; set; }
        public string STATUS { get; set; }

    }

    public class Util
    {
        // Convert csv to DataTable
        public void toDatatable(DataTable csvData, string path)
        {
            using (TextFieldParser csvReader = new TextFieldParser(path))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;
                string[] colFields = csvReader.ReadFields();
                foreach (string column in colFields)
                {
                    DataColumn datacolumn = new DataColumn(column);
                    datacolumn.AllowDBNull = true;
                    csvData.Columns.Add(datacolumn);
                }
                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            fieldData[i] = null;
                        }
                    }
                    csvData.Rows.Add(fieldData);
                }
            }
        }

        // Convert List to DataTable
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        //public static void Send_email(string subject, string mail_user, string mail_target, string cc, string body)
        //{
        //    MailMessage mail = new MailMessage(mail_user, mail_target);
        //    SmtpClient client = new SmtpClient();
        //    client.Port = 25;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.UseDefaultCredentials = false;
        //    client.Host = "nonauth-smtp.global.canon.co.jp";

        //    mail.Subject = subject;
        //    mail.Body = body;

        //    if (cc.Length > 0)
        //    {
        //        foreach (var adr_cc in cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            mail.CC.Add(adr_cc);
        //        }
        //    }

        //    client.Send(mail);
        //}

        public static void SendEmail(string subject, string mailUser, string mailTarget, string cc, string body)
        {
            MailMessage mail = new MailMessage(mailUser, mailTarget);
            SmtpClient client = new SmtpClient();

            // Set SMTP server information
            client.Port = 25; // Change to the appropriate port
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "nonauth-smtp.global.canon.co.jp";

            //// Set credentials
            //client.Credentials = new NetworkCredential("your-smtp-username", "your-smtp-password");

            //// Enable SSL if required
            //client.EnableSsl = true;

            // Set email details
            mail.Subject = subject;
            mail.Body = body;

            // Add CC recipients
            if (!string.IsNullOrEmpty(cc))
            {
                foreach (var adrCc in cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.CC.Add(adrCc);
                }
            }

            // Send the email
            client.Send(mail);
        }



    }


}
