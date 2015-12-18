using System.Net.Sockets;
/*
   Author:Sagara
*/
namespace Commons.Utils
{
    public static class NetworkExt
    {
        public delegate void OnReceived(object state, byte[] buffer);

        public static void AsyncReceiveFixed(this Socket socket, byte[] buffer,
            OnReceived receivedCallback, object state)
        {
            AsyncReceiveFixed(socket, buffer, 0, buffer.Length, receivedCallback, state);
        }

        public static void AsyncReceiveFixed(this Socket socket, byte[] buffer, int offset, int len, OnReceived receivedCallback, object state)
        {
            socket.BeginReceive(buffer, offset, len, SocketFlags.None, ar =>
            {
                var received = socket.EndReceive(ar);
                if (received == 0)
                    receivedCallback.Invoke(state, null);

                if (received < len)
                    AsyncReceiveFixed(socket, buffer, offset + received, len - received, receivedCallback, state);
                else
                    receivedCallback.Invoke(state, buffer);

            }, state);
        }
    }
}
