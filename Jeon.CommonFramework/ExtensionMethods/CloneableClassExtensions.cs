using Jeon.CommonFramework.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Jeon.CommonFramework.ExtensionMethods
{
	public static class CloneableClassExtensions
	{
		/// <summary>
		/// 받은 객체를 Deep Copy 하여 반환한다. (주의: 단순 데이터(Model) 형식으로 사용되는 객체만 사용하는 것을 권장)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		public static T DeepCopyMemberwiseClone<T>(this object data) where T : class
		{
			if (data == null)
			{
				return null;
			}

			//제네릭 인자가 있는 Class 타입은 반드시 ISerializable 를 구현할 것.
			//ISerializable 를 구현한 타입은 BinaryFormatter 사용.
			if (typeof(ISerializable).IsAssignableFrom(data.GetType())) // data.GetType().IsSerializable
			{
				var binaryText = Serializer.Serialize(data, SerializeFormats.Binary);

				return Serializer.Deserialize<T>(binaryText, SerializeFormats.Binary);
			}

			var serializedText = Serializer.Serialize(data, SerializeFormats.Xml);

			return Serializer.Deserialize<T>(serializedText, SerializeFormats.Xml);
		}
	}
}
