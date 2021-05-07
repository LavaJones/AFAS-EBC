using System;
using System.Collections.Generic;
using System.Data;

namespace Afas.AfComply.Domain
{

    public static class DataRowCollectionExtensionMethods
    {

        /// <summary>
        /// Remove the passed dataRows from the dataTable.
        /// </summary>
        public static void RemoveRows(this DataRowCollection dataRowCollection, IList<DataRow> dataRows)
        {

            foreach(DataRow dataRow in dataRows)
            {
                dataRowCollection.Remove(dataRow);
            }

        }

    }

}
