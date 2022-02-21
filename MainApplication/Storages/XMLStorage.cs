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
            throw new NotImplementedException();
        }

        public override List<T> GetAllElements()
        {
            throw new NotImplementedException();
        }

        public override Void WriteElement(T obj)
        {
            throw new NotImplementedException();
        }

        public override Void AddNewElement(T obj)
        {
            throw new NotImplementedException();
        }

        public override Void AddNewElementWithoutRewrite(T obj)
        {
            throw new NotImplementedException();
        }

        public override Void RemoveElement(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public override GetElementBy(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public override Boolean EditElementBy(Predicate<T> match, T obj)
        {
            throw new NotImplementedException();
        }

        public override Void ClearFile()
        {
            throw new NotImplementedException();
        }
    }
}