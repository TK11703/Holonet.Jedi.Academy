using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Holonet.Jedi.Academy.Entities.Charting
{
    [DataContract]
    public class DoughnutGraph<T> : PieGraph<T>
    {
        [DataMember]
        public int cutoutPercentage { get; set; }

        public DoughnutGraph() : base()
        {
            cutoutPercentage = 85;
        }
    }
}
