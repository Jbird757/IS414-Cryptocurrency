using System;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace IS414_Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            PrivateKey key1 = new PrivateKey();
            PublicKey wallet1 = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey wallet2 = key2.publicKey();

            Blockchain cripto = new Blockchain(2, 100);

            Console.WriteLine("Start the Miner");

            cripto.MinePendingTransactions(wallet1);
            decimal bal1 = cripto.GetBalanceOfWallet(wallet1);

            Console.WriteLine("\nBalance of wallet1 is $" + cripto.GetBalanceOfWallet(wallet1).ToString());

            Transaction tx1 = new Transaction(wallet1, wallet2, 10);
            tx1.SignTransaction(key1);
            cripto.addPendingTransaction(tx1);
            Console.WriteLine("Start the Miner");
            cripto.MinePendingTransactions(wallet2);
            Console.WriteLine("\nBalance of wallet1 is $" + cripto.GetBalanceOfWallet(wallet1).ToString());
            Console.WriteLine("\nBalance of wallet2 is $" + cripto.GetBalanceOfWallet(wallet2).ToString());

            string blockJSON = JsonConvert.SerializeObject(cripto, Formatting.Indented);

            Console.WriteLine(blockJSON);

            //cripto.GetLatestBlock().PrevHash = "12345";

            if (cripto.isChainValid())
            {
                Console.WriteLine("Blockchain is Valid");
            } 
            else
            {
                Console.WriteLine("Blockchain is NOT Valid");
            }
        }
    }

    


}
