using System;
using ZilyanEtl.InventWebServiceClient.Proxy;

namespace ZilyanEtl.InventWebServiceClient
{
    public class SapWsClient : IDisposable
    {
        private ZRT_ENT_PERAPORTClient _client;

        public SapWsClient()
        {
            _client = new ZRT_ENT_PERAPORTClient();
        }




        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool b)
        {
            if (!b) return;
            try
            {
                _client.Close();
            }
            finally
            {
                _client.Abort();
            }
            _client = null;
        }

        #endregion

    }
}
