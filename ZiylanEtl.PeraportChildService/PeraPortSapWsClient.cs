using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private DtoSet DtoSet { get; }

        #region Fields

        private readonly IDataAccess _dataAccess;
        private ZRT_ENT_PERAPORTClient _zRtEntPeraportClient;
        private readonly string[] _filters =
            {
                "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11","C12", "C13", "C14", "C15", "C16"
            };


        #endregion

        #region Properties

        public string Username { get; set; }

        public string Password { get; set; }

        public string Erdat { get; set; }



        public IDictionary<string, object> ChildServiceParameters { get; }

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


        public PeraPortSapWsClient(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            InitWebServiceClient();
            DtoSet = new DtoSet();
        }

        public override void StartService()
        {
            var baslamaZamani = DateTime.Now;
            var toplamZaman = Stopwatch.StartNew();
            ValidateServiceParameter();
            ZrtEntPeraportResponse1 response = null;

            var logContent = string.Empty;
            foreach (var filter in _filters)
            {
                var startNew = Stopwatch.StartNew();
                var exceptionMessage = string.Empty;

                var request = Helper.CreateRequest(filter, "2015-04-17");
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
            var bitisZamani = DateTime.Now;
            var toplamGecenZaman = toplamZaman.Elapsed;
            toplamZaman.Stop();
            Helper.CreateLog(baslamaZamani, logContent, bitisZamani, toplamGecenZaman);


            //Datayı insert edeceğiz

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
