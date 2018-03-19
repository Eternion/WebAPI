using System.Reflection;
using WebApiDevOps.Core.Extensions;
using WebApiDevOps.Server.Initialization;
using WebAPIDevOps.Core.Reflection;
using WebAPIDevOps.ORM;

namespace WebApiDevOps.Server.Managers
{
    public abstract class DataManager
    {
        public static Database DefaultDatabase;

        private Database m_database;

        public Database Database
        {
            get
            {
                return m_database ?? DefaultDatabase;
            }
        }

        public void ChangeDataSource(Database datasource)
        {
            if (m_database == null)
                m_database = datasource;
            else
            {
                m_database = datasource;

                TearDown();
                Initialize();
            }
        }

        public virtual void Initialize()
        {
        }

        public virtual void TearDown()
        {
        }
    }

    public abstract class DataManager<T> : Singleton<T> where T : class
    {
        private Database m_database;

        public Database Database
        {
            get
            {
                return m_database ?? DataManager.DefaultDatabase;
            }
        }

        public void ChangeDataSource(Database datasource)
        {
            if (m_database == null)
                m_database = datasource;
            else
            {
                m_database = datasource;

                TearDown();
                Initialize();
            }
        }

        public abstract void Initialize();

        public virtual void TearDown()
        {
        }
    }

    public static class DataManagerAllocator
    {
        public static Assembly Assembly;

        [Initialization(InitializationPass.First, "Initialize DataManagers")]
        public static void Initialize()
        {
            foreach (var type in Assembly.GetTypes())
            {
                if (type.IsAbstract || !type.IsSubclassOfGeneric(typeof (DataManager<>)) ||
                    type == typeof (DataManager<>)) continue;

                var method = type.GetMethod("Initialize", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                // if the method is already managed we don't call it
                if (method.GetCustomAttribute<InitializationAttribute>(true) != null)
                    continue;

                object instance = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static).
                    GetValue(null, new object[0]);
                method.Invoke(instance, new object[0]);
            }
        }
    }
}