using System;
using System.Collections.Generic;
using System.Text;

namespace BankAdministration.Persistence.DTOS
{
    public class DepositDto
    {
        public Int64 DepositAmount { get; set; }

        public string Number { get; set; }
    }
}
