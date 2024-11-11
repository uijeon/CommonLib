using System.Linq;
using System.Net;
using System.Xml;

namespace Jeon.CommonFramework.Utils
{
	/// <summary>
	/// 유효성 체크 유틸리티 Class
	/// </summary>
	public static class ValidationCheck
	{
		/// <summary>
		/// IP Address 체크
		/// </summary>
		/// <param name="ipString"></param>
		/// <returns></returns>
		public static bool IPAddressValidateCheck(string ipString)
		{
			if (ipString.ToLower() == "localhost")
			{
				return true;
			}

			if (ipString.Count(c => c == '.') != 3)
			{
				return false;
			}

			IPAddress address;
			return IPAddress.TryParse(ipString, out address);
		}

		/// <summary>
		/// XML 형식 체크
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsValidXml(string text)
		{
			var errored = false;
			var reader = new XmlValidatingReader(text, XmlNodeType.Element, new XmlParserContext(null, new XmlNamespaceManager(new NameTable()), null, XmlSpace.None));
			reader.ValidationEventHandler += (sender, e) =>
			{
				errored = e.Severity == System.Xml.Schema.XmlSeverityType.Error;
			};

			while (reader.Read()) {; }
			reader.Close();
			return errored == false;
		}

		//TODO : 유효성체크 매서드 추가
	}
}
