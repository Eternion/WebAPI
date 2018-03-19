namespace WebAPIDevOps.ORM
{
    public interface ISaveIntercepter
    {
        void BeforeSave(bool insert);
    }
}