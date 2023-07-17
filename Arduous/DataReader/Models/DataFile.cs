using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Models
{
    public class DataFile
    {
        private Queue<List<string>> sampleSet;
        public Queue<List<string>> SampleSet
        {
            get { return sampleSet; }
            set { sampleSet = value; }
        }

        private int sampleSize;
        public int SampleSize
        {
            get { return sampleSize; }
            set { sampleSize = value; }
        }

        private bool isSeperated;
        public bool IsSeperated
        {
            get { return isSeperated; }
            set { isSeperated = value; }
        }

        private string seperatorValue;
        public string SeperatorValue
        {
            get { return seperatorValue; }
            set { seperatorValue = value; }
        }

        private bool isQuoted;
        public bool IsQuoted
        {
            get { return isQuoted; }
            set { isQuoted = value; }
        }


        /// <summary>
        /// Constructor applies mandatory initialisations.
        /// </summary>
        public DataFile()
        {
            sampleSet = new Queue<List<string>>();
            sampleSize = 0;
            isSeperated = false;
            seperatorValue = string.Empty;
            isQuoted = false;
        }
    }
}
