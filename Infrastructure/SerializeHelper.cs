using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WallExporter.Infrastructure
{
    internal static class SerializeHelper
    {
        #region Public methods

        public static T Deserialise<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("json");

            T obj = Activator.CreateInstance<T>();

            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                obj = (T)serializer.ReadObject(stream);
            }

            return obj;
        }

        public static string FixResponseArrayString(string response)
        {
            if (string.IsNullOrEmpty(response))
                return string.Empty;

            bool found = false;
            int index = response.IndexOf("[");

            if (index > -1)
            {
                found = true;
            }

            while (found)
            {
                string emptyArray = response.Substring(index, 3);
                if (emptyArray == "[0]")
                {
                    response = response.Remove(index, 1);
                }
                else
                {
                    int firstComma = response.IndexOf(",", index);
                    if (firstComma > -1)
                    {
                        string countOfElements = response.Substring(index + 1, firstComma - index - 1);

                        bool ok = true;
                        for (int i = 0; i < countOfElements.Length; i++)
                        {
                            if (!Char.IsDigit(countOfElements[i]))
                            {
                                ok = false;
                                break;
                            }
                        }

                        if (ok)
                        {
                            response = response.Remove(index + 1, firstComma - index); // Remove with comma
                        }
                    }
                }

                index = response.IndexOf("[", index + 1);
                if (index > -1)
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
            }


            //int emptyArray = response.IndexOf("[0]");

            //if (emptyArray > -1)
            //{
            //    response = response.Remove(RESPONSE_HEADER_LENGTH, 1);
            //}
            //else
            //{
            //    int firstComma = response.IndexOf(",");
            //    if (firstComma < RESPONSE_HEADER_LENGTH || firstComma == -1)
            //        return response; // Don't need to fix.

            //    response = response.Remove(RESPONSE_HEADER_LENGTH, firstComma - (RESPONSE_HEADER_LENGTH - 1));
            //}

            return response;
        }

        #endregion

        #region Private constants

        private const int RESPONSE_HEADER_LENGTH = 13;

        #endregion
    }
}
