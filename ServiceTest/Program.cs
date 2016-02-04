using System;
using System.Collections.Generic;
using ServiceTest.ZiylanETL;
using System.Windows.Forms;

namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Form1 f = new Form1();
            Application.Run(f);
            return;
            Console.ReadKey();
            EtlServiceClient client = new EtlServiceClient();
            var req = new EtlServiceRequest();
            req.ServiceName = "Peraport ETL Service";
            req.ServiceParameter = new Dictionary<string, object>();
            req.ServiceParameter.Add("Erdat", "2016-02-03");
            client.StartChildService(req);
        }
    }
}
