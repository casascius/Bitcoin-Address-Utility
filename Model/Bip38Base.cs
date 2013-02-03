using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Casascius.Bitcoin {

    /// <summary>
    /// Base class for Bip38Confirmation and Bip38Intermediate
    /// </summary>
    public class Bip38Base {

        /// <summary>
        /// If a lot number is defined, this returns it.  If not, returns -1
        /// </summary>
        public int LotNumber {
            get {
                if (LotSequencePresent == false) return -1;
                return _ownerentropy[4] * 4096 + _ownerentropy[5] * 16 + _ownerentropy[6] / 16;
            }
        }

        /// <summary>
        /// If a sequence number is defined, this returns it.  If not, returns -1
        /// </summary>
        public int SequenceNumber {
            get {
                if (LotSequencePresent == false) return -1;
                return (_ownerentropy[6] & 0x0f) * 256 + _ownerentropy[7];
            }
        }

        protected byte[] _ownerentropy;
        public byte[] ownerentropy {
            get {

                return Util.CloneByteArray(_ownerentropy);
            }
        }

        /// <summary>
        /// If true, ownerentropy contains values for LotNumber and SequenceNumber.
        /// </summary>
        public virtual bool LotSequencePresent {
            get {
                return false;
            }            
        }

        public byte[] ownersalt {
            get {
                return Util.CloneByteArray(_ownerentropy, 0, LotSequencePresent ? 4 : 8);
            }
        }


    }
}
