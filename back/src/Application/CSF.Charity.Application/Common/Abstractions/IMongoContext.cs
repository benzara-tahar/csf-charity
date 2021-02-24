using CSF.Charity.Domain.Core.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Common.Abstractions
{
    public interface IMongoContext : IDisposable
    {
        /// <summary>
        /// add a command to the current UOW transaction
        /// </summary>
        /// <param name="func"></param>
        void AddCommand(Func<Task> func);

        /// <summary>
        /// commit the transaction
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChanges();
        IEnumerable<DomainEvent> GetDomainEvents();


        /// <summary>
        /// The IMongoClient from the official MongoDb driver
        /// </summary>
        IMongoClient Client { get; }

        /// <summary>
        /// The IMongoDatabase from the official Mongodb driver
        /// </summary>
        IMongoDatabase Database { get; }

        /// <summary>
        /// Returns a collection for a document type that has a partition key.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        IMongoCollection<TDocument> GetCollection<TDocument>();

        /// <summary>
        /// Drops a collection having a partitionkey, use very carefully.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        void DropCollection<TDocument>();

    }
}

