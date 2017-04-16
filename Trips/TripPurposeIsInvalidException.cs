using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trips
{
    [Serializable]
    public class TripPurposeIsInvalidException: Exception
    {
        public TripPurposeIsInvalidException() { }
        public TripPurposeIsInvalidException(string message): base(message) { }
        public TripPurposeIsInvalidException(string message, System.Exception inner) : base(message, inner) { }
        protected TripPurposeIsInvalidException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }       
    }
}
