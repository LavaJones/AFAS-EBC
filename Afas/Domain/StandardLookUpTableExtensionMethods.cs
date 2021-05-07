using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Domain
{
    public static class StandardLookUpTableExtensionMethods
    {

        private static ILog Log = LogManager.GetLogger(typeof(StandardLookUpTableExtensionMethods));

        /// <summary>
        /// Creates a Dictionary to be used as a Lookup Table.
        /// </summary>
        /// <typeparam name="K">The object type that is used as the key.</typeparam>
        /// <typeparam name="T">The type that is being made lookupable.</typeparam>
        /// <param name="data">The Data that needs a Look Up Table built for it.</param>
        /// <param name="mapping">The Key selector that returns the key for each Item in Data (Key must be distinct).</param>
        /// <returns>The lookup Dictionary for hte Data indexed by the Key selector provided.</returns>
        public static Dictionary<K, T> CreateLookup<K, T>(this IEnumerable<T> data, Func<T, K> mapping)
        {

            // build the empty dictionary
            Dictionary<K, T> lookup = new Dictionary<K, T>();

            // Add each item one at a time 
            foreach (T item in data)
            {
                // Use the Key Selector to get the Key for this item
                K key = mapping(item);

                // Check that the key has not already been added and throw an exception if it has
                if (lookup.ContainsKey(key))
                {
                    throw new InvalidOperationException("The key Property provided to CreateLookup must be distinct, multiple items found with key: " + key);
                }

                // Add the item and key to the table.
                lookup.Add(key, item);

            }

            // return the results back for use
            return lookup;
        }

    }

}
