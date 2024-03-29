﻿using System;
using Firebase.Messaging;
using Firebase.Iid;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Media;
using System.Collections.Generic;
using HGPT_APP.Views;
using Android.Content;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;

namespace HGPT_APP.Droid
{
    [Activity(Label = "HGPT Pro", Icon = "@mipmap/logohgpt", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.FullSensor)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool IsNotification = false;
        private object NotificationData;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);           
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);        
           
          
            await CrossMedia.Current.Initialize();
            NativeMedia.Platform.Init(this, savedInstanceState);
            NotificationCenter.NotifyNotificationTapped(Intent);
            FirebasePushNotificationManager.ProcessIntent(this, Intent);

#if DEBUG
            FirebasePushNotificationManager.Initialize(this,
                          new NotificationUserCategory[]
                          {
                    new NotificationUserCategory("message",new List<NotificationUserAction> {
                        new NotificationUserAction("Reply","Reply", NotificationActionType.Foreground),
                        new NotificationUserAction("Forward","Forward", NotificationActionType.Foreground)

                    }),
                    new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept", NotificationActionType.Default, "check"),
                    new NotificationUserAction("Reject","Reject", NotificationActionType.Default, "cancel")
                    })
                          }, true);
#else
  FirebasePushNotificationManager.Initialize(this,
                new NotificationUserCategory[]
                {
                    new NotificationUserCategory("message",new List<NotificationUserAction> {
                        new NotificationUserAction("Reply","Reply", NotificationActionType.Foreground),
                        new NotificationUserAction("Forward","Forward", NotificationActionType.Foreground)

                    }),
                    new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept", NotificationActionType.Default, "check"),
                    new NotificationUserAction("Reject","Reject", NotificationActionType.Default, "cancel")
                    })
                }, false);
#endif            
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
                NotificationData = p.Data;
                IsNotification = false;
            };
            LoadApplication(new App(IsNotification, NotificationData));

            // IsPlayServicesAvailable();
            // CreateNotificationChannel();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            if (NativeMedia.Platform.CheckCanProcessResult(requestCode, resultCode, intent))
                NativeMedia.Platform.OnActivityResult(requestCode, resultCode, intent);

            base.OnActivityResult(requestCode, resultCode, intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
    }
}