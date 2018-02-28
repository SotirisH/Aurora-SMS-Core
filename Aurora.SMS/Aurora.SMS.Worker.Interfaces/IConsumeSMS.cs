using System.Threading.Tasks;

namespace Aurora.SMS.Worker.Interfaces
{
    public interface IConsumeSMS
    {
        /// <summary>
        /// Reads the SQS and sends the messages to the provider
        /// </summary>
        /// <returns></returns>
        Task<bool> Execute();
    }
}