using System;

namespace WebAPIDevOps.ORM.SubSonic.DataProviders.Log
{
    public interface ILogAdapter
    {
        void Log(String message);
    }
}