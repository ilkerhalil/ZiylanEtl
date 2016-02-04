using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZiylanEtl.EtlWebService
{
    public class PeraportOperationResult
    {
        public int DataLength { get; set; }
        public int RecordCount { get; set; }
        public string TableName { get; set; }
        public string Filtre { get; set; }
        public DateTime WebserviceStartTime { get; set; }
        public DateTime WebserviceEndTime { get; set; }
        public TimeSpan WebserviceDataTransferDuration { get; set; }
        public DateTime DatabaseInsertStartTime { get; set; }
        public DateTime DatabaseInsertEndTime { get; set; }
        public TimeSpan DatabaseInsertDuration { get; set; }
    }
}
