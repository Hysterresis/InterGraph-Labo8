using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

namespace InterGraph_Labo8
{
    public class PaintingMachineConfiguration
    {
        #region Constants
        
        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public PaintingMachineConfiguration(double flow = 0.0, Color colorA = new Color(),
            Color colorB = new Color(), Color colorC = new Color(), Color colorD = new Color())
        {
            Flow = flow;
            ColorA = colorA;
            ColorB = colorB;
            ColorC = colorC;
            ColorD = colorD;
        }

        #endregion

        #region Propreties
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
