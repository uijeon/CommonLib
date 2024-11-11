using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Jeon.CommonFramework.Utils
{
	public enum SerializeFormats
	{
		/// <summary>
		/// Json
		/// </summary>
		Json,

		/// <summary>
		/// Xml
		/// </summary>
		Xml,

		/// <summary>
		/// Binary
		/// </summary>
		Binary
	}

	/// <summary>
	/// Encoding Set StringWriter
	/// </summary>
	public class EncodingStringWriter : StringWriter
	{
		private readonly Encoding _encoding = Encoding.UTF8;
		public EncodingStringWriter(Encoding encoding)
		{
			this._encoding = encoding;
		}
		public override Encoding Encoding { get { return this._encoding; } }
	}

	/// <summary>
	/// Serializer Util Class
	/// </summary>
	public static class Serializer
	{
		/// <summary>
		/// Serialize from T Type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="model"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string Serialize<T>(T model, SerializeFormats format)
		{
			switch (format)
			{
				case SerializeFormats.Json:
					return SerializeByJson(model);

				case SerializeFormats.Xml:
					return SerializeByXml(model);

				case SerializeFormats.Binary:
					return SerializeByBinary(model);
			}

			return null;
		}

		/// <summary>
		/// Json Deserialize
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string SerializeByJson<T>(T model)
		{
			return JsonConvert.SerializeObject(model);
		}

		/// <summary>
		/// Xml Deserialize
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string SerializeByXml<T>(T model)
		{
			var serializer = new XmlSerializer(model.GetType());

			using (var sw = new EncodingStringWriter(Encoding.UTF8))
			{
				//Serialize
				serializer.Serialize(sw, model);
				return sw.ToString();
			}
		}

		private static string SerializeByBinary<T>(T model)
		{
			using (var ms = new MemoryStream())
			{
				var bf = new BinaryFormatter();
				bf.Serialize(ms, model);
				ms.Position = 0;
				return Convert.ToBase64String(ms.ToArray());
			}
		}

		/// <summary>
		/// Deserialize to T Type (get stream)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static T Deserialize<T>(Stream stream, SerializeFormats format)
		{
			using (var reader = new StreamReader(stream))
			{
				var getString = reader.ReadToEnd();

				return Deserialize<T>(getString, format);
			}
		}

		/// <summary>
		/// Deserialize to T Type (get string)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static T Deserialize<T>(string value, SerializeFormats format)
		{
			try
			{
				switch (format)
				{
					case SerializeFormats.Json:
						return DeserializeByJson<T>(value);

					case SerializeFormats.Xml:
						return DeserializeByXml<T>(value);

					case SerializeFormats.Binary:
						return DeserializeByBinary<T>(value);
				}

				return default(T);
			}
			catch (Exception e)
			{
				Logger.WriteError("Deserialize Fail!!", e);
				return default(T);
			}
		}

		/// <summary>
		/// Json Deserialize
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		private static T DeserializeByJson<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		/// <summary>
		/// Xml Deserialize
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		private static T DeserializeByXml<T>(string value)
		{
			var serializer = new XmlSerializer(typeof(T));

			using (var stringReader = new StringReader(value))
			{
				using (var xmlReader = new XmlTextReader(stringReader))
				{
					return (T)serializer.Deserialize(xmlReader);
				}
			}
		}

		private static T DeserializeByBinary<T>(string value)
		{
			var bytes = Convert.FromBase64String(value);

			using (var ms = new MemoryStream(bytes))
			{
				return (T)new BinaryFormatter().Deserialize(ms);
			}
		}
	}
}
