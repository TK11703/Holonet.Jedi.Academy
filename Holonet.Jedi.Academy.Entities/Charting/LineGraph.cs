using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Holonet.Jedi.Academy.Entities.Charting
{
    [DataContract]
    public class LineGraph<T> : ChartDataset<T>
    {
        public LineGraph() : base()
        {
            this.fill = true;
        }

        #region Background Color Section

        [DataMember]
        public string backgroundColor { get { return DetermineBackgroundColor(); } }

        private string _backgroundColor = string.Empty;

        public void SetBackgroundColor(string value)
        {
            _backgroundColor = value;
        }

        private string DetermineBackgroundColor()
        {
            if (!string.IsNullOrEmpty(_backgroundColor))
            {
                return _backgroundColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Background Color Section

        #region Border Color Section

        [DataMember]
        public string borderColor { get { return DetermineBorderColor(); } }

        private string _borderColor = string.Empty;

        public void SetBorderColor(string value)
        {
            _borderColor = value;
        }

        private string DetermineBorderColor()
        {
            if (!string.IsNullOrEmpty(_borderColor))
            {
                return _borderColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Border Color Section

        #region Border Width Section

        [DataMember]
        public int borderWidth { get { return DetermineBorderWidth(); } }

        private int _borderWidth = 1;

        public void SetBorderWidth(int value)
        {
            _borderWidth = value;
        }

        private int DetermineBorderWidth()
        {
            if (_borderWidth > -1)
            {
                return _borderWidth;
            }
            else
            {
                return 1;
            }
        }

        #endregion Border Width Section

        #region Hover Background Color Section

        [DataMember]
        public string hoverBackgroundColor { get { return DetermineHoverBackgroundColor(); } }

        private string _hoverBackgroundColor = string.Empty;

        public void SetHoverBackgroundColor(string value)
        {
            _hoverBackgroundColor = value;
        }

        private string DetermineHoverBackgroundColor()
        {
            if (!string.IsNullOrEmpty(_hoverBackgroundColor))
            {
                return _hoverBackgroundColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Hover Background Color Section

        #region Hover Border Color Section

        [DataMember]
        public string hoverBorderColor { get { return DetermineHoverBorderColor(); } }

        private string _hoverBorderColor = string.Empty;

        public void SetHoverBorderColor(string value)
        {
            _hoverBorderColor = value;
        }

        private string DetermineHoverBorderColor()
        {
            if (!string.IsNullOrEmpty(_hoverBorderColor))
            {
                return _hoverBorderColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Hover Border Color Section

        #region Hover Border Width Section

        [DataMember]
        public int hoverBorderWidth { get { return DetermineHoverBorderWidth(); } }

        private int _hoverBorderWidth = 1;

        public void SetHoverBorderWidth(int value)
        {
            _hoverBorderWidth = value;
        }

        private int DetermineHoverBorderWidth()
        {
            if (_hoverBorderWidth > -1)
            {
                return _hoverBorderWidth;
            }
            else
            {
                return 1;
            }
        }

        #endregion Hover Border Width Section

        #region Point Background Color Section (Indexable)

        [DataMember]
        public object pointBackgroundColor { get { return DeterminePointBackgroundColor_Indexable(); } }

        private string _pointBackgroundColor = string.Empty;
        private List<string> _pointBackgroundColors = new List<string>();

        public void SetPointBackgroundColor(string value)
        {
            _pointBackgroundColor = value;
        }

        public void SetPointBackgroundColors(List<string> values)
        {
            _pointBackgroundColors = values;
        }

        private object DeterminePointBackgroundColor_Indexable()
        {
            if (_pointBackgroundColors != null && _pointBackgroundColors.Count > 0)
            {
                return _pointBackgroundColors;
            }
            else if (!string.IsNullOrEmpty(_pointBackgroundColor))
            {
                return _pointBackgroundColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Point Background Color Section (Indexable)

        #region Point Border Color Section (Indexable)

        [DataMember]
        public object pointBorderColor { get { return DeterminePointBorderColor_Indexable(); } }

        private string _pointBorderColor = string.Empty;
        private List<string> _pointBorderColors = new List<string>();

        public void SetPointBorderColor(string value)
        {
            _pointBorderColor = value;
        }
        
        public void SetPointBorderColors(List<string> values)
        {
            _pointBorderColors = values;
        }

        private object DeterminePointBorderColor_Indexable()
        {
            if (_pointBorderColors != null && _pointBorderColors.Count > 0)
            {
                return _pointBorderColors;
            }
            else if (!string.IsNullOrEmpty(_pointBorderColor))
            {
                return _pointBorderColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Point Border Color Section (Indexable)

        #region Point Border Width Section (Indexable)

        [DataMember]
        public object pointBorderWidth { get { return DeterminePointBorderWidth_Indexable(); } }

        private int _pointBorderWidth = 1;
        private List<int> _pointBorderWidths = new List<int>();

        public void SetPointBorderWidth(int value)
        {
            _pointBorderWidth = value;
        }

        public void SetPointBorderWidths(List<int> values)
        {
            _pointBorderWidths = values;
        }

        private object DeterminePointBorderWidth_Indexable()
        {
            if (_pointBorderWidths != null && _pointBorderWidths.Count > 0)
            {
                return _pointBorderWidths;
            }
            else if (_pointBorderWidth > -1)
            {
                return _pointBorderWidth;
            }
            else
            {
                return 1;
            }
        }

        #endregion Point Border Width Section (Indexable)

        #region Point Hover Background Color Section (Indexable)

        [DataMember]
        public object pointHoverBackgroundColor { get { return DeterminePointHoverBackgroundColor_Indexable(); } }

        private string _pointHoverBackgroundColor = string.Empty;
        private List<string> _pointHoverBackgroundColors = new List<string>();

        public void SetPointHoverBackgroundColor(string value)
        {
            _pointHoverBackgroundColor = value;
        }

        public void SetPointHoverBackgroundColors(List<string> values)
        {
            _pointHoverBackgroundColors = values;
        }

        private object DeterminePointHoverBackgroundColor_Indexable()
        {
            if (_pointHoverBackgroundColors != null && _pointHoverBackgroundColors.Count > 0)
            {
                return _pointHoverBackgroundColors;
            }
            else if (!string.IsNullOrEmpty(_pointHoverBackgroundColor))
            {
                return _pointHoverBackgroundColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Point Hover Background Color Section (Indexable)

        #region Point Hover Border Color Section (Indexable)

        [DataMember]
        public object pointHoverBorderColor { get { return DeterminePointHoverBorderColor_Indexable(); } }

        private string _pointHoverBorderColor = string.Empty;
        private List<string> _pointHoverBorderColors = new List<string>();

        public void SetPointHoverBorderColor(string value)
        {
            _pointHoverBorderColor = value;
        }

        public void SetPointHoverBorderColors(List<string> values)
        {
            _pointHoverBorderColors = values;
        }

        private object DeterminePointHoverBorderColor_Indexable()
        {
            if (_pointHoverBorderColors != null && _pointHoverBorderColors.Count > 0)
            {
                return _pointHoverBorderColors;
            }
            else if (!string.IsNullOrEmpty(_pointHoverBorderColor))
            {
                return _pointHoverBorderColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Point Hover Border Color Section (Indexable)

        #region Point Hover Border Width Section (Indexable)

        [DataMember]
        public object pointHoverBorderWidth { get { return DeterminePointHoverBorderWidth_Indexable(); } }

        private int _pointHoverBorderWidth = 1;
        private List<int> _pointHoverBorderWidths = new List<int>();

        public void SetPointHoverBorderWidth(int value)
        {
            _pointBorderWidth = value;
        }

        public void SetPointHoverBorderWidths(List<int> values)
        {
            _pointHoverBorderWidths = values;
        }

        private object DeterminePointHoverBorderWidth_Indexable()
        {
            if (_pointHoverBorderWidths != null && _pointHoverBorderWidths.Count > 0)
            {
                return _pointHoverBorderWidths;
            }
            else if (_pointHoverBorderWidth > -1)
            {
                return _pointHoverBorderWidth;
            }
            else
            {
                return 1;
            }
        }

        #endregion Point Hover Border Width Section (Indexable)
    }
}
