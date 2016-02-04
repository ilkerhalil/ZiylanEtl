using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZiylanEtl.EtlWebService
{
    public class PeraportOperation
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<PeraportOperationResult> Result { get; set; }
    }
}
