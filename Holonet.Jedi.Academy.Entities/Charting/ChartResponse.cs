using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Holonet.Jedi.Academy.Entities.Charting
{
    [DataContract]
    public class ChartResponse<T>
    {
        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string xAxisLabel { get; set; }

        [DataMember]
        public string yAxisLabel { get; set; }

        [DataMember]
        public List<string> labels { get; set; }

        [DataMember]
        public List<ChartDataset<T>> datasets { get; set; }

        public ChartResponse(string chartType)
        {
            this.labels = new List<string>();
            this.datasets = new List<ChartDataset<T>>();
            this.type = chartType;
        }
    }
}
