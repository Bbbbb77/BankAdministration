using System;
using System.Collections.Generic;
using System.Text;

namespace BankAdministration.Persistence.DTOS
{
    public class TransferDto
    {
        public Int64 TransferAmount { get; set; }

        public string SourceNumber { get; set; }

        public string DestNumber { get; set; }

        public string DestUserName { get; set; }
    }
}
