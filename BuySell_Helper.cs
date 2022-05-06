using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WorkerValueInvestingService
{
    class BuySell_Helper/*: BackgroundService*/
    {
      
        public int RetrieveCountUnsynchronizedRecords(string servidor, string catalogo, string cadena, string tabla)
        {
            int countUnsynrecords = 0;

            try
            {
                Servidor ServidorConnection = new Servidor();               

                ServidorConnection.Connecting(servidor, catalogo);

                if (ServidorConnection.cn != null && ServidorConnection.cn.State == ConnectionState.Closed)
                {
                    ServidorConnection.cn.Open();
                }

                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand();
                SqlCommand commandInsert = new SqlCommand();               

                // Para que el Combobox se alimenta de la base de datos:
                command.Connection = ServidorConnection.cn;
                command.CommandType = CommandType.Text;
                string query = @cadena;
                command.CommandText = query;
                dt.TableName = tabla;
                dt.Load(command.ExecuteReader());

                commandInsert.Connection = ServidorConnection.cn;
                commandInsert.CommandType = CommandType.Text;

                foreach (DataRow Fila in dt.Rows)
                {
                    countUnsynrecords = countUnsynrecords + 1;
                }

                return countUnsynrecords;
            }
            catch (Exception ex)
            {
                return countUnsynrecords;
            }

        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while(!stoppingToken.IsCancellationRequested)
        //    {
        //        string cadena = "SELECT * FROM Map_Async WHERE Sinchronizado = 0";
        //        string tabla = "Map_Async";
        //        int resultado = RetrieveCountUnsynchronizedRecords(cadena, tabla);
        //        Console.WriteLine(resultado.ToString() + " unzynchronized customers or suppliers");
        //        await Task.Delay(360000,stoppingToken);
        //    }
        //}
    }
}
