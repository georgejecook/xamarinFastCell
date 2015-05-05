# FastCell for xamarin forms

## Overview
This project contains a fast cell implementation which has some optimizaitons which drastically improve performance of listview cells, when using xaml. 

## The problem
Currently xamarin forms creates a view cell for each item in a list, even though the underlying template will only use as many cells as are visible on the screen). If your view cell is based on xaml, this means your constructor will call InitializeComponent as many times as you have items. This is what causes the jumping and stuttering when you use Xaml.



## The solution

The optimizaitons work by only initilaizing the xamarin cells for the actual cells which are used. The code changes required to achieve this are minimial.

### Extend FastCell, and initialize your cellat the appropriate place
Remove your expensive contstructor from your xaml page and replace with an implementation of the abstract method InitializeCell:

		protected override void InitializeCell ()
		{
			InitializeComponent ();
		}
		
Note, you can also do other initialization here if you need.

### Set data your cell up with the data in the BindingContext
We don't use bindings in our cells, we instead opt to set the data values by hand, when the binding context changes.

Just override the abstract SetupCell method, and you're good to go:

	protected override void SetupCell (bool isRecycled)
		{
			var mediaItem = BindingContext as MediaItem;
			if (mediaItem != null) {
				UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
				ImageView.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
				NameLabel.Text = mediaItem.Name;
				DescriptionLabel.Text = mediaItem.Description;
			}
		}
		
		
##Implementation notes
I've not tried this with non-xaml cells, nor with bindings. I suspect both will work.

Android is not implemented at all. I expect that the implementation would be trivial, and should get the same performance benefits as iOS (windows too, I'm guessing).



## Bonus - fastImage!
Another major issue of slowness on xamarin is the downloading of images into cells. On iOS we use SDWebImage and other cool libraries to help with that. 

This project also contains our FastImage wrapper, which will give you all the benefits of SDWebImage.

	UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
