using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml;

namespace InterGraph_Labo8
{
    public class BatchList : INotifyPropertyChanged
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
        public BatchList()
        {
            Batches = new ObservableCollection<Batch>();
        }

        #endregion

        #region Properties
        public int NextBatchId { get; set; }
        public ObservableCollection<Batch> Batches { get; set; }
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

        public void InitBatchesProperties(PaintingMachineConfiguration paintingMachineConfiguration, TimeSpan bucketMovingTime)
        {
            foreach (var batch in Batches)
            {
                batch.Recipe.SetFinalColor(paintingMachineConfiguration);
                batch.SetTotalTime(paintingMachineConfiguration.Flow, bucketMovingTime);
                totalTime += batch.TotalTime;
            }
            DoPropertyChanged(nameof(TotalTime));
        }

        public void XmlRead(XmlReader reader)
        {
            reader.ReadStartElement(nameof(BatchList));
            NextBatchId = reader.ReadElementContentAsInt(nameof(NextBatchId), "");
            while (reader.IsStartElement(nameof(Batch)))
            {
                Batches.Add(new Batch());
                Batches.Last().XmlRead(reader);
            }
        }
        #endregion
    }
}
