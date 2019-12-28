using System;
using System.ComponentModel;
using System.Xml;

namespace InterGraph_Labo8
{
    public class Batch
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public Batch(Recipe recipe = null, int numberOfElements = 0, int id = 0)
        {
            Recipe = recipe != null ? new Recipe(recipe) : new Recipe();
            NumberOfElements = numberOfElements;
            Id = id;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public Batch(Batch source) : this(source.Recipe, source.NumberOfElements, source.Id) { }

        #endregion

        #region Properties

        public int Id { get; set; }
        public Recipe Recipe { get; set; }
        public int NumberOfElements { get; set; }

        #endregion

        #region Methods

        public double TotalTime(double flow) => (Recipe.TotalTime(flow)* NumberOfElements);

        public void XmlRead(XmlReader reader)
        {
            reader.ReadStartElement(nameof(Batch));
            Id = reader.ReadElementContentAsInt(nameof(Id), "");
            NumberOfElements = reader.ReadElementContentAsInt(nameof(NumberOfElements), "");
            Recipe.XmlRead(reader);
            reader.ReadEndElement();
        }
        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(Batch));
            writer.WriteElementString(nameof(Id), Id.ToString());
            writer.WriteElementString(nameof(NumberOfElements), NumberOfElements.ToString());
            Recipe.XmlWrite(writer);
            writer.WriteEndElement();
        }

        #endregion
    }
}
