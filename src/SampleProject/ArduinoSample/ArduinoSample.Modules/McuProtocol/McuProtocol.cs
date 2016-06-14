
namespace ArduinoSample.Modules.McuProtocol
{
    using System.Collections.Generic;
    using XSockets.Core.Common.Protocol;
    using XSockets.Core.Common.Socket.Event.Arguments;
    using XSockets.Core.Common.Socket.Event.Interface;
    using XSockets.Plugin.Framework;
    using XSockets.Plugin.Framework.Attributes;
    using XSockets.Protocol;

    /// <summary>
    /// Simple protocol for a simple MCU client
    /// </summary>
    [Export(typeof(IXSocketProtocol), Rewritable = Rewritable.No)]
    public class McuProtocol : XSocketProtocol
    {
        public McuProtocol()
        {
            this.ProtocolProxy = new McuProtocolProxy();
        }

        /// <summary>
        /// The string to return after handshake
        /// </summary>
        public override string HostResponse => "McuProtocol";

        /// <summary>
        /// Set to true if your clients connected to this protocol will return pong on ping.
        /// </summary>
        /// <returns></returns>
        public override bool CanDoHeartbeat()
        {
            return false;
        }
        

        /// <summary>
        /// Used from handshake handler
        /// </summary>
        /// <returns></returns>
        public override IXSocketProtocol NewInstance()
        {
            return new McuProtocol();
        }

        /// <summary>
        /// Converts the incomming data from MCU into a IMessage
        /// The data is expected to be in the format "controller|topic|data"
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public override IMessage OnIncomingFrame(IEnumerable<byte> payload, MessageType messageType)
        {
            return this.ProtocolProxy.In(payload, messageType);
        }

        /// <summary>
        /// Converts a IMessage into a string to send to MCU.
        /// MCU will receive the data in the format "controller|topic|data"        
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override byte[] OnOutgoingFrame(IMessage message)
        {
            return this.ProtocolProxy.Out(message);
        }
    }
}
