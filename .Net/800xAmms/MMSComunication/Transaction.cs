using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MMSComunication
{
    class Transaction : INotifyPropertyChanged
    {
        private readonly string transactionName;
        private string transactionID;
        private readonly Direction direction;
        private ObservableCollection<MMSVariable> mmsVariables;

        public Transaction(string transactioName, Direction direction)
        {
            if (string.IsNullOrEmpty(transactionName))
            {
                throw new ArgumentException("Transaction must have a name");
            }

            this.transactionName = transactionName;
            this.direction = direction;
            this.mmsVariables = new ObservableCollection<MMSVariable>();
        }

        public bool AddTag(MMSVariable mmsVariable)
        {
            if (string.IsNullOrEmpty(mmsVariable.Name))
            {
                throw new ArgumentException("Tag must have a name");
            }

            if (this.mmsVariables.Any(x => x.Name == mmsVariable.Name))
            {
                throw new DuplicateNameException("Tag already exist");
            }

            this.mmsVariables.Add(mmsVariable);
            return true;
        }

        public string ToXML()
        {
            var sXML = string.Empty;

            sXML += "<root>";
            sXML += $"<TransactionId>{this.TransactionID.Trim()}</TransactionId>";
            sXML += $"<TimeStamp>{DateTime.Now.ToLongTimeString()}</TimeStamp>";
            sXML += "<TransactionBody>";
            sXML += this.BodyToXML();
            sXML += "</TransactionBody>";
            sXML += "</root>";

            return sXML;
        }

        public string BodyToXML()
        {
            var xml = string.Empty;

            xml += $"<{this.TransactionName.Trim()}>";
 
            foreach (var property in this.mmsVariables)
            {
                xml += "<" + property.Name.Trim() + ">";
                if (property.Value.Contains("\0"))
                {
                    xml += property.Value.Replace("\0", string.Empty).Trim();
                }
                else
                {
                    xml += property.Value.Trim();
                }

                xml += "</" + property.Name.Trim() + ">";
            }

            xml += "</" + this.TransactionName.Trim() + ">";

            return xml;
        }

        public void UpdateTransactionData(byte[] data)
        {

        }

        public string TransactionName => transactionName;

        public string TransactionID { get => this.transactionID; set => this.transactionID = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
