﻿/*
TouchFramework connects touch tracking from a tracking engine to WPF controls 
allow scaling, rotation, movement and other multi-touch behaviours.

Copyright 2009 - Mindstorm Limited (reg. 05071596)

Author - Simon Lerpiniere

This file is part of TouchFramework.

TouchFramework is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TouchFramework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser Public License for more details.

You should have received a copy of the GNU Lesser Public License
along with TouchFramework.  If not, see <http://www.gnu.org/licenses/>.

If you have any questions regarding this library, or would like to purchase 
a commercial licence, please contact Mindstorm via www.mindstorm.com.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TouchFramework.Helpers;

using WPFMitsuControls;

namespace TouchFramework.ControlHandlers
{
    /// <summary>
    /// Custom logic for handling multi-touch button interation
    /// </summary>
    public class BookHandler : ElementHandler
    {
        BookPage selectedPage = null;

        public override void TouchDown(PointF p)
        {
            Book b = Source as Book;
            if (b == null) return;
            
            b.SetSelectedPages();
            selectedPage = findSelectedPage(b, new System.Windows.Point(p.X, p.Y));

            b.CheckSheets(selectedPage);

            if (selectedPage != null)
            {
                selectedPage.GrabPage(b, new System.Windows.Point(p.X, p.Y));
            }

            base.TouchDown(p);
        }

        public override void Drag(PointF global, PointF relative)
        {
            Book b = Source as Book;
            if (b == null) return;

            if (selectedPage != null)
            {
                System.Windows.Point relPoint = b.TransformToVisual(selectedPage).Transform(new System.Windows.Point(relative.X, relative.Y));
                selectedPage.MoveGrabPoint(selectedPage, new System.Windows.Point(relPoint.X, relPoint.Y));
            }

            base.Drag(global, relative);
        }

        public override void TouchUp(PointF p)
        {
            Book b = Source as Book;
            if (b == null) return;

            if (selectedPage != null) selectedPage.LetGoPage(b, new System.Windows.Point(p.X, p.Y));
            
            base.TouchUp(p);
        }
        
        BookPage findSelectedPage(Book c, System.Windows.Point p)
        {
            Visual findFrom = c.InputHitTest(p) as Visual;
            return findFrom.FindParent<BookPage>() as BookPage;
        }
    }
}
