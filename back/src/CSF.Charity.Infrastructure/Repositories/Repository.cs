

namespace CSF.Charity.Infrastructure.Repositories
{
    using CSF.Charity.Application.Common.Interfaces;
    using CSF.Charity.Domain.Common;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

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
        protected IMongoDbContext MongoDbContext = null;

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
        protected MongoRepository(IMongoDbContext mongoDbContext)
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

        protected void SetupMongoDbContext(IMongoDbContext mongoDbContext)
        {
            MongoDbContext = MongoDbContext ?? mongoDbContext;
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
                return this.MongoDbContext.GetCollection<T>();
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
            if (typeof(T).IsSubclassOf(typeof(AuditableEntity)))
            {
                return this.GetById(new ObjectId(id as string));
            }

            return this.Collection.Find<T>(e => e.Id.Equals(id)).FirstOrDefault();
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The AuditableEntity T.</returns>
        public virtual T GetById(ObjectId id)
        {
            return this.Collection.Find<T>(e => e.Id.Equals(id)).FirstOrDefault();
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public virtual T Add(T entity)
        {
            this.Collection.InsertOne(entity);

            return entity;
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
        public virtual T Update(T entity)
        {
            this.Collection.ReplaceOne(x => x.Id.Equals(entity.Id), entity);

            return entity;
        }

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                this.Collection.ReplaceOne(x => x.Id.Equals(entity.Id), entity);
            }
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public virtual void Delete(TKey id)
        {
            this.Collection.DeleteOne(e => e.Id.Equals(id));
        }

        /// <summary>
        /// Deletes an entity from the repository by its ObjectId.
        /// </summary>
        /// <param name="id">The ObjectId of the entity.</param>
        public virtual void Delete(ObjectId id)
        {
            this.Collection.DeleteOne(e => e.Id.Equals(id));
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(T entity)
        {
            this.Delete(entity.Id);
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            foreach (T entity in this.Collection.AsQueryable<T>().Where(predicate))
            {
                this.Delete(entity.Id);
            }
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        public virtual void DeleteAll()
        {
            this.Collection.DeleteMany(e=>true);
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual long Count()
        {
            return this.Collection.CountDocuments(_=>true);
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
        /// Lets the server know that this thread is about to begin a series of related operations that must all occur
        /// on the same connection. The return value of this method implements IDisposable and can be placed in a using
        /// statement (in which case RequestDone will be called automatically when leaving the using statement). 
        /// </summary>
        /// <returns>A helper object that implements IDisposable and calls RequestDone() from the Dispose method.</returns>
        /// <remarks>
        ///     <para>
        ///         Sometimes a series of operations needs to be performed on the same connection in order to guarantee correct
        ///         results. This is rarely the case, and most of the time there is no need to call RequestStart/RequestDone.
        ///         An example of when this might be necessary is when a series of Inserts are called in rapid succession with
        ///         SafeMode off, and you want to query that data in a consistent manner immediately thereafter (with SafeMode
        ///         off the writes can queue up at the server and might not be immediately visible to other connections). Using
        ///         RequestStart you can force a query to be on the same connection as the writes, so the query won't execute
        ///         until the server has caught up with the writes.
        ///     </para>
        ///     <para>
        ///         A thread can temporarily reserve a connection from the connection pool by using RequestStart and
        ///         RequestDone. You are free to use any other databases as well during the request. RequestStart increments a
        ///         counter (for this thread) and RequestDone decrements the counter. The connection that was reserved is not
        ///         actually returned to the connection pool until the count reaches zero again. This means that calls to
        ///         RequestStart/RequestDone can be nested and the right thing will happen.
        ///     </para>
        ///     <para>
        ///         Use the connectionstring to specify the readpreference; add "readPreference=X" where X is one of the following
        ///         values: primary, primaryPreferred, secondary, secondaryPreferred, nearest.
        ///         See http://docs.mongodb.org/manual/applications/replication/#read-preference
        ///     </para>
        /// </remarks>
        //public virtual IDisposable RequestStart()
        //{
        //    return this.collection.Database.RequestStart();
        //}

        /// <summary>
        /// Lets the server know that this thread is done with a series of related operations.
        /// </summary>
        /// <remarks>
        /// Instead of calling this method it is better to put the return value of RequestStart in a using statement.
        /// </remarks>
        //public virtual void RequestDone()
        //{
        //    this.collection.Database.RequestDone();
        //}

        #region IQueryable<T>
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.Collection.AsQueryable<T>().GetEnumerator();
        }






        #endregion
    }

}