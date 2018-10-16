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
        private Point screenSize;
        
        public AdbClass()
        {
            this.StartAdbServer();
            this.screenSize = this.GetAdbScreenSize();
        }

        private void StartAdbServer()
        {
            try
            {
                log.Info("Trying to connect to emulator");
                server = new AdbServer();
                server.StartServer(@"./resources/Android/adb.exe", restartServerIfNewer: false);
                client = (AdbClient)AdbClient.Instance;            // AdbClient.Instance.CreateAdbForwardRequest("localhost", 21503);
                device = AdbClient.Instance.GetDevices()[0];
                log.Info("Connected to : " + device);
            }
            catch (Exception e)
            {
                log.Error("Connexion error : " + e.Data);
            }
        }

        private Point GetAdbScreenSize()
        {
            var receiver = new ConsoleOutputReceiver();

            this.client.ExecuteRemoteCommand("wm size", device, receiver);
            string[] strArr = receiver.ToString().Split(" ")[2].Split("x");
            int[] intArr = Array.ConvertAll(strArr, Int32.Parse);
            var endPoint = new Point(intArr[0], intArr[1]);
            log.Info("Resolution : " + endPoint.X + "x" + endPoint.Y);
            return (endPoint);
        }

        public void TouchPoint(Point point)
        {
            var receiver = new ConsoleOutputReceiver();

            log.Debug("Tap (point)" + point.X + " " + point.Y);
            client.ExecuteRemoteCommand("input tap " + point.X + " " + point.Y, device, receiver);
        }

        public void TouchRectangle(Rectangle rectangle)
        {
            var receiver = new ConsoleOutputReceiver();

            int xCenter = rectangle.X + (rectangle.Width / 2);
            int yCenter = rectangle.Y + (rectangle.Height / 2);
            log.Debug("Tap (rectangle) " + xCenter + " " + yCenter);
            client.ExecuteRemoteCommand("input tap " + xCenter + " " + yCenter, device, receiver);
        }

        public Bitmap GetAdbScreen()
        {
            return (Bitmap)client.GetFrameBufferAsync(device, CancellationToken.None).Result;
        }

        public void TakeScreenShot(string path)
        {
            client.GetFrameBufferAsync(device, CancellationToken.None).Result.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

        public Point GetScreenSize()
        {
            return this.screenSize;
        }
    }
}
