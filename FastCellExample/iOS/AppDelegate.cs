using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using TwinTechs.Ios.Controls;
using TwinTechs.Example;

namespace FastCellExample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		FastCellRenderer dummyFastCellRenderer;
		FastImageRenderer dummyFastImageRenderer;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			//not need on iOS; but simplifies the pages code between android and iOS
			AppHelper.FastCellCache = FastCellCache.Instance;

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
			#endif

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

