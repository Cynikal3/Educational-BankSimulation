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
    static class AccountTransactions
    {
        static decimal transferFee = 5.00m; //The amount for the transfer fee.
        static Account MasterAccount = Program.accounts.Where(i => i.AccountId == 1).FirstOrDefault();


        public static bool IsAccountAdvChecking(int AccountId)
        {
            Account acct = Program.accounts.Where(i => i.AccountId == AccountId).FirstOrDefault();
            if (acct != null)
            {
                if (acct.AccountType == Account.AccountType_enum.Checking && acct.SavingsType == Account.SavingsType_enum.Advanced) return true; else return false;
            }
            else
            {
                return false;
            }
        }

        public static bool Deposit(int ToAcct, decimal Amount)
        {
            Account acct = Program.accounts.Where(i => i.AccountId == ToAcct).FirstOrDefault();

            if (acct == null) return false; //Account not found.

            acct.Balance += Amount;
            acct.AccountLog.Add($"Deposit of {Amount:C} on {DateTime.Now.ToShortDateString()}");
            return true; //Operation complete.
        }

        public static bool Withdraw(int FromAcct, decimal Amount)
        {
            Account acct = Program.accounts.Where(i => i.AccountId == FromAcct).FirstOrDefault();
            if (acct == null) return false; //Account not found.

            if (acct.Balance < Amount) return false; //Not enough funds.


            //Do fancy ATM logic here. And verify if the funds were actually deposited... THEN remove the balance.
            acct.Balance -= Amount;
            acct.AccountLog.Add($"Withdrew {Amount:C} on {DateTime.Now.ToShortDateString()}");

            return true; //Operation complete.
        }

        public static bool Transfer(int FromAcct, int ToAcct, decimal amount)
        {

            if (Program.accounts != null)
            {
                Account fromAcct = Program.accounts.Where(i => i.AccountId == FromAcct).FirstOrDefault();
                Account toAcct = Program.accounts.Where(i => i.AccountId == ToAcct).FirstOrDefault();

                if (fromAcct != null && toAcct != null)
                {
                    //Optional: fromAcct.OwnerId == toAcct.OwnerId. Left out, just incase you wanted to deposit to an account not owned by you.


                    //If the FROM Account is Adv. Checking, No fee. If it's not Adv. Checking & The Fee is over $100, add the fee.
                    if (fromAcct.Balance >= (amount > 100m ? (IsAccountAdvChecking(fromAcct.AccountId) ? amount : amount + transferFee) : amount))
                    {
                        //We have enough funds. We can transfer.
                        fromAcct.Balance -= amount;
                        toAcct.Balance += amount;

                        fromAcct.AccountLog.Add($"Transferred {amount:C} to account {toAcct.AccountId}");
                        fromAcct.AccountLog.Add($"Paid a transfer fee of {transferFee:C} for transferring over $100. " +
                            $"Consider upgrading to our Advanced Checking to avoid this fee in the future!");

                        toAcct.AccountLog.Add($"Received a transfer of {amount:C} from account {fromAcct.AccountId}");

                        if (amount > 100m && !IsAccountAdvChecking(fromAcct.AccountId))
                        {
                            MasterAccount.Balance += transferFee;
                            MasterAccount.AccountLog.Add($"Received a transfer fee of {transferFee:C} from account {fromAcct.AccountId}");
                        }

                        return true; //Operation complete.

                    }
                    else { return false; }

                }
                else
                {
                    return false; //Transaction didn't complete.
                }

            }
            else { return false; }
        }
    }
}
