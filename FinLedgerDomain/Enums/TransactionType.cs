using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLedgerDomain.Enums;

public enum TransactionType
{
    Deposit,
    Withdrawal,
    Transfer,
    InstallmentPayment,
    LoanDisbursement
}
