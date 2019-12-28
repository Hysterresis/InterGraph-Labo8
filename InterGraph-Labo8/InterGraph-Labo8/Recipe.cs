using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using System.Xml;

namespace InterGraph_Labo8
{
    /// <summary>
    /// 
    /// </summary>
    public class Recipe
    {
        #region Constantes
        private const double MinQuantity = 0.0;
        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public Recipe(double maxQuality = 0.0,
            double quantityA = 0.0, double quantityB = 0.0,
            double quantityC = 0.0, double quantityD = 0.0)
        {
            MaxQuality = maxQuality;
            QuantityA = quantityA;
            QuantityB = quantityB;
            QuantityC = quantityC;
            QuantityD = quantityD;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public Recipe(Recipe source) :
            this(source.MaxQuality,
                source.QuantityA, source.QuantityB, source.QuantityC, source.QuantityD)
        {
        }

        #endregion

        #region Properties

        public double MaxQuality { get; set; }
        public double QuantityA { get; set; }
        public double QuantityB { get; set; }
        public double QuantityC { get; set; }
        public double QuantityD { get; set; }
        public double TotalQuantity => (QuantityA + QuantityB + QuantityC + QuantityD);

        #endregion

        #region Methods

        public Color FinalColor(PaintingMachineConfiguration machineConfiguration)
        {
            Color finalColor = Color.FromArgb(255, 255, 255, 255); //Init to white
            //Add A pigment
            for (int i = 0; i < (int)(machineConfiguration.ComputationPerSeconds * QuantityA / machineConfiguration.Flow); i++)
            {
                finalColor = StaticLib.MixColors(finalColor, machineConfiguration.ColorA);
            }
            //Add B pigment
            for (int i = 0; i < (int)(machineConfiguration.ComputationPerSeconds * QuantityB / machineConfiguration.Flow); i++)
            {
                finalColor = StaticLib.MixColors(finalColor, machineConfiguration.ColorB);
            }
            //Add C pigment
            for (int i = 0; i < (int)(machineConfiguration.ComputationPerSeconds * QuantityC / machineConfiguration.Flow); i++)
            {
                finalColor = StaticLib.MixColors(finalColor, machineConfiguration.ColorC);
            }
            //Add D pigment
            for (int i = 0; i < (int)(machineConfiguration.ComputationPerSeconds * QuantityD / machineConfiguration.Flow); i++)
            {
                finalColor = StaticLib.MixColors(finalColor, machineConfiguration.ColorD);
            }

            return finalColor;
        }

        public double TotalTime(double flow) => (TotalQuantity/flow);

        public void XmlRead(XmlReader reader)
        {
            reader.ReadStartElement(nameof(Recipe));
            MaxQuality = reader.ReadElementContentAsDouble(nameof(MaxQuality), "");
            QuantityA = reader.ReadElementContentAsDouble(nameof(QuantityA), "");
            QuantityB = reader.ReadElementContentAsDouble(nameof(QuantityB), "");
            QuantityC = reader.ReadElementContentAsDouble(nameof(QuantityC), "");
            QuantityD = reader.ReadElementContentAsDouble(nameof(QuantityD), "");
            reader.ReadEndElement();
        }

        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(Recipe));
            writer.WriteElementString(nameof(MaxQuality),
                MaxQuality.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString(nameof(QuantityA),
                QuantityA.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString(nameof(QuantityB),
                QuantityB.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString(nameof(QuantityC),
                QuantityC.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString(nameof(QuantityD),
                QuantityD.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }

        #endregion
    }
}
