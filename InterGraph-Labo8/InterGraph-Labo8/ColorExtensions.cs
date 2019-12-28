using System.Windows.Media;
using System.Xml;

namespace InterGraph_Labo8
{
    public static class ColorExtensions
    {
        public static void XmlRead(this Color color, XmlReader reader, string colorName)
        {
            reader.ReadStartElement(colorName);
            color.R = (byte)reader.ReadElementContentAsInt("R", "");
            color.G = (byte)reader.ReadElementContentAsInt("G", "");
            color.B = (byte)reader.ReadElementContentAsInt("B", "");
            reader.ReadEndElement();

        }
        public static void XmlWrite(this Color color, XmlWriter writer, string colorName)
        {
            writer.WriteStartElement(colorName);
            writer.WriteElementString("R", color.R.ToString());
            writer.WriteElementString("G", color.G.ToString());
            writer.WriteElementString("B", color.B.ToString());
            writer.WriteEndElement();
        }
    }
}
