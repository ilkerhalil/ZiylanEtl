﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using ZiylanEtl.Abstraction.Helper;
using ZiylanEtl.PeraportChildService.Dto;

namespace ZiylanEtl.PeraportChildService
{
    internal static class Helper
    {
        public static ZrtEntPeraportRequest CreateRequest(string filter, string erdat,string dateFormat = "yyyy-MM-dd")
        {
            var zrnEntPeraPort = new ZrtEntPeraport
            {
                Erdat =
                    string.IsNullOrWhiteSpace(erdat)
                        ? string.Format(DateTime.Now.AddDays(-1).ToString(dateFormat))
                        : erdat,
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

            var property = zrnEntPeraPort.GetType() .GetProperties().Single(w => w.Name == filter);
            property.SetValue(zrnEntPeraPort, "X");
            var request = new ZrtEntPeraportRequest
            {
                ZrtEntPeraport = zrnEntPeraPort
            };
            return request;
        }
        public static IMapper CreateMapper()
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
        public static void CreateLog(string content, DateTime webServisBaslamaZamani, DateTime webServisBitisZamani,TimeSpan webServisSure, DateTime dbKayitBaslangicZamani,DateTime dbKayitBitisZamani,TimeSpan dbKayitSuresi, TimeSpan toplamZaman)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<html><head><meta charset=\"utf-8\" /></head><body><div>");
            builder.AppendFormat("<b>Web Servis Çağrısı Başlama Zamanı {0}</b>", webServisBaslamaZamani);
            builder.AppendLine("<ul>");
            builder.AppendLine(content);
            builder.AppendLine("</ul>");
            builder.AppendLine("</div>");
            builder.AppendFormat("<b>Web Servis Çağrısı Bitiş Zamanı {0}</b>", webServisBitisZamani.ToString("F"));
            builder.AppendFormat("<b>Web Servis Çağrısında Geçen Zaman {0}</b>", webServisSure.ToString("G"));

            builder.AppendFormat("<b>Database Kayıt Başlangıç Zamanı {0}</b>", dbKayitBaslangicZamani.ToString("F"));
            builder.AppendFormat("<b>Database Kayıt Bitiş Zamanı  {0}</b>", dbKayitBitisZamani.ToString("G"));
            builder.AppendFormat("<b>Database Kayıt Esnasında Geçen Zaman {0}</b>", dbKayitSuresi.ToString("G"));
            builder.AppendFormat("<b>Toplam Süre {0}</b>", toplamZaman.ToString("G"));

            builder.AppendLine("</body></html>");
            File.WriteAllText(Path.GetRandomFileName() + ".txt", builder.ToString());
        }
        public static string SerializeNonEmptyCollections(ZrtEntPeraportResponse1 response)
        {
            var arrayProperties = response.ZrtEntPeraportResponse.GetType().GetProperties().Where(w =>
w.PropertyType.IsArray);
            var st = arrayProperties.ToDictionary(s => s.Name, info => (info.GetValue(response.ZrtEntPeraportResponse) as Array).Length);
            var dolukoleksiyonlar = st.ToFormattedString("<li>{0} : {1} \n</li>", Environment.NewLine);
            return dolukoleksiyonlar;
        }
        public static string CreateInsertQuery(Type type)
        {
            var valuesVb = string.Join(",", type.GetProperties().Select(s => $"@{s.Name}"));
            var tableName = type.Name.Replace("Dto", "").Trim();
            return $"insert into {tableName} values ({valuesVb})";
        }
    }
}
