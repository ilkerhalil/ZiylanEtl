﻿using System;
using ServiceTest.ZiylanETL;

namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            EtlServiceClient client = new EtlServiceClient();
            var req = new EtlServiceRequest();
            req.ServiceName = "Peraport ETL Service";
            Console.ReadKey();
            client.StartChildService(req);
        }
    }
}
