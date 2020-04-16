using Android;
using Android.App;
using Android.App.Admin;
using Android.Content;
using Android.Widget;

namespace ButtonWidget
{
    [BroadcastReceiver(Permission = Manifest.Permission.BindDeviceAdmin, Name = "com.rizlas.buttonwidget.AdminReceiver")]
    [MetaData("android.app.device_admin", Resource = "@xml/device_admin_sample")]
    [IntentFilter(new[] { "android.app.action.DEVICE_ADMIN_ENABLED", Intent.ActionMain })]
    class AdminReceiver : DeviceAdminReceiver
    {
        public override void OnEnabled(Context context, Intent intent)
        {
            base.OnEnabled(context, intent);
            Toast.MakeText(context, "Admin granted", ToastLength.Short).Show();
        }
        public override void OnDisabled(Context context, Intent intent)
        {
            base.OnDisabled(context, intent);
        }
    }
}
