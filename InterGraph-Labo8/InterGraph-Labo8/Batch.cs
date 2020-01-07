using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml;

namespace InterGraph_Labo8
{
    public class Batch : INotifyPropertyChanged
    {
        #region PropretyChangeInterface

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void DoPropertyChanged(string preopretyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(preopretyName));
        }

        #endregion

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
        public TimeSpan TotalTime { get { return totalTime; } }
        private TimeSpan totalTime;
        
        public TimeSpan CurrentProductionTime
        {
            get { return currentProductionTime; }
            set
            {
                currentProductionTime = value;
                DoPropertyChanged(nameof(CurrentProductionTime));
                
            }
        }
        private TimeSpan currentProductionTime;

        #endregion

        #region Methods

        /// <summary>
        /// Permet de définir et enregistrer dans la propriété "TotalTime" le temps total de
        /// production du lot, du moment où le premier seau est là jusqu'à ce que le dernier
        /// soit remplit
        /// </summary>
        /// <param name="flow"></param>
        /// <param name="bucketMovingTime"></param>
        public void SetTotalTime(double flow, TimeSpan bucketMovingTime) => totalTime = (Recipe.TotalTime(flow, bucketMovingTime).Multiply(NumberOfElements) - bucketMovingTime);
       
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
