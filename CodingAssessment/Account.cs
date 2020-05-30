using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// This project was designed in 4/2020 for a code assessment, based off of a companies requirements. I developed this to be simple, and precice.
/// It's a banking simulation software that includes Withdrawals, Deposits, and Transfers.
/// </summary>
/// Copyright ©2020+ Tim Carter (https://github.com/Cynikal3) -- If you wish to use any portion of this code for commercial use, please contact me for permission.
/// The code within this file, and any other files is meant for Educational Use only.
namespace CodingAssessment
{
    class Account
    {
        public enum AccountType_enum
        {
            Savings,
            Checking
        }

        public enum SavingsType_enum
        {
            Basic,
            Advanced
        }

        public AccountType_enum AccountType;
        public SavingsType_enum SavingsType; //Only used if AccountType = Savings

        public int AccountId = 0;
        public int OwnerId = 0;
        public decimal Balance = 0.00m; //Decimal used for accurate balances in the USD notation.

        public List<string> AccountLog = new List<string>(); //Any logging used for the account in question.
    }
}
