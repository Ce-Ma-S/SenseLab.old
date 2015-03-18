using CeMaS.Common.Validation;
using System.ServiceProcess;

namespace CeMaS.Common.Services
{
    public class ServiceHostService :
        ServiceBase
    {
        public ServiceHostService(ServiceHostController serviceHostController)
        {
            serviceHostController.ValidateNonNull(nameof(serviceHostController));
            ServiceHostController = serviceHostController;
            var serviceName = serviceHostController.ServiceHostName;
            if (serviceName.Length > MaxNameLength)
                serviceName = serviceName.Substring(0, MaxNameLength);
            ServiceName = serviceName;
            DisplayName = serviceHostController.ServiceHostName;
            StartType = ServiceStartMode.Automatic;
            Account = ServiceAccount.LocalService;
            AutoLog = true;
        }

        public ServiceHostController ServiceHostController { get; private set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string[] ServicesDependedOn { get; set; }
        public ServiceStartMode StartType { get; set; }
        public bool DelayedAutoStart { get; set; }
        public ServiceAccount Account { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ServiceProcessInstaller CreateInstaller()
        {
            var processInstaller = new ServiceProcessInstaller()
            {
                Account = Account,
                Username = Username,
                Password = Password
            };
            processInstaller.Installers.Add(
                new ServiceInstaller()
                {
                    ServiceName = ServiceName,
                    DisplayName = DisplayName,
                    ServicesDependedOn = ServicesDependedOn,
                    StartType = StartType,
                    DelayedAutoStart = DelayedAutoStart
                });
            return processInstaller;
        }

        protected override void OnStart(string[] args)
        {
            ServiceHostController.OpenServiceHost();
        }
        protected override void OnStop()
        {
            ServiceHostController.CloseServiceHost();
        }
    }
}
