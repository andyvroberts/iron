using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Services
{
    public static class ReaderFactory
    {
        public static IDataFileReader GetRepository(string repoType)
        {
            IDataFileReader repo;

            switch (repoType.ToUpper())
            {
                case ".CSV":
                    repo = new FlatDataReader();
                    break;
                case ".TXT":
                    repo = new FlatDataReader();
                    break;
                case ".DAT":
                    repo = new FlatDataReader();
                    break;
                default:
                    repo = new FlatDataReader();
                    break;
            }
            return repo;
        }
    }


}
