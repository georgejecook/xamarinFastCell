﻿using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TwinTechs.Example;
using TwinTechs.Droid.Controls;

namespace FastCellExample.Droid
{
	[Activity (Label = "FastCellExample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		//ensure classes are compiled
		FastCellRenderer dummyFastCellRenderer;
		FastImageRenderer dummyFastImageRenderer;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			AppHelper.FastCellCache = FastCellCache.Instance;

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}
	}
}

