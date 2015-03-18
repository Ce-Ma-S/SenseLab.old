using CeMaS.Common.Validation;
using SenseLab.Common.Records;
using SenseLab.Environments.Service;
using System;
using System.Threading.Tasks;

namespace SenseLab.Environments.Remote
{
    public class Recordable :
        SenseLab.Common.Records.Recordable<RecordProvider>
    {
        private Recordable(Environment environment, IEnvironmentService service,
            Guid id, IRecordType type, string name, string description, bool isAvailable)
            : base(id, type, name, description)
        {
            this.environment = environment;
            this.service = service;
            IsAvailable = IsAvailable;
        }

        internal static async Task<Recordable> Create(Guid id, Environment environment, IEnvironmentService service)
        {
            service.ValidateNonNull(nameof(service));
            var info = await service.Recordable(id);
            var result = new Recordable(environment, service,
                id, info.Type, info.Name, info.Description, info.IsAvailable);
            return result;
        }

        public new bool IsAvailable { get; private set; }

        public override async Task<RecordProvider> CreateRecordProvider()
        {
            return await RecordProvider.Create(this, environment, service);
        }

        protected override bool GetIsAvailable()
        {
            return IsAvailable;
        }

        private readonly Environment environment;
        private readonly IEnvironmentService service;
    }
}