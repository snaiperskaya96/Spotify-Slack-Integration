using Qml.Net;
using Qml.Net.Runtimes;

namespace SpotifySlackIntegration
{
    class Program
    {
        static int Main(string[] args)
        {
            RuntimeManager.DiscoverOrDownloadSuitableQtRuntime();
            QQuickStyle.SetStyle("Material");
            using (var application = new QGuiApplication(args))
            {
                using (var qmlEngine = new QQmlApplicationEngine())
                {
                    Qml.Net.Qml.RegisterType<TrayModel>("app");
                    qmlEngine.Load("TrayIcon.qml"); 
                    return application.Exec();
                }
            }
        }
    }
}
