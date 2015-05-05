using System;
using TwinTechs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using CoreGraphics;

[assembly: ExportRenderer (typeof(FastCell), typeof(FastCellRenderer))]
namespace TwinTechs.Controls
{
	internal class NativeCell : UITableViewCell
	{

		UIView _view;

		public FastCell FastCell {
			get;
			set;
		}

		public NativeCell (NSString cellId, FastCell fastCell) : base (UITableViewCellStyle.Default, cellId)
		{
			FastCell = fastCell;
			fastCell.PrepareCell ();
			var renderer = RendererFactory.GetRenderer (fastCell.View);
			_view = renderer.NativeView;
			ContentView.AddSubview (_view);
		}

		CGSize _lastSize;

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			//TODO update sizes of the xamarin view
			if (_lastSize.Equals (CGSize.Empty) || !_lastSize.Equals (Frame.Size)) {

//				var layout = FastCell.Content;
				var layout = FastCell.View as Layout<View>;
				if (layout != null) {
					layout.Layout (Frame.ToRectangle ());
					layout.ForceLayout ();
					FixChildLayouts (layout);
				}
				_lastSize = Frame.Size;
			}

			_view.Frame = ContentView.Bounds;
		}

		void FixChildLayouts (Layout<View> layout)
		{
			foreach (var child in layout.Children) {
				if (child is StackLayout) {
					((StackLayout)child).ForceLayout ();
					FixChildLayouts (child as Layout<View>);
				}
				if (child is AbsoluteLayout) {
					((AbsoluteLayout)child).ForceLayout ();
					FixChildLayouts (child as Layout<View>);
				}
			}
		}
	}





	public class FastCellRenderer : ViewCellRenderer
	{
		public FastCellRenderer ()
		{
		}

		NSString cellId;

		public override UITableViewCell GetCell (Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			cellId = cellId ?? new NSString (item.GetType ().FullName);
			var nativeCell = reusableCell as NativeCell;
			var viewCell = item as FastCell;

			if (reusableCell == null) {
				nativeCell = new NativeCell (cellId, viewCell);
			} else {
				nativeCell.FastCell.BindingContext = item.BindingContext;
			}

			return nativeCell;
		}
	}
}

