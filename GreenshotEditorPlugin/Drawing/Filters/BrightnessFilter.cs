﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2015 Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using GreenshotPlugin.Core;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using GreenshotPlugin.Interfaces.Drawing;

namespace GreenshotEditorPlugin.Drawing.Filters
{
	[Serializable]
	public class BrightnessFilter : AbstractFilter
	{
		protected double _brightness = 0.9f;

		[Field(FieldTypes.BRIGHTNESS)]
		public double Brightness
		{
			get
			{
				return _brightness;
			}
			set
			{
				_brightness = value;
				OnFieldPropertyChanged(FieldTypes.BRIGHTNESS);
			}
		}

		public BrightnessFilter(DrawableContainer parent) : base(parent)
		{
		}

		/// <summary>
		/// Implements the Apply code for the Brightness Filet
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="applyBitmap"></param>
		/// <param name="rect"></param>
		/// <param name="renderMode"></param>
		public override void Apply(Graphics graphics, Bitmap applyBitmap, Rectangle rect, RenderMode renderMode)
		{
			Rectangle applyRect = ImageHelper.CreateIntersectRectangle(applyBitmap.Size, rect, Invert);

			if (applyRect.Width == 0 || applyRect.Height == 0)
			{
				// nothing to do
				return;
			}

			GraphicsState state = graphics.Save();
			if (Invert)
			{
				graphics.SetClip(applyRect);
				graphics.ExcludeClip(rect);
			}
			using (ImageAttributes ia = ImageHelper.CreateAdjustAttributes((float) _brightness, 1f, 1f))
			{
				graphics.DrawImage(applyBitmap, applyRect, applyRect.X, applyRect.Y, applyRect.Width, applyRect.Height, GraphicsUnit.Pixel, ia);
			}
			graphics.Restore(state);
		}
	}
}