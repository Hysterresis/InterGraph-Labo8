using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace InterGraph_Labo8
{
    public class BatchList
    {
        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public BatchList()
        {
            Batches = new List<Batch>();
            NextBatchId = 1;
        }

        #endregion

        #region Properties
        public int NextBatchId { get; set; }
        public List<Batch> Batches { get; set; }
        #endregion

        #region Methods

        public void Add(Batch newBatch)
        {
            newBatch.Id = NextBatchId++;
            Batches.Add(newBatch);
        }

        public void XmlRead(XmlReader reader)
        {
            reader.ReadStartElement(nameof(BatchList));
            NextBatchId=reader.ReadElementContentAsInt(nameof(NextBatchId),"");
            while (reader.IsStartElement(nameof(Batch)))
            {
                Batches.Add(new Batch());
                Batches.Last().XmlRead(reader);
            }
        }
        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(BatchList));
            writer.WriteElementString(nameof(NextBatchId), NextBatchId.ToString());
            foreach (var batch in Batches)
            {
                batch.XmlWrite(writer);
            }
            writer.WriteEndElement();
        }

        #endregion

    }
}
