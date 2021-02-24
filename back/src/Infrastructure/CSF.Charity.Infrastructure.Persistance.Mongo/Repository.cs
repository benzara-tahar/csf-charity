

namespace CSF.Charity.Infrastructure.Persistance.Mongo
{
    using CSF.Charity.Application.Common.Abstractions;
    using CSF.Charity.Domain.Core.Models;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class MongoRepository<T, TKey> : IRepository<T, TKey>
        where T : IEntity<TKey>
    {

        /// <summary>
        /// The connection string.
        /// </summary>
        public string ConnectionString { get; protected set; }

        /// <summary>
        /// The database name.
        /// </summary>
        public string DatabaseName { get; protected set; }

        /// <summary>
        /// The MongoDbContext
        /// </summary>
        protected IMongoContext MongoContext = null;

        /// <summary>
        /// The constructor taking a connection string and a database name.
        /// </summary>
        /// <param name="connectionString">The connection string of the MongoDb server.</param>
        /// <param name="databaseName">The name of the database against which you want to perform operations.</param>
        protected MongoRepository(string connectionString, string databaseName = null)
        {
            SetupMongoDbContext(connectionString, databaseName);
        }

        /// <summary>
        /// The contructor taking a <see cref="IMongoDbContext"/>.
        /// </summary>
        /// <param name="mongoDbContext">A mongodb context implementing <see cref="IMongoDbContext"/></param>
        protected MongoRepository(IMongoContext mongoDbContext)
        {
            SetupMongoDbContext(mongoDbContext);
        }

        /// <summary>
        /// The contructor taking a <see cref="IMongoDatabase"/>.
        /// </summary>
        /// <param name="mongoDatabase">A mongodb context implementing <see cref="IMongoDatabase"/></param>
        protected MongoRepository(IMongoDatabase mongoDatabase) : this(new MongoDbContext(mongoDatabase))
        {
        }

        protected void SetupMongoDbContext(IMongoContext mongoDbContext)
        {
            MongoContext = MongoContext ?? mongoDbContext;
        }

        protected void SetupMongoDbContext(string connectionString, string databaseName)
        {
            if (databaseName == null)
            {
                var mongoUrl = new MongoUrl(connectionString);
                databaseName = databaseName ?? mongoUrl.DatabaseName;
            }
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            SetupMongoDbContext(new MongoDbContext(connectionString, databaseName));
        }

        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        public IMongoCollection<T> Collection
        {
            get
            {
                return this.MongoContext.GetCollection<T>();
            }
        }

        /// <summary>
        /// Gets the name of the collection
        /// </summary>
        public string CollectionName
        {
            get { return this.Collection.CollectionNamespace.CollectionName; }
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            Expression<Func<T, bool>> defaultPredicate = e => true;
            //var query = Query<T>.Where(predicate ?? defaultPredicate);
            var result = Collection.Find(predicate ?? defaultPredicate).ToList();
            return result;
        }

        public IEnumerable<T> GetAllPaged(int page, int size, Expression<Func<T, bool>> predicate = null)
        {
            Expression<Func<T, bool>> defaultPredicate = e => true;
            //var query = Query<T>.Where(predicate ?? defaultPredicate);
            var result = Collection.Find(predicate ?? defaultPredicate)
                .Skip(page * size)
                .Limit(size);
            return result.ToList();
        }
        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The AuditableEntity T.</returns>
        public virtual T GetById(TKey id)
        {
            //if (typeof(T).IsSubclassOf(typeof(AuditableEntity)))
            //{
            //    return this.GetById(new ObjectId(id as string));
            //}

            return this.Collection.Find<T>(e => e.Id.Equals(id)).FirstOrDefault();
        }

   

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public virtual void Add(T entity)
        {
            MongoContext.AddCommand(() => Collection.InsertOneAsync(entity));
        }

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        public virtual void Add(IEnumerable<T> entities)
        {
            this.Collection.InsertMany(entities);
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public virtual void Update(T entity)
        {
            MongoContext.AddCommand(() => Collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity));

        }

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            MongoContext.AddCommand(() =>
            {
                var batch = new List<Task>();
                foreach (T entity in entities)
                {
                    batch.Add(Collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity));
                }
                return Task.WhenAll(batch);

            });
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public virtual void Delete(TKey id)
        {
            MongoContext.AddCommand(() => Collection.DeleteOneAsync(e => e.Id.Equals(id)));
        }



        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(T entity)
        {
            MongoContext.AddCommand(() => Collection.DeleteOneAsync(e => e.Id.Equals(entity.Id)));
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {

            MongoContext.AddCommand(() => Collection.DeleteManyAsync(predicate));
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        public virtual void DeleteAll()
        {
            MongoContext.AddCommand(() => Collection.DeleteManyAsync(_ => true));
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual long Count()
        {
            return this.Collection.CountDocuments(_ => true);
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.Collection.AsQueryable<T>().Any(predicate);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.Collection.AsQueryable<T>().GetEnumerator();
        }


    }

}