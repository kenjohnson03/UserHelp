using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appy.Classes;
using Newtonsoft.Json.Linq;

namespace Appy.Data
{
    internal class ParseCategoriesJSON
    {
        public static List<Category> GetData(string filePath)
        {
            byte[] result;

            using (FileStream SourceStream = File.Open(filePath, FileMode.Open))
            {
                result = new byte[SourceStream.Length];
                SourceStream.Read(result, 0, (int)SourceStream.Length);
            }

            string fileText = System.Text.Encoding.ASCII.GetString(result);
            
            JObject j = JObject.Parse(fileText);
            JArray a = (JArray)j["Categories"];
            return a.ToObject<List<Category>>();
        }
    }
}
