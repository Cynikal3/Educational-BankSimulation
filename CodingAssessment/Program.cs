using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


/// <summary>
/// This project was designed in 4/2020 for a code assessment, based off of a companies requirements. I developed this to be simple, and precice.
/// It's a banking simulation software that includes Withdrawals, Deposits, and Transfers.
/// </summary>
/// Copyright ©2020+ Tim Carter (https://github.com/Cynikal3) -- If you wish to use any portion of this code for commercial use, please contact me for permission.
/// The code within this file, and any other files is meant for Educational Use only.

namespace CodingAssessment
{
    class Program
    {

        public static List<Account> accounts = new List<Account>();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            int TestAccountsNUM = 20;
            Console.WriteLine($"Loading {TestAccountsNUM} test accounts");
            initialSetup(TestAccountsNUM);

            Console.Clear(); //Used to clear out the "Loading" message.
            Console.ForegroundColor = ConsoleColor.White;


            foreach (var i in accounts)
            {
                Console.WriteLine($"ID: {i.AccountId} | Type: {i.AccountType}{(i.AccountType == Account.AccountType_enum.Savings ? $" ({i.SavingsType})" : null)}" +
                    $" | Balance: {i.Balance:C}");
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Push key to continue with a few random tests.");
            
            Console.ReadKey(); //Used to keep the software open.

            Console.ForegroundColor = ConsoleColor.White;
            foreach (var i in accounts)
            {
                if (i.AccountId == 1) continue; //We don't want to touch the master account.

                Random random = new Random();
                var rnd = random.Next(1,4); //4 for the number of operations (Withdraw, Deposit, Transfer) (The upper limit is never hit, simply, this is: (1 <= x < 4)

                var ammt = random.Next(1, 10000); //Used for the test cases.
                var oldBalance = i.Balance;
                switch (rnd)
                {
                    case 1: //Deposit
                        if (AccountTransactions.Deposit(i.AccountId,ammt))
                        {
                            Console.WriteLine($"Successfully deposited {ammt:C} into {i.AccountId}'s account. | Old Balance: {oldBalance:C} / New Balance: {i.Balance:C}");
                        } else
                        {
                            Console.WriteLine($"Deposit failed for account {i.AccountId}");
                        }
                        break;


                    case 2: //Withdraw
                        if (AccountTransactions.Withdraw(i.AccountId, ammt))
                        {
                            Console.WriteLine($"Successfully withdrew {ammt:C} from {i.AccountId}'s account. | Old Balance: {oldBalance:C} / New Balance: {i.Balance:C}");
                        } else
                        {
                            Console.WriteLine($"Withdraw failed for account {i.AccountId}");
                        }
                        break;


                    case 3: //Transfer
                        //We need another randomized account for this to work.
                        Account secAcct;
                        do
                        {
                            secAcct = accounts[random.Next(accounts.Count)];
                        } while (secAcct.AccountId == i.AccountId); //This makes sure we're not transfering to our own account.
                        var secAcctOldBalance = secAcct.Balance;

                        if (AccountTransactions.Transfer(i.AccountId, secAcct.AccountId, ammt))
                        {
                            Console.WriteLine($"Successfully transfered {ammt:C} from {i.AccountId}'s account to {secAcct.AccountId}'s account. | From old Balance: {oldBalance:C} / From new Balance: {i.Balance} / To old Balance: {secAcctOldBalance:C} / To new Balance: {secAcct.Balance:C}");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to transfer {ammt:C} from {i.AccountId} to {secAcct.AccountId}");
                        }
                        break;

                    default:
                        Console.WriteLine($"UNKNOWN: {rnd}");
                        break;
                }

                Thread.Sleep(100); //Adding to allow the RNG to get a new number.
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Tests completed. Please a breakpoint here, and inspect the accounts to see the user logs. (Accounts on line 14 | [0] is the Master Account)");
            Console.WriteLine("Push any key to display the accounts again, with their updated balances. (Acct Id: 1 = Master)");
            Console.ReadKey();

            Console.ForegroundColor = ConsoleColor.White;
            foreach (var i in accounts)
            {
                Console.WriteLine($"ID: {i.AccountId} | Type: {i.AccountType}{(i.AccountType == Account.AccountType_enum.Savings ? $" ({i.SavingsType})" : null)}" +
                    $" | Balance: {i.Balance:C}");
                
                Thread.Sleep(100); //Added for a nice scrolling effect.
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("All has completed. Push any key to exit the application.");
            Console.ReadKey();



        }

        /// <summary>
        /// Sets up X accounts with randomized test data.
        /// </summary>
        /// <param name="HowManyAccounts">The amount of accounts you'd like to setup for testing.</param>
        private static void initialSetup(int HowManyAccounts)
        {

            #region Master Bank Account
            Account master = new Account //This is where all of our banks money will go.
            {
                AccountId = 1,
                Balance = 0.00m,
                OwnerId = 0,
                AccountType = Account.AccountType_enum.Checking
            };
            accounts.Add(master);
            #endregion

            #region Setting Up Fake Accounts
            for (int i = 0; i < HowManyAccounts; i++)
            {

                Array values = Enum.GetValues(typeof(Account.AccountType_enum));
                Random random = new Random();
                Account.AccountType_enum randomAcctType = (Account.AccountType_enum)values.GetValue(random.Next(values.Length));

                Account tmp = new Account
                {
                    AccountId = random.Next(100000, 999999),
                    Balance = (decimal)random.Next(10000),
                    OwnerId = random.Next(),
                    AccountType = randomAcctType
                };

                if (tmp.AccountType == Account.AccountType_enum.Savings)
                {
                    values = Enum.GetValues(typeof(Account.SavingsType_enum));
                    tmp.SavingsType = (Account.SavingsType_enum)values.GetValue(random.Next(values.Length));
                }

                accounts.Add(tmp);
                Thread.Sleep(100); //When this is disabled, all 'test' cases have the same data.
            }
            #endregion
        }
    }
}
