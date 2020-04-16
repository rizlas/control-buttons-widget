using Android.App;
using Android.App.Admin;
using Android.Appwidget;
using Android.Content;
using Android.Media;
using Android.Widget;

namespace ButtonWidget
{
	[BroadcastReceiver(Label = "Control buttons Widget")]
	[IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
	// The "Resource" file has to be all in lower caps
	[MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
	public class AppWidget : AppWidgetProvider
	{
		private static string MuteTag = "Mute";
		private static string VolDownTag = "Down";
		private static string VolUpTag = "Up";
		private static string LockTag = "Lock";
		private static string PowerTag = "Power";

		/// <summary>
		/// This method is called when the 'updatePeriodMillis' from the AppwidgetProvider passes,
		/// or the user manually refreshes/resizes.
		/// </summary>
		public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
		{
			var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
			appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
		}

		private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
		{
			// Retrieve the widget layout. This is a RemoteViews, so we can't use 'FindViewById'
			var widgetView = new RemoteViews(context.PackageName, Resource.Layout.Widget);

			RegisterClicks(context, appWidgetIds, widgetView);

			return widgetView;
		}

		private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
		{
			var intent = new Intent(context, typeof(AppWidget));
			intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
			intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

			// Register click event for the Announcement-icon
			widgetView.SetOnClickPendingIntent(Resource.Id.btnVolMute, GetPendingSelfIntent(context, MuteTag));
			widgetView.SetOnClickPendingIntent(Resource.Id.btnVolDown, GetPendingSelfIntent(context, VolDownTag));
			widgetView.SetOnClickPendingIntent(Resource.Id.btnVolUp, GetPendingSelfIntent(context, VolUpTag));
			widgetView.SetOnClickPendingIntent(Resource.Id.btnLock, GetPendingSelfIntent(context, LockTag));
			widgetView.SetOnClickPendingIntent(Resource.Id.btnPower, GetPendingSelfIntent(context, PowerTag));
		}

		private PendingIntent GetPendingSelfIntent(Context context, string action)
		{
			var intent = new Intent(context, typeof(AppWidget));
			intent.SetAction(action);
			return PendingIntent.GetBroadcast(context, 0, intent, 0);
		}

		/// <summary>
		/// This method is called when clicks are registered.
		/// </summary>
		public override void OnReceive(Context context, Intent intent)
		{
			base.OnReceive(context, intent);

			if (MuteTag.Equals(intent.Action))
			{
				AudioManager am = (AudioManager)context.GetSystemService(Context.AudioService);
				am.RingerMode = RingerMode.Silent;
				Toast.MakeText(context, "Muted", ToastLength.Long).Show();
			}
			else if(VolDownTag.Equals(intent.Action))
			{
				VolumeAdjustment(context, Adjust.Lower);
			}
			else if (VolUpTag.Equals(intent.Action))
			{
				VolumeAdjustment(context, Adjust.Raise);
			}
			else if (LockTag.Equals(intent.Action))
			{
				Toast.MakeText(context, "Lock", ToastLength.Long).Show();
				Lock(context);
			}
			else if (PowerTag.Equals(intent.Action))
			{
				Toast.MakeText(context, "Power", ToastLength.Long).Show();
				PowerMenuService.PowerClicked = true;
			}
		}

		private static void VolumeAdjustment(Context context, Adjust adjust, VolumeNotificationFlags flag = VolumeNotificationFlags.ShowUi, Stream stream = Stream.Ring)
		{
			AudioManager am = (AudioManager)context.GetSystemService(Context.AudioService);
			am.AdjustStreamVolume(stream, adjust, flag);
			int Volume = am.GetStreamVolume(stream);
		}

		private void Lock(Context context)
		{
			DevicePolicyManager Policy = (DevicePolicyManager)context.GetSystemService(Context.DevicePolicyService);
			ComponentName admin = new ComponentName(context, "com.rizlas.buttonwidget.AdminReceiver");

			bool Active = Policy.IsAdminActive(admin);

			if(Active)
			{
				Policy.LockNow();
			}
			else
			{
				Toast.MakeText(context, "Please grant admin permissions", ToastLength.Long).Show();
			}
		}
	}
}
