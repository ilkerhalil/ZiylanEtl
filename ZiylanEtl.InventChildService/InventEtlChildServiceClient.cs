using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Security;
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

        private StringBuilder _builder;

        public override void StartService()
        {
            if (string.IsNullOrWhiteSpace(Username)) throw new SecurityException();
            if (string.IsNullOrWhiteSpace(Password)) throw new SecurityException();

            var toplamZaman = Stopwatch.StartNew();
            using (var client = new ZRT_ENT_PERAPORTClient("binding_SOAP12"))
            {
                client.ClientCredentials.UserName.UserName = Username;
                client.ClientCredentials.UserName.Password = Password;
                ZrtEntPeraportResponse1 response = null;

                var filters = new[]
                {
                    "C1","C2","C3","C4","C5","C6","C7","C8","C9"
                };

                CreateLog(DateTime.Now);

                foreach (var filter in filters)
                {
                    var startNew = Stopwatch.StartNew();
                    var exceptionMessage = string.Empty;
                    try
                    {
                        var request = CreateRequest(filter);
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
                        dolukoleksiyonlar = SerializeNonEmptyCollections(response);
                    }

                    var logContent = string.Concat(dosyaAdi, Environment.NewLine, exceptionMessage, Environment.NewLine,
                        gecenZaman, Environment.NewLine, dolukoleksiyonlar);

                    AppendLog(logContent);
                }

                var toplamGecenZaman = toplamZaman.Elapsed;
                SaveLog(DateTime.Now, toplamGecenZaman);
                toplamZaman.Stop();
            }
        }

        private static string SerializeNonEmptyCollections(ZrtEntPeraportResponse1 response)
        {
            var arrayProperties = response.ZrtEntPeraportResponse.GetType().GetProperties().Where(w => w.PropertyType.IsArray);
            var st = arrayProperties.ToDictionary(s => s.Name, info => (info.GetValue(response.ZrtEntPeraportResponse) as Array).Length);
            var dolukoleksiyonlar = st.ToFormattedString("<li>{0} : {1} \n</li>", Environment.NewLine);
            return dolukoleksiyonlar;
        }

        private ZrtEntPeraportRequest CreateRequest(string filter)
        {
            var zrnEntPeraPort = new ZrtEntPeraport
            {
                Erdat =
                    string.IsNullOrWhiteSpace(this.Erdat)
                        ? string.Format(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                        : this.Erdat,
                C1 = "",
                C2 = "",
                C3 = "",
                C4 = "",
                C5 = "",
                C6 = "",
                C7 = "",
                C8 = "",
                C9 = "",
                GtZinventHrk = new[] {new ZinventHrk(),},
                GtZinventAsorti = new[] {new ZinventAsorti(),},
                GtZinventFyt = new[] {new ZinventFyt(),},
                GtZinventMlz = new[] {new ZinventMlz(),},
                GtZinventStok = new[] {new ZinventStok(),},
                GtZinventStokA = new[] {new ZinventStokA(),},
                GtZinventTes = new[] {new ZinventTes(),},
                GtZinventTrn = new[] {new ZinventTrn(),},
                GtZinventUy = new[] {new ZinventUy(),}
            };

            var property = zrnEntPeraPort.GetType().GetProperties().Single(w => w.Name == filter);
            property.SetValue(zrnEntPeraPort, "X");
            var request = new ZrtEntPeraportRequest
            {
                ZrtEntPeraport = zrnEntPeraPort
            };
            return request;
        }

        private void CreateLog(DateTime baslamaZamani)
        {
            _builder = new StringBuilder();
            _builder.AppendLine("<html><head><meta charset=\"utf-8\" /></head><body><div>");
            _builder.AppendFormat("<b>Başlama Zamanı {0}</b>", baslamaZamani);
            _builder.AppendLine("<ul>");
        }

        private void AppendLog(string content)
        {
            _builder.AppendLine(content);
        }

        private void SaveLog(DateTime bitisZamani, TimeSpan toplamZaman)
        {
            _builder.AppendLine("</ul>");
            _builder.AppendLine("</div>");
            _builder.AppendFormat("<b>Bitiş Zamanı {0}</b>", bitisZamani.ToString("F"));
            _builder.AppendFormat("<b>Geçen Zaman {0}</b>", toplamZaman.ToString("G"));
            _builder.AppendLine("</body></html>");
            File.WriteAllText(Path.GetRandomFileName() + ".txt", _builder.ToString());
        }
    }
}
