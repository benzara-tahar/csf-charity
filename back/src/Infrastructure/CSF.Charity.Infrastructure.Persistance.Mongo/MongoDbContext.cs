using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Domain.Core.Models;
using CSF.Charity.Infrastructure.Persistence.Mongo.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CSF.Charity.Infrastructure.Persistance.Mongo
{


    /// <summary>
    /// The MongoDb context
    /// </summary>
    public class MongoDbContext : IMongoContext
    {
        // Every command will be stored and it'll be processed at SaveChanges
        private readonly List<Func<Task>> _commands = new List<Func<Task>>();

        public IClientSessionHandle Session { get; set; }

        /// <summary>
        /// The IMongoClient from the official MongoDB driver
        /// </summary>
        public IMongoClient Client { get; }

        /// <summary>
        /// The IMongoDatabase from the official MongoDB driver
        /// </summary>
        public IMongoDatabase Database { get; }


        /// <summary>
        /// The constructor of the MongoDbContext, it needs an object implementing <see cref="IMongoDatabase"/>.
        /// </summary>
        /// <param name="mongoDatabase">An object implementing IMongoDatabase</param>
        public MongoDbContext(IMongoDatabase mongoDatabase)
        {
            Database = mongoDatabase;
            Client = Database.Client;
        }

        /// <summary>
        /// The constructor of the MongoDbContext, it needs a connection string and a database name. 
        /// </summary>
        /// <param name="connectionString">The connections string.</param>
        /// <param name="databaseName">The name of your database.</param>
        public MongoDbContext(string connectionString, string databaseName)
        {
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Initialise an instance of a <see cref="IMongoDbContext"/> using a connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public MongoDbContext(string connectionString)
            : this(connectionString, new MongoUrl(connectionString).DatabaseName)
        {
        }

        /// <summary>
        /// The constructor of the MongoDbContext, it needs a connection string and a database name. 
        /// </summary>
        /// <param name="client">The MongoClient.</param>
        /// <param name="databaseName">The name of your database.</param>
        public MongoDbContext(MongoClient client, string databaseName)
        {

            Client = client;
            Database = client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Returns a collection for a document type. Also handles document types with a partition key.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="partitionKey">The optional value of the partition key.</param>
        public virtual IMongoCollection<TDocument> GetCollection<TDocument>()
        {
            return Database.GetCollection<TDocument>(GetCollectionName<TDocument>());
        }

        /// <summary>
        /// Drops a collection, use very carefully.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        public virtual void DropCollection<TDocument>()
        {
            Database.DropCollection(GetCollectionName<TDocument>());
        }


        /// <summary>
        /// Extracts the CollectionName attribute from the entity type, if any.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <returns>The name of the collection in which the TDocument is stored.</returns>
        protected virtual string GetAttributeCollectionName<TDocument>()
        {
            return (typeof(TDocument).GetTypeInfo()
                                     .GetCustomAttributes(typeof(CollectionNameAttribute))
                                     .FirstOrDefault() as CollectionNameAttribute)?.Name;
        }

        /// <summary>
        /// Initialize the Guid representation of the MongoDB Driver.
        /// Override this method to change the default GuidRepresentation.
        /// </summary>


        /// <summary>
        /// Given the document type and the partition key, returns the name of the collection it belongs to.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="partitionKey">The value of the partition key.</param>
        /// <returns>The name of the collection.</returns>
        protected virtual string GetCollectionName<TDocument>()
        {
            var collectionName = GetAttributeCollectionName<TDocument>() ?? Pluralize<TDocument>();

            return $"{collectionName}";
        }

        /// <summary>
        /// Very naively pluralizes a TDocument type name.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <returns>The pluralized document name.</returns>
        protected virtual string Pluralize<TDocument>()
        {
            return (typeof(TDocument).Name.Pluralize()).Camelize();
        }

        /// <summary>
        /// add a command to the transaction
        /// </summary>
        /// <param name="func"></param>
        public void AddCommand(System.Func<Task> func)
        {
            _commands.Add(func);
        }

        /// <summary>
        /// commit transaction
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChanges()
        {
            // see https://stackoverflow.com/questions/62349032/requirements-for-using-mongodb-transactions
            //using (Session = await Client.StartSessionAsync())
            //{
            //    Session.StartTransaction();

            // TODO! use lock
                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);
                _commands.Clear();

            //    await Session.CommitTransactionAsync();
            //}

            return _commands.Count;
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<DomainEvent> GetDomainEvents()
        {
            throw new NotImplementedException();
        }
    }

}