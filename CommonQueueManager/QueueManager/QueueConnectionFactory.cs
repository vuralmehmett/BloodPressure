using RabbitMQ.Client;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CommonQueueManager.QueueManager
{
    public class QueueConnectionFactory
    {
        private static object syncObj = new object();

        private static Dictionary<int, QueueConnection> _connectionsDict = null;
        public static Dictionary<int, QueueConnection> ConnectionsDict
        {
            get
            {
                if (_connectionsDict == null)
                {
                    _connectionsDict = new Dictionary<int, QueueConnection>();
                }

                return _connectionsDict;
            }
            set { }
        }

        public static IConnection CreateConnection(int threadId)
        {
            lock (syncObj)
            { 
                var rabbitMqConnection = new Connector();
                var factory = rabbitMqConnection.RabbitMqConnection();
                var newConn = factory.CreateConnection();

                ConnectionsDict.Add(threadId, new QueueConnection
                {
                    Connection = newConn,
                    Channels = new List<IModel>()
                });

                return newConn;
            }
        }

        public static IConnection GetConnection(int threadId)
        {
            if (ConnectionsDict.ContainsKey(threadId))
            { 
                return ConnectionsDict[threadId].Connection;
            }

            throw new KeyNotFoundException();
        }

        public static IModel CreateChannel(int threadId, IConnection conn)
        {
            lock (syncObj)
            {
                if (ConnectionsDict.ContainsKey(threadId) &&
                    ConnectionsDict[threadId].Connection != conn)
                {
                    throw new NotSupportedException();
                }
            
                var newChannel = conn.CreateModel();

                ConnectionsDict[threadId].Channels.Add(newChannel);

                return newChannel;
            }
        }

        public static List<IModel> GetChannels(int threadId)
        {
            return ConnectionsDict[threadId].Channels;
        }

        public static IModel GetChannelPerThreadId(int threadId)
        {
            if (!ConnectionsDict.ContainsKey(threadId))
            {
                throw new KeyNotFoundException();
            }

            return ConnectionsDict[threadId].Channels.Single();
        }

        public static List<IModel> GetChannelsForConnection(IConnection conn)
        {
            foreach (var queueConn in ConnectionsDict)
            {
                if (queueConn.Value.Connection == conn)
                {
                    return queueConn.Value.Channels;
                }
            }

            throw new InvalidOperationException();
        }
    }

    public class QueueConnection
    {
        public IConnection Connection { get; set; }
        public List<IModel> Channels { get; set; }
    }
}
