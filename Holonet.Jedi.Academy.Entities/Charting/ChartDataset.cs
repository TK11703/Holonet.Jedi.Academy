using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Holonet.Jedi.Academy.Entities.Charting
{
    [DataContract]
    public class ChartDataset<T>
    {
        [DataMember]
        public string label { get; set; }

        [DataMember]
        public int order{ get; set; }

        [DataMember]
        public List<T> data { get; set; }

        [DataMember]
        public bool fill { get; set; }

        public ChartDataset()
        {
            this.order = 0;
            this.label = string.Empty;
            this.data = new List<T>();
            this.fill = false;
        }
    }
}
