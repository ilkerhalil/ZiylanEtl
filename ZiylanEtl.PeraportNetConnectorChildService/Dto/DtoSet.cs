using System.Collections.Generic;

namespace ZiylanEtl.PeraportChildService.Dto
{
    public class DtoSet
    {
        public DtoSet()
        {
            T023TDto = new List<T023tDto>();
            T134TDto = new List<T134tDto>();
            T6WstDto = new List<T6wstDto>();
            WrfBrandsTDto = new List<WrfBrandsTDto>();
            ZentLfa1Dto = new List<ZentLfa1Dto>();
            ZentT001Dto = new List<ZentT001Dto>();
            ZinventAsortiDto = new List<ZinventAsortiDto>();
            ZinventFytDto = new List<ZinventFytDto>();
            ZinventHrkDto = new List<ZinventHrkDto>();
            ZinventMlzDto = new List<ZinventMlzDto>();
            ZinventSasDto = new List<ZinventSasDto>();
            ZinventStokDto = new List<ZinventStokDto>();
            ZinventStokADto = new List<ZinventStokADto>();
            ZinventTesDto = new List<ZinventTesDto>();
            ZinventTrnDto = new List<ZinventTrnDto>();
            ZinventUyDto = new List<ZinventUyDto>();
        }

        public List<ZinventUyDto> ZinventUyDto { get; set; }

        public List<ZinventTrnDto> ZinventTrnDto { get; set; }

        public List<ZinventTesDto> ZinventTesDto { get; set; }

        public List<ZinventStokADto> ZinventStokADto { get; set; }

        public List<ZinventStokDto> ZinventStokDto { get; set; }

        public List<ZinventSasDto> ZinventSasDto { get; set; }

        public List<ZinventMlzDto> ZinventMlzDto { get; set; }

        public List<ZinventHrkDto> ZinventHrkDto { get; set; }

        public List<ZinventFytDto> ZinventFytDto { get; set; }

        public List<ZinventAsortiDto> ZinventAsortiDto { get; set; }

        public List<ZentT001Dto> ZentT001Dto { get; set; }

        public List<ZentLfa1Dto> ZentLfa1Dto { get; set; }

        public List<WrfBrandsTDto> WrfBrandsTDto { get; set; }

        public List<T6wstDto> T6WstDto { get; set; }

        public List<T134tDto> T134TDto { get; set; }

        public List<T023tDto> T023TDto { get; set; }
    }
}
