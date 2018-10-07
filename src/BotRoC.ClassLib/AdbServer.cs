using System;
using SharpAdbClient;

namespace BotRoC.ClassLib
{
    public class AdbServer
    {
        private DeviceData device;
        public AdbServer() { }

        public int StartServer()
        {
            AdbClient.CreateAdbForwardRequest("localhost", 21503);
            device = AdbClient.Instance.GetDevices()[0];
            if (device != null) {
                Console.Write(device);
                return 0;
            }
            else {
                Console.WriteLine("Error : No device connected");
                return 1;
            }
        }

        public int GetScreenSize()
        {
            var receiver = new ConsoleOutputReceiver();

            AdbClient.Instance.ExecuteRemoteCommand("wm size", device, receiver);
            Console.WriteLine(receiver);
            AdbClient.Instance.ExecuteRemoteCommand("input tap 50 700", device, receiver);
            return 0;
        }
        public int ListenToServer()
        {
        
            return 0;
        }
    }
}
