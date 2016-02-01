using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using AutoMapper;
using ZiylanEtl.Abstraction.Helper;
using ZiylanEtl.Abstraction.ServiceContracts;
using ZiylanEtl.PeraportChildService.ServiceProxy;
using ZiylanEtl.DataAccess;
using ZiylanEtl.PeraportChildService.Dto;

namespace ZiylanEtl.PeraportChildService
{
    /* Peraport ETL Webservice Client */
    public class PeraPortSapWsClient : BaseEtlChildService
    {

        #region Fields

        private StringBuilder _builder;
        private readonly IDataAccess _dataAccess;
        private readonly ZRT_ENT_PERAPORTClient _zRtEntPeraportClient;
        private readonly IMapper _mapperInit;

        #endregion

        #region Properties

        public string Username { get; set; }

        public string Password { get; set; }

        public string Erdat { get; set; }



        public IDictionary<string, object> ChildServiceParameters { get; }

        public override string ServiceName { get; } = "Peraport ETL Service";

        #endregion

        public override void StartService()
        {

            var toplamZaman = Stopwatch.StartNew();
            ZrtEntPeraportResponse1 response = null;

            var filters = new[]
            {
                "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11","C12", "C13", "C14", "C15", "C16"
            };

            CreateLog(DateTime.Now);

            foreach (var filter in filters)
            {
                var startNew = Stopwatch.StartNew();
                var exceptionMessage = string.Empty;
                try
                {
                    var request = CreateRequest(filter);
                    response = _zRtEntPeraportClient.ZrtEntPeraport(request);
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
            //Datayı map edeceğiz
            //Datayı insert edeceğiz
        }

        private void InitWebServiceClient()
        {
            _zRtEntPeraportClient.ClientCredentials.UserName.UserName = Username;
            _zRtEntPeraportClient.ClientCredentials.UserName.Password = Password;
        }

       
        private void ValidateServiceParameter()
        {
            if (Username.IsNullAndWhiteSpace()) throw new SecurityException("Username boş olamaz");
            if (Password.IsNullAndWhiteSpace()) throw new SecurityException("Password boş olamaz");
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
                C10 = "",
                C11 = "",
                C12 = "",
                C13 = "",
                C14 = "",
                C15 = "",
                C16 = "",
                GtZinventHrk = new[] { new ZinventHrk(), },
                GtZinventAsorti = new[] { new ZinventAsorti(), },
                GtZinventFyt = new[] { new ZinventFyt(), },
                GtZinventMlz = new[] { new ZinventMlz2(), },
                GtZinventStok = new[] { new ZinventStok(), },
                GtZinventStokA = new[] { new ZinventStokA(), },
                GtZinventTes = new[] { new ZinventTes(), },
                GtZinventTrn = new[] { new ZinventTrn(), },
                GtZinventUy = new[] { new ZinventUy(), },
                GtT6wst = new[] { new T6wst(), },
                GtT134t = new[] { new T134t(), },
                GtLfa1 = new[] { new ZentLfa1(), },
                GtT001 = new[] { new ZentT001(), },
                GtT023t = new[] { new T023t(), },
                GtWrfBrandsT = new[] { new WrfBrandsT(), },
                GtZinventSas = new[] { new ZinventSas(), }
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

        private static IMapper MapperInit()
        {
            var mapperConfiguration = new MapperConfiguration(con =>
            {
                con.CreateMap<T023t, T023tDto>();
                con.CreateMap<T134t, T134tDto>();
                con.CreateMap<T6wst, T6wstDto>();
                con.CreateMap<WrfBrandsT, WrfBrandsTDto>();
                con.CreateMap<ZentLfa1, ZentLfa1Dto>();
                con.CreateMap<ZentT001, ZentT001Dto>();
                con.CreateMap<ZinventAsorti, ZinventAsortiDto>();
                con.CreateMap<ZinventFyt, ZinventFytDto>();
                con.CreateMap<ZinventHrk, ZinventHrkDto>();
                con.CreateMap<ZinventMlz2, ZinventMlzDto>();
                con.CreateMap<ZinventSas, ZinventSasDto>();
                con.CreateMap<ZinventStok, ZinventStokDto>();
                con.CreateMap<ZinventStokA, ZinventStokADto>();
                con.CreateMap<ZinventTes, ZinventTesDto>();
                con.CreateMap<ZinventTrn, ZinventTrnDto>();
                con.CreateMap<ZinventUy, ZinventUyDto>();
            });
            return mapperConfiguration.CreateMapper();
        }


    }
}
