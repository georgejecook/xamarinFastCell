using System;
using Xamarin.Forms;
using TwinTechs.Controls;
using TwinTechs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Java.Util;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;

[assembly: ExportRenderer (typeof(FastCell), typeof(FastCellRenderer))]
namespace TwinTechs.Controls
{
	internal class NativeCell : ViewGroup
	{
	
		public NativeCell (Android.Content.Context context, FastCell fastCell) : base (context)
		{
			FastCell = fastCell;
			fastCell.PrepareCell ();
			var renderer = RendererFactory.GetRenderer (fastCell.View);
			this.AddView (renderer.ViewGroup);
			//			_view = renderer.NativeView;
			//			ContentView.AddSubview (_view);
		}

		public FastCell FastCell {
			get;
			set;
		}

		Size _lastSize;

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			if (changed) {
				//TODO
			}
		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);
			//TODO update sizes of the xamarin view
			var newSize = new Size (w, h);
			if (_lastSize.Equals (Size.Zero) || !_lastSize.Equals (newSize)) {
	
				//				var layout = FastCell.Content;
				var layout = FastCell.View as Layout<Xamarin.Forms.View>;
				if (layout != null) {
					layout.Layout (new Rectangle (0, 0, w, h));
					layout.ForceLayout ();
					FixChildLayouts (layout);
				}
				_lastSize = newSize;
			}

			//TODO set the frame size
		}

		void FixChildLayouts (Layout<Xamarin.Forms.View> layout)
		{
			foreach (var child in layout.Children) {
				if (child is StackLayout) {
					((StackLayout)child).ForceLayout ();
					FixChildLayouts (child as Layout<Xamarin.Forms.View>);
				}
				if (child is Xamarin.Forms.AbsoluteLayout) {
					((Xamarin.Forms.AbsoluteLayout)child).ForceLayout ();
					FixChildLayouts (child as Layout<Xamarin.Forms.View>);
				}
			}
		}
	}





	public class FastCellRenderer : ViewCellRenderer
	{
		string cellId;

		static Dictionary<Android.Views.View,Dictionary<Android.Views.View,FastCell>> _reusableFastCellsByViewByParent;

		static Dictionary<Android.Views.View,FastCell> GetCellDictionary (ViewGroup parent)
		{
			if (_reusableFastCellsByViewByParent == null) {
				_reusableFastCellsByViewByParent = new Dictionary<Android.Views.View, Dictionary<Android.Views.View, FastCell>> ();
			}
			if (!_reusableFastCellsByViewByParent.ContainsKey (parent)) {
				_reusableFastCellsByViewByParent [parent] = new Dictionary<Android.Views.View, FastCell> ();
			}
			return _reusableFastCellsByViewByParent [parent];
		}

		int _cellId = 0;

		//TODO add a lookup for the cells we piggy back of.
		protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			var cellLookup = GetCellDictionary (parent);
			var fastCell = item as FastCell;
			Android.Views.View cellCore = convertView;
			if (cellCore != null && cellLookup.ContainsKey (cellCore)) {
				var reusedItem = cellLookup [cellCore];
				reusedItem.BindingContext = fastCell.BindingContext;
			} else {
				if (!fastCell.IsInitialized) {
					fastCell.PrepareCell ();
				}
				cellCore = base.GetCellCore (item, convertView, parent, context);
				cellCore.Id = _cellId;
				_cellId++;
				cellLookup [cellCore] = fastCell;
			}
			return cellCore;
		}
	}
}

