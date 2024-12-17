using DarkSky.Core.Cursors;
using DarkSky.Core.Cursors.Feeds;
using DarkSky.Core.ViewModels.Temporary;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSky.Core.Classes
{
	public record CursorNavigationItem(string Name, ICursorSource CursorSource);
}
