using System;

namespace Aurora.SMS.Providers
{
    public interface IClientProviderFactory
    {
        ISMSClientProxy CreateClient(string providerName,
                                                       string username,
                                                       string password);
    }

    /// <summary>
    /// Factory for creating HttpClients to communicate with the Http  SMS providers
    /// </summary>
    public class ClientProviderFactory : IClientProviderFactory
    {
        public ISMSClientProxy CreateClient(string providerName,
                                                         string username,
                                                         string password)
        {
            switch (providerName.ToLower())
            {
                case "snailabroad":
                    return new SnailAbroadProxy(username, password);

                default:
                    throw new Exception(string.Format("The provider with name {0} has not been registered!", providerName));
            }
        }
    }
}