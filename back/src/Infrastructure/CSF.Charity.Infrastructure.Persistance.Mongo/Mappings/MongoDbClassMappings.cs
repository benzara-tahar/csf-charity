using CSF.Charity.Domain.Core.Models;
using CSF.Charity.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace CSF.Charity.Infrastructure.Persistence.Mongo.Mappings
{
    public static class MongoDbClassMappings
    {




        /// <summary>
        /// Sets the Guid representation of the MongoDB Driver.
        /// </summary>
        /// <param name="guidRepresentation">The new value of the GuidRepresentation</param>
        public static void Configure()
        {

            //BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            BsonSerializer.RegisterIdGenerator(typeof(Guid), CombGuidGenerator.Instance);
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("pack",pack, _ => true);
        }
        public static void MapDomainEntities()
        {

            //BsonClassMap.RegisterClassMap<TodoItem>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.MapIdMember(c => c.Id);
            //});
            //BsonClassMap.RegisterClassMap<Entity<int>>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.MapIdMember(c => c.Id);
            //});
            BsonClassMap.RegisterClassMap<AuditableEntity<Guid>>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });
            BsonClassMap.RegisterClassMap<TodoItem>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.DomainEvents);
            });


        }
    }
}
