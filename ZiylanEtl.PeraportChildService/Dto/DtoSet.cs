using System.Collections.Generic;

namespace ZiylanEtl.PeraportChildService.Dto
{
    public class DtoSet
    {
        public DtoSet()
        {
            T023tDtos = new List<T023tDto>();
            T134TDtos = new List<T134tDto>();
            T6WstDtos = new List<T6wstDto>();
            WrfBrandsTDtos = new List<WrfBrandsTDto>();
            ZentLfa1Dtos = new List<ZentLfa1Dto>();
            ZentT001Dtos = new List<ZentT001Dto>();
            ZinventAsortiDtos = new List<ZinventAsortiDto>();
            ZinventFytDtos = new List<ZinventFytDto>();
            ZinventHrkDtos = new List<ZinventHrkDto>();
            ZinventMlzDtos = new List<ZinventMlzDto>();
            ZinventSasDtos = new List<ZinventSasDto>();
            ZinventStokDtos = new List<ZinventStokDto>();
            ZinventStokADtos = new List<ZinventStokADto>();
            ZinventTesDtos = new List<ZinventTesDto>();
            ZinventTrnDtos = new List<ZinventTrnDto>();
            ZinventUyDtos = new List<ZinventUyDto>();
        }

        public List<ZinventUyDto> ZinventUyDtos { get; set; }

        public List<ZinventTrnDto> ZinventTrnDtos { get; set; }

        public List<ZinventTesDto> ZinventTesDtos { get; set; }

        public List<ZinventStokADto> ZinventStokADtos { get; set; }

        public List<ZinventStokDto> ZinventStokDtos { get; set; }

        public List<ZinventSasDto> ZinventSasDtos { get; set; }

        public List<ZinventMlzDto> ZinventMlzDtos { get; set; }

        public List<ZinventHrkDto> ZinventHrkDtos { get; set; }

        public List<ZinventFytDto> ZinventFytDtos { get; set; }

        public List<ZinventAsortiDto> ZinventAsortiDtos { get; set; }

        public List<ZentT001Dto> ZentT001Dtos { get; set; }

        public List<ZentLfa1Dto> ZentLfa1Dtos { get; set; }

        public List<WrfBrandsTDto> WrfBrandsTDtos { get; set; }

        public List<T6wstDto> T6WstDtos { get; set; }

        public List<T134tDto> T134TDtos { get; set; }

        public List<T023tDto> T023tDtos { get; set; }
    }
}
