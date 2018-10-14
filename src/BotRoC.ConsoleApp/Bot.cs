using System;
using System.Threading;
using System.Drawing;

namespace BotRoC.ConsoleApp
{
    public class Bot
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AdbClass adbClass;
        private String resolutionPath;
        private Point ScoutBase;

        public Bot()
        {
            log.Info("Démarrage du bot...");
            this.adbClass = new AdbClass();

            Point resolution = this.adbClass.GetScreenSize();
            this.resolutionPath = "resources/Resolutions/" + resolution.X + 'x' + resolution.Y + '/';
            log.Info("Le dossier de ressource utilisé est : " + this.resolutionPath);
        }
        public void StartGame()
        {
            log.Info("Lancement du jeu...");
            this.TouchImage("App.jpg");
            while (1 == 1)
            {
                try
                {
                    Thread.Sleep(5000);
                    Rectangle home1 = FindImage("Home1.jpg");
                    break;
                }
                catch (Exception ImageNotFound)
                {
                    log.Info("Lancement en cours...");
                }
            }
            log.Info("Jeu lancé !");
        }

        private Rectangle FindImage(string path)
        {
            log.Debug("Recherche sur l'écran de l'image : " + this.resolutionPath + path);
            Bitmap needle = ImageUtil.OpenImage(this.resolutionPath + path);
            Bitmap screen = adbClass.GetAdbScreen();
            Rectangle image = ImageUtil.SearchBitmap(needle, screen, 0.0);
            if (image.Width != 0)
                return image;
            else
                throw new ImageNotFound();
        }
        private void TouchImage(string path)
        {
            Rectangle image = this.FindImage(path);
            adbClass.TouchRectangle(image);
        }

        public AdbClass GetAdbClass()
        {
            return this.adbClass;
        }
    }

    class ImageNotFound : Exception
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageNotFound()
        {
            log.Debug("Image non retrouvée");
        }
    }
}
