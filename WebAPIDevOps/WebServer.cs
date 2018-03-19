using Microsoft.Owin.Hosting;
using NLog;
using RazorEngine;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiDevOps.Server.Initialization;
using WebApiDevOps.Server.Managers;
using WebAPIDevOps.Core.Attributes;
using WebAPIDevOps.Core.Xml.Config;
using WebAPIDevOps.ORM;

namespace WebAPIDevOps.Server
{
    public class WebServer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public const string configFilePath = "./config.xml";


        [Variable]
        public static int WebAPIPort = 9000;
        [Variable]
        public static string WebAPIIP = "localhost";
        [Variable]
        public static string WebAPIKey = string.Empty;

        [Variable]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            Port = "3306",
            DbName = "devops_data",
            User = "root",
            Password = "",
            ProviderName = "MySql.Data.MySqlClient",
        };
        public XmlConfig Config
        {
            get;
            protected set;
        }
        public InitializationManager InitializationManager
        {
            get;
            protected set;
        }

        public DatabaseAccessor DBAccessor
        {
            get;
            protected set;
        }
        public struct hello
        {
            public int test;
            public string teste;
        }
        public void Initialize()
        {
            try
            {

                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);
                Config = new XmlConfig(configFilePath);
                this.Config.AddAssemblies(loadedAssemblies.Values.ToArray<Assembly>());
                if (!File.Exists(configFilePath))
                {
                    this.Config.Create(false);
                    this.logger.Info("Configuration file is created");
                }
                else
                {
                    this.Config.Load();
                    this.logger.Info("Configuration file is loaded !");
                }

                InitializationManager = InitializationManager.Instance;
                InitializationManager.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies());

                logger.Info("Initializing Database...");
                DBAccessor = new DatabaseAccessor(DatabaseConfiguration);
                DBAccessor.RegisterMappingAssembly(Assembly.GetExecutingAssembly());

                InitializationManager.Initialize(InitializationPass.Database);
                DBAccessor.Initialize();

                logger.Info("Opening Database...");
                DBAccessor.OpenConnection();
                DataManager.DefaultDatabase = DBAccessor.Database;
                DataManagerAllocator.Assembly = Assembly.GetExecutingAssembly();
                

                WebApp.Start<Startup>(url: $"http://{WebAPIIP}:{WebAPIPort}/");


                this.logger.Info($"WebServer started on http://{WebAPIIP}:{WebAPIPort}/");
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot start WebAPI: {ex.ToString()}");
            }
        }

        public class ErrorMessageResult : IHttpActionResult
        {
            private readonly string Message;
            private readonly HttpStatusCode StatusCode;

            public ErrorMessageResult(string message, HttpStatusCode statusCode)
            {
                Message = message;
                StatusCode = statusCode;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(StatusCode)
                {
                    Content = new StringContent(Message)
                };

                return Task.FromResult(response);
            }
        }

        public class HtmlActionResult : IHttpActionResult
        {
            private const string ViewDirectory = @"E:devConsoleApplication8ConsoleApplication8";
            private readonly string _view;
            private readonly dynamic _model;

            public HtmlActionResult(string viewName, dynamic model)
            {
                _view = LoadView(viewName);
                _model = model;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var parsedView = Razor.Parse(_view, _model);
                response.Content = new StringContent(parsedView);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return Task.FromResult(response);
            }

            private static string LoadView(string name)
            {
                var view = File.ReadAllText(Path.Combine(ViewDirectory, name + ".cshtml"));
                return view;
            }
        }
    }
}