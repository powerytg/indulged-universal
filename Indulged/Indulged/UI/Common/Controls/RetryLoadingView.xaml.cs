using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.Controls
{
    public sealed partial class RetryLoadingView : UserControl
    {
        public Action LoadingAction;

        /// <summary>
        /// Title property
        /// </summary>
        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register(
        "LoadingText",
        typeof(string),
        typeof(RetryLoadingView),
        new PropertyMetadata("Loading...", OnLoadingTextPropertyChanged));

        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        private static void OnLoadingTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RetryLoadingView)sender;
            target.OnLoadingTextChanged();
        }

        private void OnLoadingTextChanged()
        {
            LoadingLabel.Text = LoadingText;
        }

        /// <summary>
        /// Error text property
        /// </summary>
        public static readonly DependencyProperty ErrorTextProperty = DependencyProperty.Register(
        "ErrorText",
        typeof(string),
        typeof(RetryLoadingView),
        new PropertyMetadata("An error has occurred during the operation", OnErrorTextPropertyChanged));

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        private static void OnErrorTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RetryLoadingView)sender;
            target.OnErrorTextChanged();
        }

        private void OnErrorTextChanged()
        {
            LoadingLabel.Text = ErrorText;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RetryLoadingView()
        {
            this.InitializeComponent();
        }

        public void ShowLoadingScreen()
        {
            RetryButton.Visibility = Visibility.Collapsed;
            LoadingIndicator.Visibility = Visibility.Visible;

            if (LoadingText != null)
            {
                LoadingLabel.Text = LoadingText;
            }            
        }

        public void ShowRetryScreen()
        {
            RetryButton.Visibility = Visibility.Visible;
            LoadingIndicator.Visibility = Visibility.Collapsed;

            if (ErrorText != null)
            {
                LoadingLabel.Text = ErrorText;
            }            
        }

        public void Destroy()
        {
            LoadingIndicator.IsActive = false;
            this.Visibility = Visibility.Collapsed;
            LoadingAction = null;
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            if(LoadingAction != null)
            {
                ShowLoadingScreen();
                LoadingAction();
            }
        }

    }
}
