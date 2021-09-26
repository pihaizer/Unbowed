using System;
using UnityEngine;

#if UNITY_ANDROID
using TMPro;
using Unity.Notifications.Android;
#endif
#if UNITY_IOS
using System.Collections;
using Unity.Notifications.iOS;
#endif

namespace Utility {
    public static class Notifications {
        static INotificationManager _notificationManager;
        public static void Init() {
#if UNITY_ANDROID
        _notificationManager = new AndroidNotificationManager();
#endif
#if UNITY_IOS
        _notificationManager = new IOSNotificationManager();
#endif
        }
        public static void SendNotification(string title, string text, DateTime fireTime, int? id = null) {
            if (_notificationManager != null)
                _notificationManager.SendNotification(title, text, fireTime, id);
            else
                Debug.LogWarning("Tried to send notification while notification manager was not set.");
        }
        public static void CancelNotifications() {
            _notificationManager?.CancelNotifications();
        }

        interface INotificationManager {
            void SendNotification(string title, string text, DateTime fireTime, int? id = null);
            void CancelNotifications();
        }

#if UNITY_ANDROID
    private class AndroidNotificationManager : INotificationManager {
        private static AndroidNotificationChannel _channel;
        private const string _channelId = "crazypig_channel";
        public AndroidNotificationManager() {
            _channel = new AndroidNotificationChannel() {
                Id = _channelId,
                Name = "Main Sparkle Piglet channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(_channel);
        }
        public void SendNotification(string title, string text, DateTime fireTime, int? id = null) {
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = text;
            notification.FireTime = fireTime;
            if (id.HasValue) {
                AndroidNotificationCenter.SendNotificationWithExplicitID(notification, _channel.Id, id.Value);
            } else
                AndroidNotificationCenter.SendNotification(notification, _channel.Id);
            Debug.Log($"Scheduled notification: {title};{text};{fireTime}");
        }
        public void CancelNotifications() {
            AndroidNotificationCenter.CancelAllNotifications();
        }
    }
#endif

#if UNITY_IOS
    private class IOSNotificationManager : INotificationManager {
        public IOSNotificationManager() {
            RequestAuthorization();
        }

        public void SendNotification(string title, string text, DateTime fireTime, int? id = null) {
            var timeTrigger = new iOSNotificationTimeIntervalTrigger() {
                TimeInterval = fireTime - DateTime.Now,
                Repeats = false
            };

            var notification = new iOSNotification() {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = id.ToString(),
                Title = title,
                Body = text,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
        }
        public void CancelNotifications() {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }
        private IEnumerator RequestAuthorization() {
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using (var req = new AuthorizationRequest(authorizationOption, true)) {
                while (!req.IsFinished) {
                    yield return null;
                };

                string res = "\n RequestAuthorization:";
                res += "\n finished: " + req.IsFinished;
                res += "\n granted :  " + req.Granted;
                res += "\n error:  " + req.Error;
                res += "\n deviceToken:  " + req.DeviceToken;
                Debug.Log(res);
            }
        }
    }
#endif
    }
}