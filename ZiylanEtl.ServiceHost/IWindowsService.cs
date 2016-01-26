namespace ZiylanEtl.ServiceHost
{
    public  interface IWindowsService
    {
        void Start();
        void Stop();
        void Pause();
        void Continue();
    }
}