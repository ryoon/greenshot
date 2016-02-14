﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2016 Thomas Braun, Jens Klingen, Robin Krom,
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

using Dapplo.Config.Ini;
using GreenshotPlugin.OAuth;
using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Dapplo.HttpExtensions;

namespace GreenshotDropboxPlugin
{
	/// <summary>
	/// Description of DropboxUtils.
	/// </summary>
	public static class DropboxUtils
	{
		private static readonly Serilog.ILogger LOG = Serilog.Log.Logger.ForContext(typeof(DropboxUtils));
		private static readonly IDropboxConfiguration config = IniConfig.Current.Get<IDropboxConfiguration>();
		private static readonly Uri DROPBOX_API_URI = new Uri("https://api.dropbox.com/1");
		private static readonly Uri DROPBOX_OAUTH_URI = DROPBOX_API_URI.AppendSegments("oauth");
		private static readonly Uri DROPBOX_API_CONTENT_URI = new Uri("https://api-content.dropbox.com/1/files_put/sandbox/");
		private static readonly Uri DROPBOX_SHARES_URI = DROPBOX_API_URI.AppendSegments("shares", "sandbox");

		public static async Task<string> UploadToDropbox(HttpContent content, string filename)
		{
			var oAuth = new OAuthSession(config.ClientId, config.ClientSecret);
			oAuth.BrowserSize = new Size(1080, 650);
			oAuth.CheckVerifier = false;

			oAuth.AccessTokenUrl = DROPBOX_OAUTH_URI.AppendSegments("access_token");
			oAuth.AuthorizeUrl = DROPBOX_OAUTH_URI.AppendSegments("authorize");
			oAuth.RequestTokenUrl = DROPBOX_OAUTH_URI.AppendSegments("request_token");
			oAuth.LoginTitle = "Dropbox authorization";
			oAuth.Token = config.DropboxToken;
			oAuth.TokenSecret = config.DropboxTokenSecret;

			try
			{
				string uploadResponse = await oAuth.MakeOAuthRequest(HttpMethod.Post, DROPBOX_API_CONTENT_URI.AppendSegments(Uri.EscapeDataString(filename)), null, null, null, null, content);
				LOG.Debug("Upload response: {0}", uploadResponse);
			}
			catch (Exception ex)
			{
				LOG.Error("Upload error: ", ex);
				throw;
			}
			finally
			{
				if (!string.IsNullOrEmpty(oAuth.Token))
				{
					config.DropboxToken = oAuth.Token;
				}
				if (!string.IsNullOrEmpty(oAuth.TokenSecret))
				{
					config.DropboxTokenSecret = oAuth.TokenSecret;
				}
			}

			// Try to get a URL to the uploaded image
			try
			{
				string responseString = await oAuth.MakeOAuthRequest(HttpMethod.Get, DROPBOX_SHARES_URI.AppendSegments(Uri.EscapeDataString(filename)), null);
				if (responseString != null)
				{
					LOG.Debug("Parsing output: {0}", responseString);
					dynamic result = SimpleJson.DeserializeObject(responseString);
					return result.url;
				}
			}
			catch (Exception ex)
			{
				LOG.Error("Can't parse response.", ex);
			}
			return null;
		}
	}
}