using DarkSky.Core.ViewModels.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace DarkSky.Templates
{
	public class CursorItemSelector : DataTemplateSelector
	{
		public DataTemplate PostItemTemplate { get; set; }
		public DataTemplate ListItemTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			if (item is PostViewModel)
			{
				return PostItemTemplate;
			}
			else if (item is ListViewModel)
			{
				return ListItemTemplate;
			}
			return base.SelectTemplateCore(item, container);
		}
	}
}
