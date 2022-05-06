using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace WorkerValueInvestingService
{  

    class Servidor
    {
        public SqlConnection cn;
        public string server;
        public string catalog;

        DataSet dataSet;
               
        public void Connecting(string server, string catalog)
        {
            cn = new SqlConnection("Data source=" + server + ";initial catalog=" + catalog + ";Integrated Security=true;");
        }

        public string GetServer()
        {
            return server;
        }

        public string GetCatalog()
        {
            return catalog;
        }


        public void NovoDataset()
        {
            try
            {
                dataSet = null;
                dataSet = new DataSet();
                dataSet.Tables.Add();
                dataSet.Tables[0].Columns.Add("servidor", typeof(string));
                dataSet.Tables[0].Columns.Add("catalogo", typeof(string));
                dataSet.Tables[0].Columns.Add("token", typeof(string));
            }
            catch (Exception ex)
            {
                // eventViewerLog.Log(ex, EventLogEntryType.Error)
                Console.WriteLine(ex.Message);
            }
        }

        private string Desencriptar(string origen)
        {
            string desencriptado;
            byte[] mesBytes = Convert.FromBase64String(origen);
            byte[] keyBytes = Encoding.UTF8.GetBytes("ciro2029");
            DESCryptoServiceProvider crypto = new DESCryptoServiceProvider();
            crypto.Key = keyBytes;
            crypto.IV = keyBytes;
            ICryptoTransform iCrypto = crypto.CreateDecryptor();
            byte[] resultatBytes = iCrypto.TransformFinalBlock(mesBytes, 0, mesBytes.Length);
            desencriptado = Encoding.UTF8.GetString(resultatBytes);

            return desencriptado;
        }

        public void LeerXml()
        {
            XmlDocument documentoXML;
            XmlNodeList nodeList;
            System.Xml.XmlNode nodo;
            documentoXML = new XmlDocument();
            var location = new Uri(System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase);
            var applicationLocation = new FileInfo(location.AbsolutePath).Directory;
            var applicationLocationFolderUp = System.IO.Directory.GetParent(applicationLocation.ToString()).ToString();
            documentoXML.Load(applicationLocationFolderUp + @"\Configuration.xml");
            nodeList = documentoXML.SelectNodes("NewDataSet/Table1");
            foreach (System.Xml.XmlNode nd in nodeList)
            {
                var nodo1 = nd.ChildNodes[0].InnerText;
                var nodo2 = nd.ChildNodes[1].InnerText;
                var nodo3 = nd.ChildNodes[2].InnerText;
                server = nodo1;
                catalog = Desencriptar(nodo2);
            }
        }

    }
}
