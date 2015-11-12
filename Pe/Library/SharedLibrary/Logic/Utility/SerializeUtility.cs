﻿/**
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using Newtonsoft.Json;

    /// <summary>
    /// 設定の入出力。
    /// </summary>
    public static class SerializeUtility
    {
        /// <summary>
        /// DataContract属性を保持しているか。
        /// <para>http://stackoverflow.com/questions/221687/can-you-use-where-to-require-an-attribute-in-c</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static bool HasDataContract<T>()
        {
            var results = typeof(T).GetCustomAttributes(typeof(DataContractAttribute), true);
            return results != null && results.Any();
        }

        /// <summary>
        /// Serializable属性を保持しているか。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static bool HasSerializable<T>()
        {
            var results = typeof(T).GetCustomAttributes(typeof(SerializableAttribute), false);
            return results != null && results.Any();
        }

        /// <summary>
        /// ファイル入力用ストリームを作成。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static FileStream CreateReadFileStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// ファイル出力用ストリームを作成。
        /// <para>親ディレクトリが必要なら勝手に作る。</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static FileStream CreateWriteFileStream(string path)
        {
            FileUtility.MakeFileParentDirectory(path);
            return new FileStream(path, FileMode.Create, FileAccess.Write);
        }

        /// <summary>
        /// XMLストリーム読み込み。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T LoadXmlDataFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasDataContract<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            using(var xmlReader = XmlReader.Create(stream)) {
                var serializer = new DataContractSerializer(typeof(T));
                var result = (T)serializer.ReadObject(xmlReader);
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// XMLファイル読み込み。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadXmlDataFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadXmlDataFromStream<T>(stream);
            }
        }

        /// <summary>
        /// XMLストリーム読み込み。
        /// <para>Serializableを使用。</para>
        /// <para>http://stackoverflow.com/questions/2209443/c-sharp-xmlserializer-bindingfailure</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T LoadXmlSerializeFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasSerializable<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            using(var reader = new XmlTextReader(stream)) {
                //var serializer = new XmlSerializer(typeof(T));
                var serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                var result = (T)serializer.Deserialize(reader);
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// XMLファイル読み込み。
        /// <para>Serializableを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T LoadXmlSerializeFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadXmlSerializeFromStream<T>(stream);
            }
        }


        /// <summary>
        /// XMLストリーム書き出し。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        public static void SaveXmlDataToStream<T>(Stream stream, T model)
             where T : IModel
        {
            Debug.Assert(model != null);

            if(!HasDataContract<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            var xmlSetting = new XmlWriterSettings() {
                Encoding = new System.Text.UTF8Encoding(),
                OmitXmlDeclaration = false,
                Indent = true,
                IndentChars = "\t",
            };
            using(var xmlWriter = XmlWriter.Create(stream, xmlSetting)) {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(xmlWriter, model);
            }
        }
        /// <summary>
        /// XMLファイル書き出し。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveXmlDataToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveXmlDataToStream(stream, model);
            }
        }

        /// <summary>
        /// XMLストリーム書き出し。
        /// <para>Serializableを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        public static void SaveXmlSerializeToStream<T>(Stream stream, T model)
            where T : IModel
        {
            Debug.Assert(model != null);

            if(!HasSerializable<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, model);
        }

        /// <summary>
        /// XMLファイル書き出し。
        /// <para>Serializableを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveXmlSerializeToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveXmlSerializeToStream(stream, model);
            }
        }

        /// <summary>
        /// XMLストリーム読み込み。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T LoadJsonDataFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasDataContract<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            //var serializer = new DataContractJsonSerializer(typeof(T));
            //return (T)serializer.ReadObject(stream);
            using(var reader = new StreamReader(stream)) {
                var result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// XMLファイル読み込み。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadJsonDataFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadJsonDataFromStream<T>(stream);
            }
        }
        /// <summary>
        /// Jsonストリーム書き出し。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveJsonDataToStream<T>(Stream stream, T model)
            where T : IModel
        {
            if(!HasDataContract<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            var setting = new JsonSerializerSettings() {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
            };

            var jsonString = JsonConvert.SerializeObject(model, setting);
            using(var writer = new StreamWriter(stream)) {
                writer.Write(jsonString);
            }
        }

        /// <summary>
        /// Jsonファイル書き出し。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveJsonDataToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveJsonDataToStream(stream, model);
            }
        }

        /// <summary>
        /// バイナリストリーム書き出し。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        public static void SaveBinaryDataToStream<T>(Stream stream, T model)
            where T : IModel
        {
            if(!HasDataContract<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            using(var writer = XmlDictionaryWriter.CreateBinaryWriter(stream)) {

                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(writer, model);
            }
        }

        /// <summary>
        /// バイナリファイル書き出し。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveBinaryDataToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveBinaryDataToStream(stream, model);
            }
        }

        public static T LoadBinaryDataFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasDataContract<T>()) {
                throw new InvalidOperationException(typeof(T).ToString());
            }

            var quotas = new XmlDictionaryReaderQuotas() {
                MaxArrayLength = (int)stream.Length,
                MaxStringContentLength = (int)stream.Length,
            };
            using(var reader = XmlDictionaryReader.CreateBinaryReader(stream, null, quotas)) {
                //reader.Read();
                var serializer = new DataContractSerializer(typeof(T));
                var result = (T)serializer.ReadObject(reader);
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// バイナリファイル読み込み。
        /// <para>DataContractを使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadBinaryDataFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadBinaryDataFromStream<T>(stream);
            }
        }
    }
}
