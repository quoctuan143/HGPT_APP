using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.FirebasePushNotification;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.XForms.iOS.TextInputLayout;
using Syncfusion.XForms.Pickers.iOS;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
namespace HGPT_APP.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        //Dictionary<string, string> dict = new Dictionary<string, string>();
        private bool IsNotification = false;
        private object NotificationData;
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags(new string[] { "CollectionView_Experimental", "Brush_Experimental", "SwipeView_Experimental", "CarouseView_Experimental", "IndicatorView_Experimental" });
            global::Xamarin.Forms.Forms.Init();
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            NativeMedia.Platform.Init(GetTopViewController);
            Syncfusion.SfDataGrid.XForms.iOS.SfDataGridRenderer.Init();
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfButtonRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();           
            Syncfusion.XForms.iOS.BadgeView.SfBadgeViewRenderer.Init();
            Syncfusion.XForms.iOS.RichTextEditor.SfRichTextEditorRenderer.Init();
            SfCalendarRenderer.Init();
            SfListViewRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            ImageCircleRenderer.Init();
            SfPickerRenderer.Init();
            SfDatePickerRenderer.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();
           
            //FirebasePushNotificationManager.Initialize(options, true);
            //FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
            //            {
            //               new NotificationUserCategory("message",new List<NotificationUserAction> {
            //                   new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
            //               }),
            //               new NotificationUserCategory("request",new List<NotificationUserAction> {
            //                   new NotificationUserAction("Accept","Accept"),
            //                   new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
            //               })  });
            //FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
            UIColor color = Color.FromHex("06264c").ToUIColor();
            UINavigationBar.Appearance.BackgroundColor = color;
            UINavigationBar.Appearance.BarTintColor = color;
            UITabBar.Appearance.BackgroundColor = color;
            UITabBar.Appearance.BarTintColor = color;
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
                NotificationData = p.Data;
                IsNotification = false;
            };
            LoadApplication(new App(IsNotification, NotificationData));

            return base.FinishedLaunching(app, options);
        }
        public UIViewController GetTopViewController()
        {
            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (vc is UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }
        //private void IOS_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        //{
        //   if (e.Data.Count > 0)
        //    {
        //        dict.Add("key", "nhanthongbao"); // cái này chỉ để kiểm tra có nhận thông báo ko.
        //    }    
        //}

        public override void WillEnterForeground(UIApplication uiApplication)
        {

        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}
