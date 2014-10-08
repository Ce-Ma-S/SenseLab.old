namespace SenseLab.Common.Data
{
    public class StreamInfo
    {
        public StreamInfo(string namespaceName, string name)
        {
            namespaceName.ValidateNonNullOrEmpty("namespaceName");
            name.ValidateNonNullOrEmpty("name");
            NamespaceName = namespaceName;
            Name = name;
        }

        public string NamespaceName { get; private set; }
        public string Name { get; private set; } 
    }
}
