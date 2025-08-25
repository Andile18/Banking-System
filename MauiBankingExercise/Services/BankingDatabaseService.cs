using MauiBankingExercise.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MauiBankingExercise.Services
    {
        public class BankingDatabaseService
        {
            private readonly SQLiteConnection _db;
            private static string DbFile = "BankingApp.db";

            public BankingDatabaseService()
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbFile);

                if (!File.Exists(dbPath))
                    File.Create(dbPath).Dispose();

                _db = new SQLiteConnection(dbPath);

                if (_db.Table<Customer>().Count() == 0)
                    BankingSeeder.Seed(_db);
            }

            public List<Customer> GetAllCustomers() => _db.Table<Customer   >().ToList();
            public Customer GetCustomer(int id) => _db.Table<Customer>().FirstOrDefault(c => c.CustomerId == id);

            public List<Account> GetCustomerAccounts(int customerId) =>
                _db.Table<Account>().Where(a => a.CustomerId == customerId).ToList();

            public List<Transaction> GetAccountTransactions(int accountId, int limit = 10) =>
                _db.Table<Transaction>()
                   .Where(t => t.AccountId == accountId)
                   .OrderByDescending(t => t.TransactionDate)
                   .Take(limit)
                   .ToList();

            public AccountType GetAccountType(int id) =>
                _db.Table<AccountType>().FirstOrDefault(t => t.AccountTypeId == id);

            public TransactionType GetTransactionType(int id) =>
                _db.Table<TransactionType>().FirstOrDefault(t => t.TransactionTypeId == id);

            public string GetTransactionTypeName(int id) =>
                GetTransactionType(id)?.Name ?? "Unknown";

            public void AddTransaction(int accountId, decimal amount, int typeId)
            {
                var account = _db.Table<Account>().FirstOrDefault(a => a.AccountId == accountId);
                if (account == null) throw new Exception("Account not found");
                if (typeId == 2 && account.AccountBalance < amount)
                    throw new Exception("Insufficient balance");

                if (typeId == 1) account.AccountBalance += amount;
                if (typeId == 2) account.AccountBalance -= amount;

                _db.Update(account);

                var tx = new Transaction
                {
                    AccountId = accountId,
                    Amount = amount,
                    TransactionTypeId = typeId,
                    TransactionDate = DateTime.Now
                };

                _db.Insert(tx);
            }
        }
    }

