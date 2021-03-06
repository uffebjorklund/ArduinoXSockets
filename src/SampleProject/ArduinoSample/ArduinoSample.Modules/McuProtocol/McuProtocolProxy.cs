﻿namespace ArduinoSample.Modules.McuProtocol
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using XSockets.Core.Common.Globals;
    using XSockets.Core.Common.Protocol;
    using XSockets.Core.Common.Socket.Event.Arguments;
    using XSockets.Core.Common.Socket.Event.Interface;
    using XSockets.Core.Common.Utility.Serialization;
    using XSockets.Core.Utility.Protocol.FrameBuilders;
    using XSockets.Core.XSocket.Model;
    using XSockets.Plugin.Framework;

    public class McuProtocolProxy : IProtocolProxy
    {
        private IXSocketJsonSerializer JsonSerializer { get; set; }

        public McuProtocolProxy()
        {
            JsonSerializer = Composable.GetExport<IXSocketJsonSerializer>();
        }

        public IMessage In(IEnumerable<byte> payload, MessageType messageType)
        {
            var data = Encoding.UTF8.GetString(payload.ToArray());
            if (data.Length == 0) return null;
            var d = data.Split('|');
            switch (d[1])
            {
                case "subscribe":
                case Constants.Events.PubSub.Subscribe:
                    return new Message(new XSubscription { Topic = d[2] }, Constants.Events.PubSub.Subscribe, d[0], JsonSerializer);
                case "unsubscribe":
                case Constants.Events.PubSub.Unsubscribe:
                    return new Message(new XSubscription { Topic = d[2] }, Constants.Events.PubSub.Unsubscribe, d[0], JsonSerializer);

                default:
                    return new Message(d[2], d[1], d[0], JsonSerializer);
            }
        }

        public byte[] Out(IMessage message)
        {
            if (message.Topic == Constants.Events.Controller.Opened)
            {
                var c = this.JsonSerializer.DeserializeFromString<ClientInfo>(message.Data);
                var d = $"PI:{c.PersistentId},CI:{c.ConnectionId}";
                message = new Message(d, message.Topic, message.Controller);
            }
            return new XDataFrame($"{message.Controller}|{message.Topic}|{message.Data}").ToBytes();
        }
    }
}