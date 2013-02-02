// Copyright 2012 Mike Caldwell (Casascius)
// This file is part of Bitcoin Address Utility.

// Bitcoin Address Utility is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Bitcoin Address Utility is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Bitcoin Address Utility.  If not, see http://www.gnu.org/licenses/.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Casascius.Bitcoin {
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
