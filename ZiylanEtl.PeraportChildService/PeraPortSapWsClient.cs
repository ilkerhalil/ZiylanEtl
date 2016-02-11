using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Configuration;
using ZiylanEtl.Abstraction.Helper;
using ZiylanEtl.Abstraction.Notification;
using ZiylanEtl.Abstraction.ServiceContracts;
using ZiylanEtl.PeraportChildService.ServiceProxy;
using ZiylanEtl.DataAccess;
using ZiylanEtl.PeraportChildService.Dto;

namespace ZiylanEtl.PeraportChildService
{
    /* Peraport ETL Webservice Client */
    public class PeraPortSapWsClient : BaseEtlChildService
    {
        private DtoSet DtoSet { get; }

        #region Fields

        private readonly IDataAccess _dataAccess;
        private readonly INotification _notification;
        private ZRT_ENT_PERAPORTClient _zRtEntPeraportClient;
        private string[] _filters =
            {
                "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11","C12", "C13", "C14", "C15", "C16"
            };

        private bool _insert;

        #endregion

        #region Properties

        public string Username { get; set; }

        public string Password { get; set; }

        public string Erdat { get; set; }



       // public IDictionary<string, object> ChildServiceParameters { get; }

        public override string ServiceName { get; } = "Peraport ETL Service";

        #endregion

        #region PrivateMethods

        private void InitWebServiceClient()
        {
            _zRtEntPeraportClient = new ZRT_ENT_PERAPORTClient("binding_SOAP12");
            _zRtEntPeraportClient.ClientCredentials.UserName.UserName = Username;
            _zRtEntPeraportClient.ClientCredentials.UserName.Password = Password;
        }

        private void ValidateServiceParameter()
        {
            if (Username.IsNullAndWhiteSpace()) throw new SecurityException("Username boş olamaz");
            if (Password.IsNullAndWhiteSpace()) throw new SecurityException("Password boş olamaz");
        }

        #endregion


        public PeraPortSapWsClient(IDataAccess dataAccess,INotification notification)
        {
            _dataAccess = dataAccess;
            _notification = notification;
            //ChildServiceParameters = new Dictionary<string, object>();
            InitWebServiceClient();
            DtoSet = new DtoSet();
        }

        public override void StartService()
        {
            var webServisBaslamaZamani = DateTime.Now;
            var toplamZaman = Stopwatch.StartNew();
            ValidateServiceParameter();
            ZrtEntPeraportResponse1 response = null;

            var logContent = string.Empty;

            object tables = null;
            this.ChildServiceParameters.TryGetValue("Tables", out tables);
            if (tables != null) _filters = tables.ToString().Split(new char[] {','}, StringSplitOptions.None);


            foreach (var filter in _filters)
            {
                Console.WriteLine(filter);
                var startNew = Stopwatch.StartNew();
                var exceptionMessage = string.Empty;

                object erdat = null;

                this.ChildServiceParameters.TryGetValue("Erdat", out erdat);
                Erdat = erdat.ToString();

                var request = Helper.CreateRequest(filter, Erdat);
                response = _zRtEntPeraportClient.ZrtEntPeraport(request);

                var parameterName = $"Filtre : {filter} \n Test Zamanı : {DateTime.Now.ToLongDateString() + ":" + DateTime.Now.ToLongTimeString()}";
                var gecenZaman = startNew.Elapsed;
                startNew.Stop();

                //info => (info.GetValue(response.ZrtEntPeraportResponse) as Array
                var dolukoleksiyonlar = Helper.SerializeNonEmptyCollections(response);

                var enumerable = response.ZrtEntPeraportResponse.GetType()
                    .GetProperties().Where(w => w.PropertyType.IsArray && w.GetValue(response.ZrtEntPeraportResponse) != null && (w.GetValue(response.ZrtEntPeraportResponse) as Array).Length > 1)
                    .Select(s => s.GetValue(response.ZrtEntPeraportResponse));

                if (enumerable.Any())
                {
                    AddDtoSet(enumerable.Single());
                }
             
                logContent += string.Concat(parameterName, Environment.NewLine, exceptionMessage, Environment.NewLine, gecenZaman, Environment.NewLine, dolukoleksiyonlar);
            }

            var webServisSure = toplamZaman.Elapsed;
            var webServisBitisZamani = DateTime.Now;
            var dbKayitBaslangicZamani = DateTime.Now;
            var dbKayitSuresi = Stopwatch.StartNew();

            object insert = null;
            this.ChildServiceParameters.TryGetValue("Insert", out insert);

            bool testMode = Convert.ToBoolean(ConfigurationManager.AppSettings["TestMode"]);

            if (insert != null) _insert = (bool)insert;

            //Database'e yaz
            if(_insert && !testMode) InsertData(DtoSet);

            var toplamDbKayitSuresi = dbKayitSuresi.Elapsed;
            var dbKayitBitisZamani = DateTime.Now;
            dbKayitSuresi.Stop();

            var toplamGecenZaman = toplamZaman.Elapsed;
            toplamZaman.Stop();
            //_notification.Send("");

            Helper.CreateLog(logContent, webServisBaslamaZamani, webServisBitisZamani, webServisSure, dbKayitBaslangicZamani, dbKayitBitisZamani , toplamDbKayitSuresi, toplamGecenZaman);
        }

        private void InsertData(DtoSet dtoSet)
        {
            foreach (var propertyInfo in DtoSet.GetType().GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                if (propertyInfo.GetValue(dtoSet) == null) continue;
                if ((propertyInfo.GetValue(dtoSet) as IList).Count <= 1) continue;
                Console.WriteLine(propertyInfo.PropertyType.GenericTypeArguments[0]);
                var query = Helper.CreateInsertQuery(propertyInfo.PropertyType.GenericTypeArguments[0]);
                _dataAccess.ExecuteQuery(query, CommandType.Text, propertyInfo.GetValue(dtoSet));
            }
        }

        private void AddDtoSet(object o)
        {
            var mapper = Helper.CreateMapper();
            var targetType = mapper.ConfigurationProvider.GetAllTypeMaps().Single(s => s.SourceType == o.GetType().GetElementType()).DestinationType;
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(targetType);
            var tr = mapper.Map(o, o.GetType(), constructedListType);
            var property = DtoSet.GetType().GetProperties().Single(s => s.PropertyType == tr.GetType());
            property.SetValue(DtoSet, tr);
        }
    }
}
