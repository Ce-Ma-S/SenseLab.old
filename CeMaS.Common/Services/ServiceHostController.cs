using CeMaS.Common.Validation;
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;

namespace CeMaS.Common.Services
{
    public class ServiceHostController :
        Disposable
    {
        public ServiceHostController(ServiceHostBase serviceHost)
        {
            serviceHost.ValidateNonNull(nameof(serviceHost));
            ServiceHost = serviceHost;
            Service = new ServiceHostService(this);
        }

        public ServiceHostBase ServiceHost { get; private set; }
        public ServiceHostService Service { get; private set; }

        public virtual void Run(params string[] arguments)
        {
            if (HasFlag(arguments, "install", "i"))
                InstallService();
            else if (HasFlag(arguments, "uninstall", "u"))
                UninstallService();
            else if (Environment.UserInteractive)
                RunDirectly();
            else
                RunAsWindowsService();
        }

        protected internal string ServiceHostName
        {
            get
            {
                return string.Format("{0}.{1}",
                    ServiceHost.Description.Namespace,
                    ServiceHost.Description.Name);
            }
        }
        protected string ServiceHostTitle
        {
            get
            {
                return string.Format("Service '{0}':",
                    ServiceHostName);
            }
        }

        protected bool HasFlag(string[] arguments, params string[] names)
        {
            if (arguments == null || arguments.Length == 0)
                return false;
            foreach (var name in names)
            {
                var n = name.ToLower();
                if (arguments.Any(
                    a =>
                    {
                        a = a.ToLower();
                        return
                            a == '/' + n ||
                            a == '\\' + n ||
                            a == '-' + n;
                    }))
                {
                    return true;
                }
            }
            return false;
        }
        protected virtual void InstallService()
        {
            var installer = Service.CreateInstaller();
            installer.Install(null);
        }
        protected virtual void UninstallService()
        {
            var installer = Service.CreateInstaller();
            installer.Uninstall(null);
        }
        protected internal virtual void OpenServiceHost()
        {
            Console.WriteLine(ServiceHostTitle);
            Console.WriteLine(string.Format("Opening at\n{0}",
                string.Join("\n", ServiceHost.Description.Endpoints.Select(ep => ep.Address))));
            try
            {
                ServiceHost.Open();
                Console.WriteLine("Opened");
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }
        protected internal virtual void CloseServiceHost()
        {
            Console.WriteLine(ServiceHostTitle);
            Console.WriteLine("Closing");
            try
            {
                ServiceHost.Close();
                Console.WriteLine("Closed");
            }
            catch (Exception e)
            {
                ServiceHost.Abort();
                OnError(e);
            }
        }
        protected virtual void OnError(Exception e)
        {
            Console.WriteLine("Error occured:\n" + e.ToString());
        }
        protected virtual void RunDirectly()
        {
            OpenServiceHost();
        }
        protected virtual void RunAsWindowsService()
        {
            ServiceBase.Run(Service);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                CloseServiceHost();
        }
    }


    public class ServiceHostController<T> :
        ServiceHostController
        where T : new()
    {
        public ServiceHostController()
            : base(
                  new ServiceHost(
                      new T()
                      ))
        {
        }
    }
}
