using System;
using System.Threading;
using System.Drawing;
using SharpAdbClient;

namespace BotRoC.ConsoleApp
{
    public class AdbClass
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AdbServer server;
        private AdbClient client;
        private DeviceData device;
        // private Framebuffer buffer;
        public AdbClass() { }

        public int StartServer()
        {
            try
            {
                log.Info("Tentative de connexion à l'émulateur");
                server = new AdbServer();
                var result = server.StartServer(@"./resources/Android/adb.exe", restartServerIfNewer: false);
                this.client = (AdbClient)AdbClient.Instance;            // AdbClient.Instance.CreateAdbForwardRequest("localhost", 21503);
                device = AdbClient.Instance.GetDevices()[0];
                log.Info("Connecté à " + device);
                return 0;
            }
            catch (Exception e)
            {
                log.Error("Erreur lors de la connexion : " + e);
                return -1;
            }
        }

        public Point GetScreenSize()
        {
            var receiver = new ConsoleOutputReceiver();

            client.ExecuteRemoteCommand("wm size", device, receiver);
            string[] strArr = receiver.ToString().Split(" ")[2].Split("x");
            int[] intArr = Array.ConvertAll(strArr, Int32.Parse);
            var endPoint = new Point(intArr[0], intArr[1]);
            log.Info("La résolution est de : " + endPoint.X + "x" + endPoint.Y);
            return (endPoint);
        }

        public void TouchPoint(Point point)
        {
            var receiver = new ConsoleOutputReceiver();

            log.Info("Tap (point)" + point.X + " " + point.Y);
            client.ExecuteRemoteCommand("input tap " + point.X + " " + point.Y, device, receiver);
        }

        public void TouchRectangle(Rectangle rectangle)
        {
            var receiver = new ConsoleOutputReceiver();

            int xCenter = rectangle.X + (rectangle.Width / 2);
            int yCenter = rectangle.Y + (rectangle.Height / 2);
            log.Info("Tap (rectangle) " + xCenter + " " + yCenter);
            client.ExecuteRemoteCommand("input tap " + xCenter + " " + yCenter, device, receiver);
        }

        public Bitmap GetScreenShot()
        {
            client.GetFrameBufferAsync(device, CancellationToken.None).Result.Save("screenshot.jpg", System.Drawing.Imaging.ImageFormat.Png);
            return (Bitmap)client.GetFrameBufferAsync(device, CancellationToken.None).Result;
        }
    }
}
