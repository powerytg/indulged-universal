using Indulged.API.Utils;
using Indulged.UI.Common.Controls.Events;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Indulged.UI.Common.Controls
{
    public sealed class ModalPopup : Control
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ModalPopup()
        {
            this.DefaultStyleKey = typeof(ModalPopup);
        }

        // Button click event
        public event EventHandler<ModalPopupEventArgs> DismissWithButtonClick;
        public event EventHandler<ModalPopupEventArgs> ButtonClick;

        // Popup journal
        private static List<ModalPopup> popupHistory = new List<ModalPopup>();

        public static bool HasPopupHistory()
        {
            return (popupHistory.Count > 0);
        }

        public static void RemoveLastPopup()
        {
            if (popupHistory.Count == 0)
                return;

            var lastPopup = popupHistory[popupHistory.Count - 1];
            if (popupHistory.Contains(lastPopup))
                popupHistory.Remove(lastPopup);

            lastPopup.Dismiss();
        }

        private Page currentPage;
        private Page CurrentPage
        {
            get
            {
                if (currentPage == null)
                {
                    var frame = (Frame)Window.Current.Content;
                    currentPage = (Page)frame.Content;
                }

                return currentPage;
            }
        }

        // Curtain and borders
        private Rectangle topShadow;
        private Rectangle bottomShadow;
        private Rectangle curtain;
        private Grid contentView;
        private StackPanel buttonContainer;
        private TextBlock titleLabel;

        // Content elements
        private FrameworkElement contentElement;

        // Measured content element size
        private Size expectedContentSize;

        // Dialog title
        private string title = null;

        // Button titles
        private bool shouldAutoDismissWhenClickOnButtons;
        private List<String> buttonTitles = new List<string>();
        public List<Button> Buttons = new List<Button>();

        // Reference to the parent popup
        private Popup popupContainer;

        // Show the popup window with custom content
        public static ModalPopup ShowWithButtons(FrameworkElement content, string title = null, List<Button> _buttons = null, bool _shoulsAutoDismissWhenClickingOnButtons = true)
        {
            Popup popupContainer = new Popup();
            ModalPopup popup = new ModalPopup();
            popupContainer.Child = popup;
            popup.contentElement = content;
            popup.popupContainer = popupContainer;
            popup.shouldAutoDismissWhenClickOnButtons = _shoulsAutoDismissWhenClickingOnButtons;

            // Set title
            if (title != null)
                popup.title = title;

            // Create optional buttons
            if (_buttons != null)
            {
                foreach (var btn in _buttons)
                {
                    popup.Buttons.Add(btn);
                }
            }

            //popup.HostView.Opacity = 0.2;
            popup.CurrentPage.IsHitTestVisible = false;
            popupContainer.IsOpen = true;

            // Add to history
            popupHistory.Add(popup);

            return popup;

        }

        public static ModalPopup Show(FrameworkElement content, string title = null, List<string> buttonTitles = null, bool _shoulsAutoDismissWhenClickingOnButtons = true)
        {
            Popup popupContainer = new Popup();
            ModalPopup popup = new ModalPopup();
            popupContainer.Child = popup;
            popup.contentElement = content;
            popup.popupContainer = popupContainer;
            popup.shouldAutoDismissWhenClickOnButtons = _shoulsAutoDismissWhenClickingOnButtons;

            // Set title
            if (title != null)
                popup.title = title;

            // Create optional buttons
            if (buttonTitles != null)
            {
                foreach (string buttonTitle in buttonTitles)
                {
                    popup.buttonTitles.Add(buttonTitle);
                }
            }

            //popup.HostView.Opacity = 0.2;
            popup.CurrentPage.IsHitTestVisible = false;
            popupContainer.IsOpen = true;

            // Add to history
            popupHistory.Add(popup);


            return popup;
        }

        // Show the popup window with text
        public static ModalPopup Show(string text, string title = null, List<string> buttonTitles = null, bool _shoulsAutoDismissWhenClickingOnButtons = true)
        {
            // Create a text label
            TextBlock label = new TextBlock();
            label.Foreground = new SolidColorBrush(Colors.White);
            label.FontSize = 24;
            label.Margin = new Thickness(28);
            label.Text = text;
            label.TextAlignment = TextAlignment.Center;
            label.Width = Window.Current.Bounds.Width - label.Margin.Left - label.Margin.Right;
            label.TextWrapping = TextWrapping.Wrap;
            return Show(label, title, buttonTitles, _shoulsAutoDismissWhenClickingOnButtons);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            initialChildren();

            // Perform fade in animation
            this.PerformAppearanceAnimation();
        }

        // Calculated content height
        private void initialChildren()
        {
            double w = Window.Current.Bounds.Width;

            topShadow = GetTemplateChild("TopShadow") as Rectangle;
            bottomShadow = GetTemplateChild("BottomShadow") as Rectangle;
            contentView = GetTemplateChild("ContentView") as Grid;
            buttonContainer = GetTemplateChild("ButtonContainer") as StackPanel;
            curtain = GetTemplateChild("Curtain") as Rectangle;

            // Add an optional title label
            titleLabel = null;
            if (title != null)
            {
                titleLabel = new TextBlock();
                titleLabel.Text = title;
                titleLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xd9, 0xf3));
                titleLabel.FontSize = 30;
                titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
                titleLabel.TextWrapping = TextWrapping.Wrap;
                titleLabel.TextAlignment = TextAlignment.Center;
                titleLabel.Margin = new Thickness(0, 8, 0, 8);
                titleLabel.SetValue(Grid.RowProperty, 0);
                contentView.Children.Add(titleLabel);
            }

            // Add any custom content
            if (contentElement != null)
            {
                contentElement.SetValue(Grid.RowProperty, 1);
                contentView.Children.Add(contentElement);
                contentView.InvalidateArrange();
                contentView.UpdateLayout();

                double measuredWidth = contentElement.ActualWidth + contentElement.Margin.Left + contentElement.Margin.Right;
                double measuredHeight = 0;
                if (titleLabel != null)
                {
                    measuredHeight += titleLabel.ActualHeight;
                }

                double contentHeight = Math.Max(contentElement.ActualHeight, contentElement.Height);
                if (!double.IsNaN(contentHeight))
                    measuredHeight += contentHeight + contentElement.Margin.Top + contentElement.Margin.Bottom;
                else
                    measuredHeight += 240 + contentElement.Margin.Top + contentElement.Margin.Bottom;

                expectedContentSize = new Size(measuredWidth, measuredHeight);
            }
            else
            {
                expectedContentSize = new Size(w, 240);
            }

            // Add custom buttons
            if (buttonTitles.Count > 0)
            {
                foreach (string buttonTitle in buttonTitles)
                {
                    var button = new Button();
                    button.Content = buttonTitle;
                    button.Style = Application.Current.Resources["MainButtonStyle"] as Style;
                    button.Margin = new Thickness(10, 0, 10, 0);
                    button.HorizontalAlignment = HorizontalAlignment.Right;
                    buttonContainer.Children.Add(button);
                    button.Click += OnButtonClick;
                }
            }
            else if (Buttons.Count > 0)
            {
                foreach (var button in Buttons)
                {
                    button.Margin = new Thickness(10, 0, 10, 0);
                    button.HorizontalAlignment = HorizontalAlignment.Right;
                    buttonContainer.Children.Add(button);
                    button.Click += OnButtonClick;
                }

            }
        }

        private bool isApplicationBarVisibleBeforePopup;

        public void Dismiss()
        {
            DismissWithActionAsync(null);
        }

        public void DismissWithActionAsync(Action action)
        {
            // Remove from history
            if (popupHistory.Contains(this))
                popupHistory.Remove(this);

            CurrentPage.Frame.Navigated -= OnPageNavigated;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= OnBackKeyPress;

            this.Projection = new PlaneProjection { CenterOfRotationX = 0, RotationX = 0 };

            Storyboard animation = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            animation.Duration = duration;

            var alphaAnimation = new DoubleAnimation();
            alphaAnimation.Duration = duration;
            alphaAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            animation.Children.Add(alphaAnimation);
            alphaAnimation.To = 0;
            Storyboard.SetTarget(alphaAnimation, this);
            Storyboard.SetTargetProperty(alphaAnimation, "Opacity");


            var planeAnimation = new DoubleAnimation();
            planeAnimation.Duration = duration;
            planeAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            animation.Children.Add(planeAnimation);
            planeAnimation.To = -90;
            Storyboard.SetTarget(planeAnimation, this.Projection);
            Storyboard.SetTargetProperty(planeAnimation, "RotationX");

            animation.Begin();
            animation.Completed += async (sender, args) =>
            {
                if (popupContainer != null)
                {
                    popupContainer.IsOpen = false;
                    popupContainer = null;
                }

                await Dispatcher.RunAsync(new CoreDispatcherPriority(), async () => {
                    // Show application bar
                    if (isApplicationBarVisibleBeforePopup)
                    {
                        CurrentPage.BottomAppBar.IsEnabled = true;
                    }
                        
                    StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    await statusBar.ShowAsync();

                    CurrentPage.IsHitTestVisible = true;

                    // Perform cleanup
                    if (contentElement.GetType().HasImplementedInterface(typeof(IModalPopupContent)))
                    {
                        var modalContent = contentElement as IModalPopupContent;
                        modalContent.OnPopupRemoved();
                    }

                    // Perform afterburn action
                    if (action != null)
                        action();
                });

            };
        }

        public void ReplaceContentWith(string newTitle, FrameworkElement newElement, List<Button> newButtons, Action action = null)
        {
            double w = Window.Current.Bounds.Width;

            // Add new content element
            newElement.SetValue(Grid.RowProperty, 1);
            CompositeTransform ct = (CompositeTransform)newElement.RenderTransform;
            if (ct == null)
            {
                ct = new CompositeTransform();
                newElement.RenderTransform = ct;
            }

            ct.TranslateX = w;

            contentView.Children.Add(newElement);


            // Slide away the old contents
            Storyboard animation = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.5));

            DoubleAnimation oldContentXAnimation = new DoubleAnimation();
            animation.Children.Add(oldContentXAnimation);
            oldContentXAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            oldContentXAnimation.To = -w;
            oldContentXAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(oldContentXAnimation, contentElement);
            Storyboard.SetTargetProperty(oldContentXAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

            // Slide in the new content element
            DoubleAnimationUsingKeyFrames newContentXAnimation = new DoubleAnimationUsingKeyFrames();
            animation.Children.Add(newContentXAnimation);
            newContentXAnimation.Duration = animation.Duration;
            newContentXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0), Value = w });
            newContentXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.2), Value = w });
            newContentXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.5), Value = 0 });
            Storyboard.SetTarget(newContentXAnimation, newElement);
            Storyboard.SetTargetProperty(newContentXAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

            animation.Completed += (sender, e) =>
            {
                // Perform cleanup for old element
                if (contentElement.GetType().HasImplementedInterface(typeof(IModalPopupContent)))
                {
                    var modalContent = contentElement as IModalPopupContent;
                    modalContent.OnPopupRemoved();
                }

                contentView.Children.Remove(contentElement);
                contentElement = newElement;

                if (action != null)
                    action();
            };
            animation.Begin();

            PerformOldButtonSwapOutAnimation(newButtons);


            // Change title
            titleLabel.Text = newTitle;
        }

        private void PerformOldButtonSwapOutAnimation(List<Button> newButtons)
        {
            double w = Window.Current.Bounds.Width;

            // Slide away old buttons
            Storyboard animation = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation oldButtonDeckAnimation = new DoubleAnimation();
            animation.Children.Add(oldButtonDeckAnimation);
            oldButtonDeckAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            oldButtonDeckAnimation.To = -w;
            oldButtonDeckAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(oldButtonDeckAnimation, buttonContainer);
            Storyboard.SetTargetProperty(oldButtonDeckAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

            animation.Completed += (sender, e) =>
            {
                buttonContainer.Children.Clear();
                PerformNewButtonSwapInAnimation(newButtons);
            };

            animation.Begin();
        }

        private void PerformNewButtonSwapInAnimation(List<Button> newButtons)
        {
            double w = Window.Current.Bounds.Width;

            var ct = (CompositeTransform)buttonContainer.RenderTransform;
            ct.TranslateX = w;

            foreach (var button in newButtons)
            {
                button.Margin = new Thickness(20, 0, 20, 0);
                button.HorizontalAlignment = HorizontalAlignment.Right;
                buttonContainer.Children.Add(button);
                button.Click += OnButtonClick;
            }

            // Slide in new buttons
            Storyboard animation = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation oldButtonDeckAnimation = new DoubleAnimation();
            animation.Children.Add(oldButtonDeckAnimation);
            oldButtonDeckAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            oldButtonDeckAnimation.To = 0;
            oldButtonDeckAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(oldButtonDeckAnimation, buttonContainer);
            Storyboard.SetTargetProperty(oldButtonDeckAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

            animation.Completed += (sender, e) =>
            {
                Buttons = newButtons;
            };

            animation.Begin();
        }

        private void PerformAppearanceAnimation()
        {
            if (CurrentPage.BottomAppBar != null)
            {
                isApplicationBarVisibleBeforePopup = CurrentPage.BottomAppBar.IsEnabled;
                CurrentPage.BottomAppBar.IsEnabled = false;
            }

            // Override the current page's back button
            CurrentPage.Frame.Navigated += OnPageNavigated;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += OnBackKeyPress;

            double w = Window.Current.Bounds.Width;
            double h = Window.Current.Bounds.Height;

            // Initial settings
            Width = w;
            Height = h;

            CompositeTransform ct = (CompositeTransform)topShadow.RenderTransform;
            ct.TranslateY = -h;

            ct = (CompositeTransform)bottomShadow.RenderTransform;
            ct.TranslateY = h;

            ct = (CompositeTransform)curtain.RenderTransform;
            ct.ScaleY = h / expectedContentSize.Height;

            // Content view
            contentView.Opacity = 0;

            // Buttons
            buttonContainer.Opacity = 0;
            ct = (CompositeTransform)buttonContainer.RenderTransform;
            ct.TranslateY = -120;

            Storyboard animation = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.5));

            animation.Duration = duration;

            DoubleAnimation topShadowAnimation = new DoubleAnimation();
            animation.Children.Add(topShadowAnimation);
            topShadowAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            topShadowAnimation.To = 0;
            topShadowAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(topShadowAnimation, topShadow);
            Storyboard.SetTargetProperty(topShadowAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            DoubleAnimation bottomShadowAnimation = new DoubleAnimation();
            animation.Children.Add(bottomShadowAnimation);
            bottomShadowAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            bottomShadowAnimation.To = 0;
            bottomShadowAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(bottomShadowAnimation, bottomShadow);
            Storyboard.SetTargetProperty(bottomShadowAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            // Curtain animation
            DoubleAnimation curtainHeightAnimation = new DoubleAnimation();
            animation.Children.Add(curtainHeightAnimation);
            curtainHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            curtainHeightAnimation.To = 1.0;
            curtainHeightAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(curtainHeightAnimation, curtain);
            Storyboard.SetTargetProperty(curtainHeightAnimation, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");

            // Curtain alpha animation
            var curtainAlphaAnimation = new DoubleAnimationUsingKeyFrames();
            curtainAlphaAnimation.Duration = duration;
            animation.Children.Add(curtainAlphaAnimation);
            curtainAlphaAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0), Value = 0.3 });
            curtainAlphaAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.3), Value = 0, EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn } });
            Storyboard.SetTarget(curtainAlphaAnimation, curtain);
            Storyboard.SetTargetProperty(curtainAlphaAnimation, "Opacity");


            // Content view
            var contentAnimation = new DoubleAnimationUsingKeyFrames();
            contentAnimation.Duration = duration;
            animation.Children.Add(contentAnimation);
            contentAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.2), Value = 0.0, EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } });
            contentAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.4), Value = 1.0, EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } });
            Storyboard.SetTarget(contentAnimation, contentView);
            Storyboard.SetTargetProperty(contentAnimation, "Opacity");

            // Button container animation
            var buttonAnimation = new DoubleAnimationUsingKeyFrames();
            buttonAnimation.Duration = duration;
            animation.Children.Add(buttonAnimation);
            buttonAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0), Value = -120 });
            buttonAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.5), Value = 0, EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } });
            Storyboard.SetTarget(buttonAnimation, buttonContainer);
            Storyboard.SetTargetProperty(buttonAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            var buttonAlphaAnimation = new DoubleAnimationUsingKeyFrames();
            buttonAlphaAnimation.Duration = duration;
            animation.Children.Add(buttonAlphaAnimation);
            buttonAlphaAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.3), Value = 0 });
            buttonAlphaAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(0.5), Value = 1, EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn } });
            Storyboard.SetTarget(buttonAlphaAnimation, buttonContainer);
            Storyboard.SetTargetProperty(buttonAlphaAnimation, "Opacity");
            animation.Begin();
        }

        public void DismissWithButtonIndex(int buttonIndex)
        {
            // Remove from history
            if (popupHistory.Contains(this))
                popupHistory.Remove(this);

            CurrentPage.Frame.Navigated -= OnPageNavigated;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= OnBackKeyPress;

            this.Projection = new PlaneProjection { CenterOfRotationX = 0, RotationX = 0 };

            Storyboard animation = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            animation.Duration = duration;

            var alphaAnimation = new DoubleAnimation();
            alphaAnimation.Duration = duration;
            alphaAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            animation.Children.Add(alphaAnimation);
            alphaAnimation.To = 0;
            Storyboard.SetTarget(alphaAnimation, this);
            Storyboard.SetTargetProperty(alphaAnimation, "Opacity");


            var planeAnimation = new DoubleAnimation();
            planeAnimation.Duration = duration;
            planeAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            animation.Children.Add(planeAnimation);
            planeAnimation.To = -90;
            Storyboard.SetTarget(planeAnimation, this.Projection);
            Storyboard.SetTargetProperty(planeAnimation, "RotationX");

            animation.Begin();
            animation.Completed += async (sender, args) =>
            {
                // Perform cleanup
                if (contentElement.GetType().HasImplementedInterface(typeof(IModalPopupContent)))
                {
                    var modalContent = contentElement as IModalPopupContent;
                    modalContent.OnPopupRemoved();
                }

                if (popupContainer != null)
                {
                    popupContainer.IsOpen = false;
                    popupContainer = null;
                }

                var e = new ModalPopupEventArgs();
                e.ButtonIndex = buttonIndex;
                DismissWithButtonClick.DispatchEventOnMainThread(this, e);

                await Dispatcher.RunAsync(new CoreDispatcherPriority(), async () =>
                {
                    // Show application bar
                    StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                    await statusBar.ShowAsync();

                    if (isApplicationBarVisibleBeforePopup)
                        CurrentPage.BottomAppBar.IsEnabled = true;

                    CurrentPage.IsHitTestVisible = true;
                });

            };
        }

        // Button click event
        private void OnButtonClick(object sender, RoutedEventArgs args)
        {
            Button targetButton = (Button)sender;
            int buttonIndex = buttonContainer.Children.IndexOf(targetButton);

            // Dismiss self
            if (shouldAutoDismissWhenClickOnButtons)
            {
                DismissWithButtonIndex(buttonIndex);
            }
            else
            {
                var evt = new ModalPopupEventArgs();
                evt.ButtonIndex = buttonIndex;
                ButtonClick.DispatchEvent(this, evt);
            }
        }

        private void OnBackKeyPress(Object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            Dismiss();
        }

        private void OnPageNavigated(Object sender, NavigationEventArgs e)
        {
            Dismiss();
        }
    }
}
