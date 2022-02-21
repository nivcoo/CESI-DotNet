using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MainApplication.Storages.NamingPolicies;

namespace MainApplication.Storages;

{
    public class XMLStorage<T> : AStorage<T>
    
    {
        public XMLStorage(String filePath) : base(filePath)
        {
        }

        public override GetElement()
        {
              var obj = RunMutexFunc(() =>
        {
            var text = File.ReadAllText(FilePath);

            if (text == "")
                return default;
                    var elementsList = xmlSerializer.Deserialize<List<T>>(text.Trim(), _serializerOptions);

            return elementsList ?? default;
        });

        return (T?) obj;
        }

        public override List<T> GetAllElements()
        {
            var list = RunMutexFunc(() =>
        {
            var text = File.ReadAllText(FilePath);
            if (text == "")
                return new List<T>();
            var elementsList = xmlSerializer.Deserialize<List<T>>(text.Trim(), _serializerOptions);
            return elementsList ?? new List<T>();
        }) as List<T>;

        if (list != default)
            return list;
        return new List<T>();
        }

        public override Void WriteElement(T obj)
        {
             if (obj != null)
            SerializeAndSaveIntoFiles(obj);
        }

        public override Void AddNewElement(T obj)
        {
             var elementsList = GetAllElements();
        elementsList.Add(obj);
        SerializeAndSaveIntoFiles(elementsList);
        }

        public override Void AddNewElementWithoutRewrite(T obj)
        {
            //voir pout le XML si ca fontionne 
          /*   RunMutexAction(() =>
        {
            var objects = new List<T> {obj};
            using var fs = new FileStream(FilePath, FileMode.Open);
            var serializeObject = SerializeObject(objects);
            using var sw = new StreamWriter(fs);
            if (fs.Length > 1)
            {
                fs.Position = fs.Seek(-1, SeekOrigin.End);

                if (fs.ReadByte() == ']')
                {
                    fs.SetLength(fs.Length - 4 - Environment.NewLine.Length);
                    var serializeObjectWithoutFirst = Regex.Split(serializeObject, Environment.NewLine).Skip(1);
                    serializeObject = string.Join(Environment.NewLine, serializeObjectWithoutFirst.ToArray());
                    sw.Write("  }," + Environment.NewLine);
                }
            }
            sw.Write(serializeObject);
            sw.Close();
            fs.Close();
        });*/
        }

        public override Void RemoveElement(Predicate<T> match)
        {
                 var elementsList = GetAllElements();
        var element = elementsList.Find(match);
        if (element == null)
            return;
        elementsList.Remove(element);
        SerializeAndSaveIntoFiles(elementsList);
        }

        public override GetElementBy(Predicate<T> match)
        {
            var elementsList = GetAllElements();
        return elementsList.Find(match);
        }

        public override Boolean EditElementBy(Predicate<T> match, T obj)
        {
              var elementsList = GetAllElements();
        var selected = elementsList.Find(match);
        if (selected == null)
            return false;
        elementsList.Remove(selected);
        elementsList.Add(obj);
        SerializeAndSaveIntoFiles(elementsList);
        return true;
        }

        public override Void ClearFile()
        {
           RunMutexAction(() => File.WriteAllText(FilePath, string.Empty));
        }
        private string SerializeObject(object obj)
    {
        return xmlSerializer.Serialize(obj, _serializerOptions);
    }
    }
}