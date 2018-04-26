using System;
using System.Diagnostics;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ServerSocket
{
    internal class SocketServer
    {
        private readonly int _port;
        public int Port { get { return _port; } }

        private StreamSocketListener listener;

        public delegate void DataRecived(string data);
        public event DataRecived OnDataRecived;

        public delegate void Error(string message);
        public event Error OnError;

        public SocketServer(int port)
        {
            _port = port;
        }

        public async void Star()
        {
            try
            {
                //Fecha a conexo com a porta que est escutando atualmente
                if (listener != null)
                {
                    await listener.CancelIOAsync();
                    listener.Dispose();
                    listener = null;
                }

                //Criar uma nova instancia do listerner
                listener = new StreamSocketListener();

                //Adiciona o evento de conexo recebida ao mtodo Listener_ConnectionReceived
                listener.ConnectionReceived += Listener_ConnectionReceived;
                //Espera fazer o bind da porta
                await listener.BindServiceNameAsync(Port.ToString());
            }
            catch (Exception e)
            {
                //Caso acontea um erro, dispara o evento de erro
                if (OnError != null)
                    OnError(e.Message);
            }
        }

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            var reader = new DataReader(args.Socket.InputStream);
            try
            {
                while (true)
                {
                    uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                    //Caso ocora um desconexo
                    if (sizeFieldCount != sizeof(uint))
                        return;

                    //Tamanho da string
                    uint stringLenght = reader.ReadUInt32();
                    //Ler os dados do InputStream
                    uint actualStringLength = await reader.LoadAsync(stringLenght);
                    //Caso ocora um desconexo
                    if (stringLenght != actualStringLength)
                        return;
                    //Dispara evento de dado recebido
                    if (OnDataRecived != null)
                    {
                        //Le a string com o tamanho passado
                        string data = reader.ReadString(actualStringLength);
                        //Dispara evento de dado recebido
                        OnDataRecived(data);
                    }
                }

            }
            catch (Exception ex)
            {
                // Dispara evento em caso de erro, com a mensagem de erro
                if (OnError != null)
                    OnError(ex.Message);
            }
        }
    }
}
