using DemoExam.Timer;
using DemoExam.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DemoExam;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static SessionTimer SessionTimer { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        SessionTimer = new SessionTimer(TimeSpan.FromSeconds(62));
        SessionTimer.sessionEnded += OnSessionEnded;
    }

    public void OnSessionEnded(object sender, EventArgs e)
    {
        MessageBox.Show("Время закончилось!");
        Application.Current.Properties["CurrentUser"] = null;

        Auth a = new Auth();
        a.Show();

        foreach (Window w in Application.Current.Windows)
        {
            if (w is not Views.Auth)
            {
                w.Close();
            }
        }
        return;
    }

}

