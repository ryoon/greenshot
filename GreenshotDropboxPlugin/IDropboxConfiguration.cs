﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2015 Thomas Braun, Jens Klingen, Robin Krom, Francis Noel
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

using Dapplo.Config.Converters;
using Dapplo.Config.Ini;
using GreenshotPlugin.Configuration;
using System.ComponentModel;

namespace GreenshotDropboxPlugin
{
	/// <summary>
	/// Description of ImgurConfiguration.
	/// </summary>
	[IniSection("Dropbox"), Description("Greenshot Dropbox Plugin configuration")]
	public interface IDropboxConfiguration : IIniSection<IDropboxConfiguration>
	{
		[Description("What file type to use for uploading"), DefaultValue(OutputFormat.png)]
		OutputFormat UploadFormat
		{
			get;
			set;
		}

		[Description("JPEG file save quality in %."), DefaultValue(80)]
		int UploadJpegQuality
		{
			get;
			set;
		}

		[Description("After upload send Dropbox link to clipboard."), DefaultValue(true)]
		bool AfterUploadLinkToClipBoard
		{
			get;
			set;
		}

		[Description("The Dropbox token"), TypeConverter(typeof (StringEncryptionTypeConverter))]
		string DropboxToken
		{
			get;
			set;
		}

		[Description("The Dropbox token secret"), TypeConverter(typeof (StringEncryptionTypeConverter))]
		string DropboxTokenSecret
		{
			get;
			set;
		}

		/// <summary>
		/// Not stored, but read so people could theoretically specify their own Client ID.
		/// </summary>
		[IniPropertyBehavior(Write = false), DefaultValue("@credentials_dropbox_consumer_key@")]
		string ClientId
		{
			get;
			set;
		}

		/// <summary>
		/// Not stored, but read so people could theoretically specify their own client secret.
		/// </summary>
		[IniPropertyBehavior(Write = false), DefaultValue("@credentials_dropbox_consumer_secret@")]
		string ClientSecret
		{
			get;
			set;
		}
	}
}