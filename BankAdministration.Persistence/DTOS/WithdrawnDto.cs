using System;
using System.Collections.Generic;
using System.Text;

namespace BankAdministration.Persistence.DTOS
{
    public class WithdrawnDto
    {
        public Int64 WithdrawnAmount { get; set; }

        public string Number { get; set; }
    }
}
