using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Holonet.Jedi.Academy.Entities
{
    [DataContract]
    public class JSONResponse
    {
        private bool m_success = false;
        [DataMember]
        public bool Success
        {
            get { return m_success; }
            set { m_success = value; }
        }

        private List<string> m_errors = null;
        [DataMember]
        public List<string> Errors
        {
            get { return m_errors; }
            set { m_errors = value; }
        }

        public JSONResponse()
        {
            this.Errors = new List<string>();
        }

    }
}
