using System.Collections.Generic;

namespace ZiylanEtl.Abstraction.ServiceContracts
{
    public abstract class BaseEtlChildService : IEtlChildService
    {
        public abstract string ServiceName { get; }
        public abstract void StartService();

        protected BaseEtlChildService()
        {
            ChildServiceParameters = new Dictionary<string, object>();
        }

        public void StartService(out string logContent)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<string, object> ChildServiceParameters { get; private set; }
    }
}