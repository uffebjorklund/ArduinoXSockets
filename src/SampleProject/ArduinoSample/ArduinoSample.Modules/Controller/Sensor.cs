namespace ArduinoSample.Modules.Controller
{
    using XSockets.Core.XSocket;
    using System.Threading.Tasks;
    using XSockets.Core.XSocket.Helpers;

    /// <summary>
    /// Sensor (Arduino in this case) sends data to this controller
    /// </summary>
    public class Sensor : XSocketController
    {
        public async Task Data(int value)
        {
            //send data to monitoring clients
            await this.InvokeToAll<Monitor>(value, "data");
        }
    }
}
