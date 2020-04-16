using Android;
using Android.AccessibilityServices;
using Android.App;
using Android.Content;
using Android.Views.Accessibility;

namespace ButtonWidget
{
    [Service(Label = "PowerMenuService", Enabled = true, Exported = true, Name = "com.rizlas.buttonwidget.PowerMenuService", Permission = Manifest.Permission.BindAccessibilityService)]
    [IntentFilter(new[] { AccessibilityService })]
    [MetaData("android.accessibilityservice", Resource = "@xml/accessibility_service")]
    class PowerMenuService : AccessibilityService
    {
        // Not a good way but it works, so far so good, changed when the power button is clicked
        public static bool PowerClicked { get; set; } = false;

        public override void OnAccessibilityEvent(AccessibilityEvent e)
        {
            if (PowerClicked)
            {
                PerformGlobalAction(GlobalAction.PowerDialog);
                PowerClicked = false;
            }
        }

        public override void OnInterrupt()
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}