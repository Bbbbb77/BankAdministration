using System;
using System.Collections.Generic;
using System.Text;

namespace BankAdministration.Persistence.DTOS
{
    public class LockDto
    {
        public String Number { get; set; }

        public Boolean IsLocked { get; set; }
	}
}
