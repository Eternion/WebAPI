﻿using WebAPIDevOps.ORM;
using WebAPIDevOps.ORM.SubSonic.SQLGeneration.Schema;

namespace WebAPIDevOps.Server.Databases.Texts
{
    public class TextRecordRelator
    {
        public const string FetchQueries = "SELECT * FROM texts";
    }
    [TableName("texts")]
    public class TextRecord : IAutoGeneratedRecord
    {
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        public string French
        {
            get;
            set;
        }
        public string English
        {
            get;
            set;
        }
        public string Spanish
        {
            get;
            set;
        }
        public string Italian
        {
            get;
            set;
        }
        public string German
        {
            get;
            set;
        }
    }
}