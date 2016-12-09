using System;
using NetworksApi.TCP.CLIENT;

namespace AdvanceClient
{
    internal class OnClientDisconnectedDelegete
    {
        private Action<object, ClientDisconnectedArguments> client_OnClientDisconnected;

        public OnClientDisconnectedDelegete(Action<object, ClientDisconnectedArguments> client_OnClientDisconnected)
        {
            this.client_OnClientDisconnected = client_OnClientDisconnected;
        }
    }
}