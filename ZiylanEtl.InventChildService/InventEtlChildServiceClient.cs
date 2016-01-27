using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Diagnostics;
using ZiylanEtl.Abstraction.ServiceContracts;
using ZiylanEtl.InventChildService.Proxy;

namespace ZiylanEtl.InventChildService
{
    /// <summary>
    /// todo:Web Service client implement edilecek
    /// todo:Toplam ne kadar sürdü,web servis metod başına ne kadar sürdü,loglanmalı
    /// todo:IDispose implement edilmeli
    /// </summary>
    public class InventEtlChildServiceClient : IEtlChildService
    {
        public string ServiceName { get; } = "INVENT";
        public string Username { get; set; }
        public string Password { get; set; }

        public void StartService()
        {
            var client = new ZRT_ENT_PERAPORTClient("binding_SOAP12");
            client.ClientCredentials.UserName.UserName = this.Username;
            client.ClientCredentials.UserName.Password = this.Password;
            ZrtEntPeraportResponse1 response = null;

            var filters = new[]
            {
             "C1","C2","C3","C4","C5","C6","C7","C8","C9"
            };

            foreach (var filter in filters)
            {
                var startNew = Stopwatch.StartNew();
                var exceptionMessage = string.Empty;
                try
                {
                    var zrnEntPeraPort = new ZrtEntPeraport
                    {
                        Erdat = "2016-01-18",
                        C1 = "",
                        C2 = "",
                        C3 = "",
                        C4 = "",
                        C5 = "",
                        C6 = "",
                        C7 = "",
                        C8 = "",
                        C9 = "",
                        GtZinventHrk = new ZinventHrk[] { new ZinventHrk(), },
                        GtZinventAsorti = new ZinventAsorti[] { new ZinventAsorti(), },
                        GtZinventFyt = new ZinventFyt[] { new ZinventFyt(), },
                        GtZinventMlz = new ZinventMlz[] { new ZinventMlz(), },
                        GtZinventStok = new ZinventStok[] { new ZinventStok(), },
                        GtZinventStokA = new ZinventStokA[] { new ZinventStokA(), },
                        GtZinventTes = new ZinventTes[] { new ZinventTes(), },
                        GtZinventTrn = new ZinventTrn[] { new ZinventTrn(), },
                        GtZinventUy = new ZinventUy[] { new ZinventUy(), }
                    };
                    var property = zrnEntPeraPort.GetType().GetProperties().Single(w => w.Name == filter);
                    property.SetValue(zrnEntPeraPort, "X");
                    var request = new ZrtEntPeraportRequest
                    {
                        ZrtEntPeraport = zrnEntPeraPort
                    };
                    response = client.ZrtEntPeraport(request);
                }

                catch (Exception ex)
                {
                    exceptionMessage = string.Format("Exception Message {0}\n", ex);
                }
                var dosyaAdi = string.Format("Filtre : {0} \n Test Zamanı : {1}", filter, DateTime.Now.ToLongDateString() + ":" + DateTime.Now.ToLongTimeString());
                var gecenZaman = startNew.Elapsed;
                startNew.Stop();
                var dolukoleksiyonlar = string.Empty;
                if (string.IsNullOrWhiteSpace(exceptionMessage))
                {
                    var arrayProperties = response.ZrtEntPeraportResponse.GetType().GetProperties().Where(w => w.PropertyType.IsArray);
                    var st = arrayProperties.ToDictionary(s => s.Name, info => (info.GetValue(response.ZrtEntPeraportResponse) as Array).Length);
                    dolukoleksiyonlar = st.ToFormattedString("{0} : {1} \n", Environment.NewLine);
                }
                var memorySize = GC.GetTotalMemory(true);
                File.WriteAllText(filter + ".txt", string.Concat(new object[] { dosyaAdi, Environment.NewLine, exceptionMessage, Environment.NewLine, gecenZaman, Environment.NewLine, dolukoleksiyonlar }));
                Console.WriteLine();
            }
            Console.WriteLine("Bitti");
            Console.ReadLine();
        }
    }
}
