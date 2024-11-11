using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.HttpUtils
{
	public enum RequestTypes
	{
		/// <summary>
		/// GET Method 200 (404 (Not Found))
		/// </summary>
		GET,

		/// <summary>
		/// POST (201 (Created), 404 (Not Found), 409 (Conflict))
		/// </summary>
		POST,

		/// <summary>
		/// PUT (200 (OK) or 204 (No Content), 404 (Not Found), 405 (Method Not Allowed)),
		/// </summary>
		PUT,

		/// <summary>
		/// PATCH (405 (Method Not Allowed), 200 (OK) or 204 (No Content). 404 (Not Found))
		/// </summary>
		PATCH,

		/// <summary>
		/// DELETE (405 (Method Not Allowed), 200 (OK). 404 (Not Found))
		/// </summary>
		DELETE,
	}

	public class RequestParameter
	{
		public RequestParameter(string url) : 
			this(url, RequestTypes.GET, null, Encoding.UTF8)
		{

		}

		public RequestParameter(string url, RequestTypes requestType, string postMessage) : 
			this(url, requestType, postMessage, Encoding.UTF8)
		{

		}

		public RequestParameter(string url, RequestTypes requestType, string postMessage, Encoding encodingOption)
		{
			this.Url = url;
			this.PostMessage = postMessage;
			this.RequestType = requestType;
			this.EncodingOption = encodingOption;
		}

		/// <summary>
		/// Gets or sets Url.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets EncodingOption.
		/// </summary>
		public Encoding EncodingOption { get; set; } = Encoding.UTF8;

		/// <summary>
		/// Gets or sets PostMessage.
		/// </summary>
		public string PostMessage { get; set; }

		/// <summary>
		/// Gets or sets PostMessage.
		/// </summary>
		public RequestTypes RequestType { get; set; } = RequestTypes.GET;
	}
}
