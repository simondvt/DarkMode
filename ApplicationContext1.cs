using DarkMode.Properties;
using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DarkMode
{
    public class ApplicationContext1 : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private bool isDark = false;

        public ApplicationContext1()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.MoonIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            // Events handlers
            trayIcon.Click += TrayIcon_Click;

            // Get current value to display on Text
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            isDark = key.GetValue("AppsUseLightTheme").ToString() == "0";
            key.Close();

            if (isDark)
                trayIcon.Text = "DarkMode ON";
            else
                trayIcon.Text = "DarkMode OFF";

            // Set up timers

        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            if (isDark)
                LightMode();
            else
                DarkMode();

            isDark = !isDark;
        }

        private void Exit(object sender, EventArgs e)
        {
            // Il click sull exit triggera anche il click sull icona, quindi
            // chiamo ancora trayicon_click per lasciare inalterato lo stato 
            // quando chiudo.
            TrayIcon_Click(null, null);

            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }

        private void SetTheme(int value)
        {
            RegistryKey key;

            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            key.SetValue("AppsUseLightTheme", value);
            key.Close();
        }

        private void DarkMode()
        {
            SetTheme(0);
            trayIcon.Text = "DarkMode ON";
        }

        private void LightMode()
        {
            SetTheme(1);
            trayIcon.Text = "DarkMode OFF";
        }
    }
}
