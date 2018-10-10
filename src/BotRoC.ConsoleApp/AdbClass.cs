using System;
using System.Threading;
using System.Drawing;
using SharpAdbClient;

namespace BotRoC.ConsoleApp
{
    public class AdbClass
    {
        private AdbServer server;
        private AdbClient client;
        private DeviceData device;
        private Framebuffer buffer;
        public AdbClass() { }

        public int StartServer()
        {
            try
            {
                server = new AdbServer();
                var result = server.StartServer(@"C:\Android\adb.exe", restartServerIfNewer: false);
                this.client = (AdbClient)AdbClient.Instance;            // AdbClient.Instance.CreateAdbForwardRequest("localhost", 21503);
                device = AdbClient.Instance.GetDevices()[0];
                if (device != null)
                {
                    Console.WriteLine(device);
                    return 0;
                }
                else
                {
                    Console.WriteLine("No device");
                    return -1;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public string GetScreenSize()
        {
            var receiver = new ConsoleOutputReceiver();

            client.ExecuteRemoteCommand("wm size", device, receiver);
            return (receiver.ToString().Split(" ")[2]);
        }

        public void TouchScreen(int x, int y)
        {
            var receiver = new ConsoleOutputReceiver();

            client.ExecuteRemoteCommand("input tap " + x + " "+  y, device, receiver);
        }

        public Bitmap GetScreenShot()
        {
            // client.GetFrameBufferAsync(device, CancellationToken.None).Result.Save("screenshot.jpg", System.Drawing.Imaging.ImageFormat.Png);
            return (Bitmap)client.GetFrameBufferAsync(device, CancellationToken.None).Result;
        }
    }
}
