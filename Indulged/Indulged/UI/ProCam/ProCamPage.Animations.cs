using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private Storyboard focusBinkAnimation = null;

        private void PerformFocusAnimation(Windows.Foundation.Point? pt)
        {
            CompositeTransform tf = (CompositeTransform)AutoFocusBrackets.RenderTransform;
            tf.CenterX = AutoFocusBrackets.Width / 2;
            tf.CenterY = AutoFocusBrackets.Height / 2;
            tf.TranslateX = pt.Value.X - LayoutRoot.ActualWidth / 2;
            tf.TranslateY = pt.Value.Y - LayoutRoot.ActualHeight / 2;
            tf.ScaleX = 1;
            tf.ScaleY = 1;
            AutoFocusBrackets.Visibility = Visibility.Visible;

            // Zoom animation
            Storyboard sb = new Storyboard();
            sb.Duration = new Duration(TimeSpan.FromSeconds(0.2));

            DoubleAnimation xAnimation = new DoubleAnimation();
            xAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            xAnimation.To = 0.2;
            xAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(xAnimation, AutoFocusBrackets);
            Storyboard.SetTargetProperty(xAnimation, "(UIElement.RenderTransform).(CompositeTransform.ScaleX)");
            sb.Children.Add(xAnimation);

            DoubleAnimation yAnimation = new DoubleAnimation();
            yAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            yAnimation.To = 0.2;
            yAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(yAnimation, AutoFocusBrackets);
            Storyboard.SetTargetProperty(yAnimation, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
            sb.Children.Add(yAnimation);

            sb.Begin();
            sb.Completed += (sender, e) =>
            {
                if (focusBinkAnimation != null)
                {
                    focusBinkAnimation.Stop();
                    focusBinkAnimation = null;
                }

                focusBinkAnimation = new Storyboard();
                focusBinkAnimation.RepeatBehavior = RepeatBehavior.Forever;

                DoubleAnimationUsingKeyFrames blinkAnimation = new DoubleAnimationUsingKeyFrames();
                blinkAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0)), Value = 1 });
                blinkAnimation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 1)), Value = 0 });
                Storyboard.SetTarget(blinkAnimation, AutoFocusBrackets);
                Storyboard.SetTargetProperty(blinkAnimation, "(UIElement.Opacity)");
                focusBinkAnimation.Children.Add(blinkAnimation);
                focusBinkAnimation.Begin();
            };
        }

        private void PerformFocusLockedAnimation()
        {
            Storyboard sb = new Storyboard();
            sb.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation opactiyAnimation = new DoubleAnimation();
            opactiyAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            opactiyAnimation.To = 0;
            Storyboard.SetTarget(opactiyAnimation, AutoFocusBrackets);
            Storyboard.SetTargetProperty(opactiyAnimation, "(UIElement.Opacity)");
            sb.Children.Add(opactiyAnimation);
            sb.Begin();
            sb.Completed += (sender, e) =>
            {
                AutoFocusBrackets.Visibility = Visibility.Collapsed;
                CompositeTransform tf = (CompositeTransform)AutoFocusBrackets.RenderTransform;
                tf.ScaleX = 1;
                tf.ScaleY = 1;
            };
        }

        private Rectangle curtain;
        private void PerformCaptureAnimation()
        {
            LoadingView.Text = "Processing ...";
            ShowLoadingView();

            curtain = new Rectangle();
            curtain.Fill = new SolidColorBrush(Colors.White);
            LayoutRoot.Children.Add(curtain);

            Storyboard animation = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            animation.Duration = duration;

            DoubleAnimation curtainAnimation = new DoubleAnimation();
            animation.Children.Add(curtainAnimation);
            curtainAnimation.Duration = duration;
            curtainAnimation.To = 0;
            curtainAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn };
            Storyboard.SetTarget(curtainAnimation, curtain);
            Storyboard.SetTargetProperty(curtainAnimation, "Opacity");

            animation.Completed += (sender, e) =>
            {
                LayoutRoot.Children.Remove(curtain);
            };

            animation.Begin();
        }
    }
}
