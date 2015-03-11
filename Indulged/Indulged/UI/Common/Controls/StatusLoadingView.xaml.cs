﻿using System;
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
    public sealed partial class StatusLoadingView : LoadingViewBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StatusLoadingView()
        {
            this.InitializeComponent();
        }

        protected override void OnThemeColorChanged()
        {
            base.OnThemeColorChanged();
            LoadingLabel.Foreground = ThemeColor;
            LoadingIndicator.Foreground = ThemeColor;
        }

        protected override void OnLoadingTextChanged()
        {
            base.OnLoadingTextChanged();
            LoadingLabel.Text = LoadingText;
        }

        public void ShowErrorScreen()
        {
            LoadingIndicator.Visibility = Visibility.Collapsed;

            if (ErrorText != null)
            {
                LoadingLabel.Text = ErrorText;
            }
        }

        public void ShowLoadingScreen()
        {
            LoadingIndicator.Visibility = Visibility.Visible;

            if (LoadingText != null)
            {
                LoadingLabel.Text = LoadingText;
            }
        }

    }
}
