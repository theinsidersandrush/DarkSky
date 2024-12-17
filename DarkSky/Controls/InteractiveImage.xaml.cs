using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.Controls
{
	/*
	 * Control to display an image which when clicked will request to show an image overlay
	 * The control will contain a list of images to send to the ImageOverlay
	 * The control will also contain the singular image it is displaying
	 */
	public sealed partial class InteractiveImage : UserControl
	{
		public InteractiveImage()
		{
			this.InitializeComponent();
		}
	}
}
