using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Holonet.Jedi.Academy.Entities.Charting
{
    [DataContract]
    public class PolarAreaGraph<T> : PieGraph<T>
    {

        public PolarAreaGraph() : base()
        {
        }
    }
}
