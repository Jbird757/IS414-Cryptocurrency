using System;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace IS414_Blockchain
{
    class Block
    {
        public int Index { get; set; }
        public string PrevHash { get; set; }
        public string Timestamp { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }
        public List<Transaction> Transactions { get; set; }


        public Block(int index, string timestamp, List<Transaction> transactions, string prevHash = "")
        {
            this.Index = index;
            this.Timestamp = timestamp;
            this.Transactions = transactions;
            this.PrevHash = prevHash;
            this.Hash = CalculateHash();
            this.Nonce = 0;
        }

        public string CalculateHash()
        {
            string blockData = this.Index + this.PrevHash + this.Timestamp + this.Transactions.ToString() + this.Nonce;
            byte[] blockBytes = Encoding.ASCII.GetBytes(blockData);
            byte[] hashByte = SHA256.Create().ComputeHash(blockBytes);
            return BitConverter.ToString(hashByte).Replace("-", "");
        }

        public void Mine(int difficulty)
        {
            while (this.Hash.Substring(0, difficulty) != new String('0', difficulty))
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
                //Console.WriteLine("Mining: " + this.Hash);
            }

            Console.WriteLine("Block has been mined: " + this.Hash);
        }
    }
}
