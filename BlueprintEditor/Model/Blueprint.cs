using System.Collections.Generic;
using System.Xml.Serialization;

namespace BlueprintEditor.Model
{
    public class Blueprint
    {
        [XmlArray(ElementName = "Voxels")]
        [XmlArrayItem(typeof(Voxel), ElementName = "Voxel")]
        public List<Voxel> Voxels { get; set; }
    }
}
