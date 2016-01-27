using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.ServiceModel.Description;
using System.Text;
using ZiylanEtl.Abstraction.ServiceContracts;
using ZiylanEtl.InventChildService.Proxy;

namespace ZiylanEtl.InventChildService
{
    /// <summary>
    /// todo:Web Service client implement edilecek
    /// todo:Toplam ne kadar sürdü,web servis metod başına ne kadar sürdü,loglanmalı
    /// todo:IDispose implement edilmeli
    /// </summary>
    public class InventEtlChildServiceClient : BaseEtlChildService
    {
        public override string ServiceName { get; } = "INVENT";
        

        public string Username { get; set; }
        public string Password { get; set; }

        public InventEtlChildServiceClient()
        {
            object value;
            if (ChildServiceParameters.TryGetValue("ErDat", out value))
            {
                Erdat = value as string;
            }
        }

        public string Erdat { get; set; }

        public override void StartService()
        {
            var baslamaZamanı = DateTime.Now.ToString("F");
            Stopwatch toplamZaman;
            string logContent;
            StringBuilder builder;
            using (var client = new ZRT_ENT_PERAPORTClient("binding_SOAP12"))
            {
                client.ClientCredentials.UserName.UserName = Username;
                client.ClientCredentials.UserName.Password = Password;
                ZrtEntPeraportResponse1 response = null;
                toplamZaman = Stopwatch.StartNew();
                var filters = new[]
                {
                    "C1","C2","C3","C4","C5","C6","C7","C8","C9"
                };
                logContent = string.Empty;
                builder = new StringBuilder();
                builder.AppendLine("<div>");
                builder.AppendFormat("<b>Başlama Zamanı {0}</b>", baslamaZamanı);
                builder.AppendLine("<ul>");

                foreach (var filter in filters)
                {
                    var startNew = Stopwatch.StartNew();
                    var exceptionMessage = string.Empty;
                    try
                    {
                        var zrnEntPeraPort = new ZrtEntPeraport
                        {
                            Erdat = string.IsNullOrWhiteSpace(this.Erdat) ? string.Format(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")) : this.Erdat,
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
                        exceptionMessage = $"<li>Exception Message {ex}\n</li>";
                    }
                    var dosyaAdi =
                        $"Filtre : {filter} \n Test Zamanı : {DateTime.Now.ToLongDateString() + ":" + DateTime.Now.ToLongTimeString()}";
                    var gecenZaman = startNew.Elapsed;
                    startNew.Stop();
                    var dolukoleksiyonlar = string.Empty;
                    if (string.IsNullOrWhiteSpace(exceptionMessage))
                    {
                        var arrayProperties = response.ZrtEntPeraportResponse.GetType().GetProperties().Where(w => w.PropertyType.IsArray);
                        var st = arrayProperties.ToDictionary(s => s.Name, info => (info.GetValue(response.ZrtEntPeraportResponse) as Array).Length);
                        dolukoleksiyonlar = st.ToFormattedString("<li>{0} : {1} \n</li>", Environment.NewLine);
                    }
                    logContent = string.Concat(dosyaAdi, Environment.NewLine, exceptionMessage, Environment.NewLine,
                        gecenZaman, Environment.NewLine, dolukoleksiyonlar);

                }
            }
            builder.AppendLine(logContent);
            builder.AppendLine("</ul>");
            builder.AppendLine("</div>");
            toplamZaman.Stop();
            builder.AppendFormat("<b>Bitiş Zamanı {0}</b>", DateTime.Now.ToString("F"));
            builder.AppendFormat("<b>Geçen Zaman {0}</b>", toplamZaman.Elapsed.ToString("G"));
            File.WriteAllText(Path.GetRandomFileName() + ".txt", builder.ToString());
        }
    }
}
