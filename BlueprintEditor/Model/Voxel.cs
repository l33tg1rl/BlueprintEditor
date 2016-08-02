using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace BlueprintEditor.Model
{
    [Serializable]
    public class Voxel
    {
        [XmlElement("XCoordinate")]
        public decimal XCoord { get; set; }

        [XmlElement("YCoordinate")]
        public decimal YCoord { get; set; }

        [XmlElement("ZCoordinate")]
        public decimal ZCoord { get; set; }

        [XmlElement("VectorColor")]
        public Color VectorColor { get; set; }
    }
}