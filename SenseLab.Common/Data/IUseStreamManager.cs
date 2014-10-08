namespace SenseLab.Common.Data
{
    public interface IUseStreamManager
    {
        /// <summary>
        /// Stream manager which has to be provided for proper functionality.
        /// </summary>
        IStreamManager StreamManager { get; set; }
    }
}
