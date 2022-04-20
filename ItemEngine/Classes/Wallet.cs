using Globals.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes
{
    internal class Wallet : ISaveable
    {
        public int Balance { get; private set; }

        public void Deposit(int amt)
        {
            Balance += amt;
        }

        public bool CanAfford(int amt)
        {
            return Balance >= amt;
        }
        /// <summary>
        /// Returns amount able to withdraw. subtracts from balance. 
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public int Withdraw(int amt)
        {
            int amountToReturn = 0;
            if (Balance < amt)
            {
                amountToReturn = Balance;
                Balance = 0;

            }
            else
            {
                Balance -= amt;
                amountToReturn = amt;
            }
            return amountToReturn;
                
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Balance);
        }

        public void LoadSave(BinaryReader reader)
        {
            Balance = reader.ReadInt32();
        }

        public void CleanUp()
        {
            Balance = 0;
        }
    }
}
