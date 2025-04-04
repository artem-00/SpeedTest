﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Desktop.Views
{
    public partial class MainWindow
    {
        private MainPage _mainPage = new();
        private readonly SettingsPage _settingsPage = new();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(_mainPage);
            Closing += MainWindow_Closing!;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not MainPage)
            {
                if (MainFrame.Content is SettingsPage)
                {
                    NavigateToPage(_mainPage, "SpeedTest", "../Resources/settings-50.png");
                }
            }
            else
            {
                NavigateToPage(_settingsPage, "Settings", "../Resources/home-50.png");
            }
        }

        private void NavigateToPage(Page page, string pageTitle, string imagePath)
        {
            MainFrame.Navigate(page);
            CurrentPageTitle.Text = pageTitle;

            UpdateUI(pageTitle, imagePath);
        }

        private void UpdateUI(string pageTitle, string imagePath)
        {
            CurrentPageTitle.Text = pageTitle;

            var bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            CurrentImage.Source = bitmapImage;
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            _mainPage = new MainPage();
            MainFrame.Navigate(_mainPage);

            UpdateUI("SpeedTest", "../Resources/settings-50.png");
        }


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (_settingsPage.ShowSpeedCheckBox.IsChecked == true)
            {
                _settingsPage.ShowSpeedCheckBox.IsChecked = false;
            }
        }
    }
}