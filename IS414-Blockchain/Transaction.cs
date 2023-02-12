using System;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace IS414_Blockchain
{
    class Transaction
    {
        public PublicKey FromAddress { get; set; }
        public PublicKey ToAddress { get; set; }
        public decimal Amount { get; set; }
        public Signature signature { get; set; }

        public Transaction(PublicKey from, PublicKey to, decimal amount)
        {
            this.FromAddress = from;
            this.ToAddress = to;
            this.Amount = amount;
        }

        public void SignTransaction(PrivateKey signKey)
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string signingDER = BitConverter.ToString(signKey.publicKey().toDer()).Replace("-", "");

            if (fromAddressDER != signingDER)
            {
                throw new Exception("You cannot sign transactions for other wallet");
            }

            string txHash = this.CalculateHash();
            this.signature = Ecdsa.sign(txHash, signKey);
        }

        public string CalculateHash()
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string toAddressDER = BitConverter.ToString(ToAddress.toDer()).Replace("-", "");
            string transactionData = fromAddressDER + toAddressDER + Amount;
            byte[] tdBytes = Encoding.ASCII.GetBytes(transactionData);
            return BitConverter.ToString(SHA256.Create().ComputeHash(tdBytes)).Replace("-", "");
        }

        public bool isValid()
        {
            if (this.FromAddress is null) return true;

            if (this.signature is null)
            {
                throw new Exception("No signature in this transaction");
            }

            return Ecdsa.verify(this.CalculateHash(), this.signature, this.FromAddress);
        }
    }
}
