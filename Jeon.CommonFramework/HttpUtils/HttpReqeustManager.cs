using Jeon.CommonFramework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace Jeon.CommonFramework.HttpUtils
{
	public enum ResponseTypes
	{
		/// <summary>
		/// 성공
		/// </summary>
		Success,
		/// <summary>
		/// 에러
		/// </summary>
		Error,
	}

	/// <summary>
	/// Http 통신 Manager
	/// </summary>
	public class HttpReqeustManager
	{
		private static Lazy<HttpReqeustManager> _instance = new Lazy<HttpReqeustManager>(() => new HttpReqeustManager());

		public static HttpReqeustManager Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		/// <summary>
		/// 기본 요청 매서드 작성 (문자열 반환)
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public string CallService(RequestParameter model)
		{
			if (model == null)
			{
				return null;
			}

			try
			{
				return this.ServiceRequest(model);
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		/// <summary>
		/// 비동기 Service Response String 반환 (awaitable Async) 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<string> CallServiceAsync(RequestParameter model)
		{
			if (model == null)
			{
				return null;
			}

			try
			{
				var result = await ServiceRequestAsync(model);

				return result;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		private Task<string> ServiceRequestAsync(RequestParameter model)
		{
			return Task.Run(() => this.ServiceRequest(model));
		}

		private string ServiceRequest(RequestParameter parameter)
		{
			var message = string.Empty;

			Logger.WriteInfo(string.Format("Request Info] Url : {0}, RequestType : {1}", parameter.Url, parameter.RequestType));

			using (var webResponse = this.GetResponse(parameter))
			{

				if (webResponse == null)
				{
					return ResponseTypes.Error.ToString();
				}

				using (var response = webResponse.GetResponseStream())
				{
					if (response == null)
					{
						Logger.WriteError("Response is NULL!!");

						return ResponseTypes.Error.ToString();
					}

					using (var reader = new StreamReader(response, parameter.EncodingOption))
					{
						message = reader.ReadToEnd();
					}

					return message;
				}
			}
		}

		private HttpWebRequest Request(string url, ICredentials credentials = null)
		{
			try
			{
				var req = WebRequest.Create(url) as HttpWebRequest;

				if (req == null)
				{
					return null;
				}

				req.Timeout = 5000;
				req.ContentType = "multipart/form-data";
				req.Method = RequestTypes.GET.ToString();
				req.SendChunked = false;
				req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
				req.Credentials = credentials;

				return req;
			}
			catch (Exception ex)
			{
				Logger.WriteError("Request Error ", ex);
				return null;
			}
		}

		private WebResponse GetResponse(RequestParameter parameter)
		{
			var url = parameter.Url;
			var encodingOption = parameter.EncodingOption;

			if (string.IsNullOrEmpty(url))
			{
				return null;
			}

			try
			{
				var req = this.Request(url);

				if (req == null)
				{
					return null;
				}

				req.Method = parameter.RequestType.ToString();

				if (parameter.RequestType == RequestTypes.GET)
				{
					return req.GetResponse();
				}

				var postData = parameter.PostMessage;
				var byteArray = encodingOption.GetBytes(postData);

				// Set the ContentType property of the WebRequest.
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = byteArray.Length;

				using (var dataStream = req.GetRequestStream())
				{
					dataStream.Write(byteArray, 0, byteArray.Length);
				}

				return req.GetResponse();
			}
			catch (Exception ex)
			{
				Logger.WriteError("Response Error ", ex);
				return null;
			}
		}
	}
}
