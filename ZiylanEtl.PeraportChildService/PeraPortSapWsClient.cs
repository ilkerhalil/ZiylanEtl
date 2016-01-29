using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using AutoMapper;
using ZiylanEtl.Abstraction.Helper;
using ZiylanEtl.Abstraction.ServiceContracts;
using ZiylanEtl.DataAccess;
using ZiylanEtl.PeraportChildService.Dto;
using ZiylanEtl.PeraportChildService.ServiceProxy;

namespace ZiylanEtl.PeraportChildService
{
    public class PeraPortSapWsClient : BaseEtlChildService
    {
        private readonly IDataAccess _dataAccess;
        private readonly ZRT_ENT_PERAPORTClient _zRtEntPeraportClient;
        private readonly IMapper mapper;

        public string UserName { get; }

        public string Password { get; set; }



        public PeraPortSapWsClient(IDataAccess dataAccess)
            : base()
        {
            _dataAccess = dataAccess;
            _zRtEntPeraportClient = new ZRT_ENT_PERAPORTClient();
            UserName = "ZIYLAN";
            Password = "Floflo+Floflo+1";
            mapper = MapperInit();
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

        private IMapper MapperInit()
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
                con.CreateMap<ZinventMlz, ZinventMlzDto>();
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
