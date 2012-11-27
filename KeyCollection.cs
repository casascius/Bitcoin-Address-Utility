using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BtcAddress {
    public class KeyCollection {

        public List<KeyCollectionItem> Items = new List<KeyCollectionItem>();

        public event Action<KeyCollectionItem> ItemAdded;
        public event Action<IEnumerable<KeyCollectionItem>> ItemsAdded;
        public event Action<IEnumerable<KeyCollectionItem>> ItemsDeleted;


        public void AddItem(KeyCollectionItem item) {
            Items.Add(item);
            if (ItemAdded != null) ItemAdded.Invoke(item);
        }

        public void AddItemRange(IEnumerable<KeyCollectionItem> items) {
            foreach (var item in items) {
                Items.Add(item);
            }
            if (ItemsAdded != null) ItemsAdded.Invoke(items);
        }

        public void DeleteItemRange(IEnumerable<KeyCollectionItem> items) {
            foreach (var item in items) {
                Items.Remove(item);
            }
            if (ItemsDeleted != null) ItemsDeleted.Invoke(items);
        }
        


    }
}
