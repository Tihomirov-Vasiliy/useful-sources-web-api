using System;

namespace Domain.Exceptions
{
    [Serializable]
    public class ObjectNotFoundException:Exception
    {
        public ObjectNotFoundException(){}
        public ObjectNotFoundException(string message) : base(message) { }
    }
}
