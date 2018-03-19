using WebAPIDevOps.ORM.SubSonic.DataProviders;
using WebAPIDevOps.ORM.SubSonic.Schema;

namespace WebAPIDevOps.ORM
{
    public interface IManualGeneratedRecord
    {
        ITable GetTableInformation(IDataProvider provider); 
    }
}