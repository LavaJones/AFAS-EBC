using Afas.AfComply.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.LegacyData
{
    public partial class employee
    {
        public virtual string SSN 
        {
            get
            {
                return AesEncryption.Decrypt(this.ssn);
            }

            set
            {
                this.ssn = AesEncryption.Encrypt(value.ZeroPadSsn());
            }
        }

        public virtual string SSN_hidden { get { return AesEncryption.Decrypt(this.ssn).Masked_SSN(); } }

    }
}
