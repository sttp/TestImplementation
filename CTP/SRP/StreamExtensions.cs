using System;
using System.IO;
using GSF;
using GSF.IO;

namespace CTP
{
    public static class StreamExtensions
    {
        public static void WriteDocument(this Stream stream, object document, string rootElementName)
        {
            WriteDocument(stream, CtpDocument.Serialize(document, rootElementName));
        }

        public static void WriteDocument(this Stream stream, CtpDocument document)
        {
            byte[] data = document.ToArray();
            stream.Write(BigEndian.GetBytes(data.Length), 0, 4);
            stream.Write(data, 0, data.Length);
        }

        public static CtpDocument ReadDocument(this Stream stream)
        {
            byte[] array = new byte[4];
            stream.ReadAll(array, 0, 4);
            array = new byte[BigEndian.ToInt32(array, 0)];
            stream.ReadAll(array, 0, array.Length);
            return new CtpDocument(array);
        }

        public static T ReadDocument<T>(this Stream stream, string rootElementName)
        {
            var document = ReadDocument(stream);
            if (document.RootElement != rootElementName)
                throw new Exception("Document Mismatch");
            return CtpDocument.Load<T>(document);
        }
    }
}