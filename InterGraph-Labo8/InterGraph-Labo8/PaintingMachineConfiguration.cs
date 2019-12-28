using System.Windows.Media;
using System.Xml;

namespace InterGraph_Labo8
{
    public class PaintingMachineConfiguration
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public PaintingMachineConfiguration()
        {
            Flow = 0.0;
            ColorA = new Color();
            ColorB = new Color();
            ColorC = new Color();
            ColorD = new Color();
        }

        public PaintingMachineConfiguration(double flow, Color colorA, 
            Color colorB, Color colorC, Color colorD)
        {
            Flow = flow;
            ColorA = colorA;
            ColorB = colorB;
            ColorC = colorC;
            ColorD = colorD;
        }

        #endregion

        #region Properties
        public double ComputationPerSeconds { get; } = 1000 / 250;
        public Color ColorA { get; set; }
        public Color ColorB { get; set; }
        public Color ColorC { get; set; }
        public Color ColorD { get; set; }
        public double Flow { get; set; }
        
        #endregion

        #region Methods

        

        public void XmlRead(XmlReader reader)
        {
            reader.ReadStartElement(nameof(PaintingMachineConfiguration));
            Flow = reader.ReadElementContentAsDouble(nameof(Flow), "");
            reader.ReadStartElement("PaintColors");
            ColorA.XmlRead(reader, nameof(ColorA));
            ColorB.XmlRead(reader, nameof(ColorB));
            ColorC.XmlRead(reader, nameof(ColorC));
            ColorD.XmlRead(reader, nameof(ColorD));
            reader.ReadEndElement();
            reader.ReadEndElement();
        }
        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(PaintingMachineConfiguration));
            writer.WriteElementString(nameof(Flow), Flow.ToString());
            writer.WriteStartElement("PaintColors");
            ColorA.XmlWrite(writer, nameof(ColorA));
            ColorB.XmlWrite(writer, nameof(ColorB));
            ColorC.XmlWrite(writer, nameof(ColorC));
            ColorD.XmlWrite(writer, nameof(ColorD));
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        #endregion
    }
}
