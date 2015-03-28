using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Records;
using SenseLab.Environments.Service;
using System;
using System.Threading.Tasks;

namespace SenseLab.Environments.Remote
{
    public class Recordable :
        SenseLab.Common.Records.Recordable<RecordProvider>
    {
        protected Recordable(Environment environment, IEnvironmentService service,
            Guid id, IRecordType type, string name, string description, bool isAvailable)
            : base(id, type, name, description)
        {
            this.environment = environment;
            Service = service;
            IsAvailable = IsAvailable;
        }

        internal static async Task<Recordable> Create(Guid id, Environment environment, IEnvironmentService service)
        {
            service.ValidateNonNull(nameof(service));
            var info = await service.Recordable(id);
            Recordable result;
            if (info is RecordableCommandInfo)
            {
                var commandInfo = (RecordableCommandInfo)info;
                result = new RecordableCommand(environment, service,
                    id, info.Type, info.Name, info.Description, info.IsAvailable,
                    commandInfo.IsSynchronous);
            }
            else
            {
                result = new Recordable(environment, service,
                    id, info.Type, info.Name, info.Description, info.IsAvailable);
            }
            environment.AddRecordable(result);
            return result;
        }

        #region IsAvailable

        public new bool IsAvailable
        {
            get { return isAvailable; }
            internal set
            {
                SetProperty(() => IsAvailable, ref isAvailable, value, OnIsAvailableChanged);
            }
        }

        protected override bool GetIsAvailable()
        {
            return IsAvailable;
        }

        private bool isAvailable;

        #endregion

        public override async Task<RecordProvider> CreateRecordProvider()
        {
            return await RecordProvider.Create(this, environment, Service);
        }

        protected readonly IEnvironmentService Service;

        private readonly Environment environment;
    }
}