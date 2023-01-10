using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Holonet.Jedi.Academy.Entities.Charting
{
    [DataContract]
    public class BarGraph<T> : ChartDataset<T>
    {
        public BarGraph() : base()
        {
            
        }

        #region Background Color Section (Indexable)

        [DataMember]
        public object backgroundColor { get { return DetermineBackgroundColor_Indexable(); } }

        private string _backgroundColor = string.Empty;
        private List<string> _backgroundColors = new List<string>();

        public void SetBackgroundColor(string value)
        {
            _backgroundColor = value;
        }

        public void SetBackgroundColors(List<string> values)
        {
            _backgroundColors = values;
        }

        private object DetermineBackgroundColor_Indexable()
        {
            if (_backgroundColors != null && _backgroundColors.Count > 0)
            {
                return _backgroundColors;
            }
            else if (!string.IsNullOrEmpty(_backgroundColor))
            {
                return _backgroundColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Background Color Section (Indexable)

        #region Border Color Section (Indexable)

        [DataMember]
        public object borderColor { get { return DetermineBorderColor_Indexable(); } }

        private string _borderColor = string.Empty;
        private List<string> _borderColors = new List<string>();

        public void SetBorderColor(string value)
        {
            _borderColor = value;
        }

        public void SetBorderColors(List<string> values)
        {
            _borderColors = values;
        }

        private object DetermineBorderColor_Indexable()
        {
            if (_borderColors != null && _borderColors.Count > 0)
            {
                return _borderColors;
            }
            else if (!string.IsNullOrEmpty(_borderColor))
            {
                return _borderColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Border Color Section (Indexable)

        #region Border Width Section (Indexable)

        [DataMember]
        public object borderWidth { get { return DetermineBorderWidth_Indexable(); } }

        private int _borderWidth = 1;
        private List<int> _borderWidths = new List<int>();

        public void SetBorderWidth(int value)
        {
            _borderWidth = value;
        }

        public void SetBorderWidths(List<int> values)
        {
            _borderWidths = values;
        }

        private object DetermineBorderWidth_Indexable()
        {
            if (_borderWidths != null && _borderWidths.Count > 0)
            {
                return _borderWidths;
            }
            else if (_borderWidth > -1)
            {
                return _borderWidth;
            }
            else
            {
                return 1;
            }
        }

        #endregion Border Width Section (Indexable)

        #region Hover Background Color Section (Indexable)

        [DataMember]
        public object hoverBackgroundColor { get { return DetermineHoverBackgroundColor_Indexable(); } }

        private string _hoverBackgroundColor = string.Empty;
        private List<string> _hoverBackgroundColors = new List<string>();

        public void SetHoverBackgroundColor(string value)
        {
            _hoverBackgroundColor = value;
        }

        public void SetHoverBackgroundColors(List<string> values)
        {
            _hoverBackgroundColors = values;
        }

        private object DetermineHoverBackgroundColor_Indexable()
        {
            if (_hoverBackgroundColors != null && _hoverBackgroundColors.Count > 0)
            {
                return _hoverBackgroundColors;
            }
            else if (!string.IsNullOrEmpty(_hoverBackgroundColor))
            {
                return _hoverBackgroundColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Hover Background Color Section (Indexable)

        #region Hover Border Color Section (Indexable)

        [DataMember]
        public object hoverBorderColor { get { return DetermineHoverBorderColor_Indexable(); } }

        private string _hoverBorderColor = string.Empty;
        private List<string> _hoverBorderColors = new List<string>();

        public void SetHoverBorderColor(string value)
        {
            _hoverBorderColor = value;
        }

        public void SetHoverBorderColors(List<string> values)
        {
            _hoverBorderColors = values;
        }

        private object DetermineHoverBorderColor_Indexable()
        {
            if (_hoverBorderColors != null && _hoverBorderColors.Count > 0)
            {
                return _hoverBorderColors;
            }
            else if (!string.IsNullOrEmpty(_hoverBorderColor))
            {
                return _hoverBorderColor;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Hover Border Color Section (Indexable)

        #region Hover Border Width Section (Indexable)

        [DataMember]
        public object hoverBorderWidth { get { return DetermineHoverBorderWidth_Indexable(); } }

        private int _hoverBorderWidth = 1;
        private List<int> _hoverBorderWidths = new List<int>();

        public void SetHoverBorderWidth(int value)
        {
            _hoverBorderWidth = value;
        }

        public void SetHoverBorderWidths(List<int> values)
        {
            _hoverBorderWidths = values;
        }

        private object DetermineHoverBorderWidth_Indexable()
        {
            if (_hoverBorderWidths != null && _hoverBorderWidths.Count > 0)
            {
                return _hoverBorderWidths;
            }
            else if (_hoverBorderWidth > -1)
            {
                return _hoverBorderWidth;
            }
            else
            {
                return 1;
            }
        }

        #endregion Hover Border Width Section (Indexable)
    }
}
