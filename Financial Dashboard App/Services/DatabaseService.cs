﻿using DocumentFormat.OpenXml.Wordprocessing;
using Financial_Dashboard_App.DbContexts;
using Financial_Dashboard_App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Dashboard_App.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly AppDbContextFactory dbContextFactory;

        public DatabaseService(AppDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            using(AppDbContext context = dbContextFactory.CreateDbContext())
            {
                return await context.Transactions.ToListAsync();
            }
        }
        
        public async Task CreateTransaction(Transaction transaction)
        {
            using(AppDbContext context = dbContextFactory.CreateDbContext())
            {
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            using(AppDbContext context = dbContextFactory.CreateDbContext())
            {
                var updatedTransaction = await context.Transactions.FindAsync(transaction.Id);
                if(updatedTransaction != null)
                {
                    updatedTransaction.Date = transaction.Date;
                    updatedTransaction.Description = transaction.Description;
                    updatedTransaction.Amount = transaction.Amount;
                    updatedTransaction.Type = transaction.Type;

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteTransaction(Transaction transaction)
        {
            using(AppDbContext context = dbContextFactory.CreateDbContext())
            {
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
            }
        }
    }
}
