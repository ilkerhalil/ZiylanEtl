﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceTest.ZiylanETL;

namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            EtlServiceClient client = new EtlServiceClient();
            var req = new EtlServiceRequest();
            req.ServiceName = "NULL Child Service";
            client.StartChildService(req);
        }
    }
}