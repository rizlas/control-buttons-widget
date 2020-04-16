using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.App.Admin;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace ButtonWidget
{
	[Activity(Label = "Control buttons Widget", MainLauncher = true, Icon = "@drawable/ic_launcher")]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar(toolbar);

			SupportActionBar.Title = "Control buttons Widget";

			Button btnAccessibility = this.FindViewById<Button>(Resource.Id.btnAccessibility);
			btnAccessibility.Click += BtnAccessibility_Click;

			Button btnAdmin = this.FindViewById<Button>(Resource.Id.btnAdmin);
			btnAdmin.Click += BtnAdmin_Click; ;

			NotificationManager notificationManager = (NotificationManager)this.GetSystemService(Context.NotificationService);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.M && !notificationManager.IsNotificationPolicyAccessGranted)
			{
				var intent = new Intent(Android.Provider.Settings.ActionNotificationPolicyAccessSettings);
				StartActivity(intent);				
			}

			Toast.MakeText(this, "Long-press the homescreen to add the widget", ToastLength.Long).Show();
		}

		private void BtnAdmin_Click(object sender, System.EventArgs e)
		{
			Lock(this);
		}

		private void BtnAccessibility_Click(object sender, System.EventArgs e)
		{
			Intent intent = new Intent(Android.Provider.Settings.ActionAccessibilitySettings);
			StartActivityForResult(intent, 0);
		}

		private void Lock(Context context)
		{
			DevicePolicyManager Policy = (DevicePolicyManager)context.GetSystemService(Context.DevicePolicyService);
			ComponentName admin = new ComponentName(context, "com.rizlas.buttonwidget.AdminReceiver");

			bool Active = Policy.IsAdminActive(admin);

			if (!Active)
			{
				Intent intent = new Intent(DevicePolicyManager.ActionAddDeviceAdmin);
				intent.PutExtra(DevicePolicyManager.ExtraDeviceAdmin, admin);
				intent.PutExtra(DevicePolicyManager.ExtraAddExplanation, "Permissions to lock the phone screen");
				context.StartActivity(intent);
			}
			else
			{
				Toast.MakeText(this, "Admin access granted", ToastLength.Long).Show();
			}
		}
	}
}

