using System.Collections.Generic;
using System.Security;
using ZiylanEtl.Abstraction.Helper;
using ZiylanEtl.Abstraction.ServiceContracts;
using ZiylanEtl.DataAccess;
using ZiylanEtl.PeraportChildService.ServiceProxy;

namespace ZiylanEtl.PeraportChildService
{
    public class PeraPortSapWsClient : BaseEtlChildService
    {
        private readonly IDataAccess _dataAccess;
        private readonly ZRT_ENT_PERAPORTClient _zRtEntPeraportClient;

        public string UserName { get; }

        public string Password { get; set; }



        public PeraPortSapWsClient(IDataAccess dataAccess)
            : base()
        {
            _dataAccess = dataAccess;
            _zRtEntPeraportClient = new ZRT_ENT_PERAPORTClient();
            UserName = "ZIYLAN";
            Password = "Floflo+Floflo+1";
        }

        public IDictionary<string, object> ChildServiceParameters { get; }
        public override string ServiceName { get; } = "Peraport ETL Service";

        public override void StartService()
        {
            ValidateServiceParameter();
            InitWebServiceClient();

            //Datayı çağıracağız

            //Datayı map edeceğiz

            //Datayı insert edeceğiz
        }

        private void InitWebServiceClient()
        {
            _zRtEntPeraportClient.ClientCredentials.UserName.UserName = UserName;
            _zRtEntPeraportClient.ClientCredentials.UserName.Password = Password;
        }

        private void ValidateServiceParameter()
        {
            if (UserName.IsNullAndWhiteSpace()) throw new SecurityException("UserName boş olamaz");
            if (Password.IsNullAndWhiteSpace()) throw new SecurityException("Password boş olamaz");
        }

        
    }
}
