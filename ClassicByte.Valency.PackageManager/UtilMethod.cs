using System;
using System.Xml.Serialization;

namespace ClassicByte.Valency.PackageManager;

public static class UtilMethod
{
    public static void SerializeObjectToXml(object obj, string filePath)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        XmlSerializer serializer = new(obj.GetType());
        using StreamWriter writer = new(filePath);
        serializer.Serialize(writer, obj);
    }

    public static T DeserializeXmlToObject<T>(string filePath) where T : class
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("指定的 XML 文件不存在", filePath);
        }

        XmlSerializer serializer = new(typeof(T));

        using StreamReader reader = new(filePath);
        if (serializer.Deserialize(reader) is not T deserializedObject)
        {
            throw new InvalidDataException("XML 文件内容无法反序列化为指定的 .NET 类型");
        }

        return deserializedObject;
    }
}
