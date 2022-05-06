using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerValueInvestingService
{
    public class Worker : BackgroundService
    {
        string servidor;
        string catalogo;

        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Servidor ServidorConnection = new Servidor();
            ServidorConnection.NovoDataset();
            ServidorConnection.LeerXml();
            servidor = ServidorConnection.GetServer();
            catalogo = ServidorConnection.GetCatalog();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {           

            while (!stoppingToken.IsCancellationRequested)
            {

                GetMessages();

                await Task.Delay(3000, stoppingToken);
            }
        }

        private void GetMessages()
        {
            BuySell_Helper buysellHelper = new BuySell_Helper();

            int resultadoSuppliers = 0;
            int oldresultadoSuppliers = 0;

            int resultadoCustomers = 0;
            int oldresultadoCustomers = 0;

            int resultadoPurchases = 0;
            int oldresultadoPurchases = 0;

            int resultadoSales = 0;
            int oldresultadoSales = 0;

            int resultadoItems = 0;
            int oldresultadoItems = 0;

            string cadena = "SELECT * FROM Map_Async WHERE Sinchronizado = 0 AND Supplier = 1";
            string tabla = "Map_Async";
            resultadoSuppliers = buysellHelper.RetrieveCountUnsynchronizedRecords(servidor, catalogo, cadena, tabla);
            if (resultadoSuppliers > 0)
            {               
                if(oldresultadoSuppliers != resultadoSuppliers)
                {
                    Console.WriteLine(resultadoSuppliers.ToString() + " unzynchronized suppliers");
                }
                oldresultadoSuppliers = resultadoSuppliers;
            }

            cadena = "SELECT * FROM Map_Async WHERE Sinchronizado = 0 AND Supplier = 0";
            tabla = "Map_Async";
            resultadoCustomers = buysellHelper.RetrieveCountUnsynchronizedRecords(servidor, catalogo, cadena, tabla);
            if (resultadoCustomers > 0)
            {
                if(oldresultadoCustomers != resultadoCustomers)
                {
                    Console.WriteLine(resultadoCustomers.ToString() + " unzynchronized customers");
                }
                oldresultadoCustomers = resultadoCustomers;
            }

            cadena = "SELECT * FROM CatMap_Sync WHERE Sinchronizado = 0";
            tabla = "CatMap_Sync";
            resultadoItems = buysellHelper.RetrieveCountUnsynchronizedRecords(servidor, catalogo, cadena, tabla);
            if (resultadoItems > 0)
            {
                if(oldresultadoItems != resultadoItems)
                {
                    Console.WriteLine(resultadoItems.ToString() + " unzynchronized items");
                }
                oldresultadoItems = resultadoItems;
            }


            cadena = "SELECT * FROM PurchasedItems_Sync WHERE Sinchronizado = 0";
            tabla = "PurchasedItems_Sync";
            resultadoPurchases = buysellHelper.RetrieveCountUnsynchronizedRecords(servidor, catalogo, cadena, tabla);
            if (resultadoPurchases > 0)
            {
                if(oldresultadoPurchases != resultadoPurchases)
                {
                    Console.WriteLine(resultadoPurchases.ToString() + " unzynchronized purchases");
                }
                oldresultadoPurchases = resultadoPurchases;
            }

            cadena = "SELECT * FROM SoldItems_Sync WHERE Sinchronizado = 0";
            tabla = "SoldItems_Sync";
            resultadoSales = buysellHelper.RetrieveCountUnsynchronizedRecords(servidor, catalogo, cadena, tabla);
            if (resultadoSales > 0)
            {
                if(oldresultadoSales != resultadoSales)
                {
                    Console.WriteLine(resultadoSales.ToString() + " unzynchronized sales");
                }
                oldresultadoSales = resultadoSales;
            }
        }
    }
}
