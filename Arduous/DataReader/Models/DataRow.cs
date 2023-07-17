using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Models
{
    public class DataRow
    {
        private List<string> columns;
        public List<string> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        private List<Tuple<char, CharType>> characters;
        public List<Tuple<char, CharType>> Characters
        {
            get { return characters; }
            set { characters = value; }
        }


        /// <summary>
        /// Constructor performs mandatory initialisations.
        /// </summary>
        public DataRow()
        {
            columns = new List<string>();
            characters = new List<Tuple<char, CharType>>();
        }

    }
}
