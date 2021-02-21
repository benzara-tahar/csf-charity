using CSF.Charity.Domain.Common;
using CSF.Charity.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSF.Charity.Infrastructure.Persistence.Mappings
{
    public static class MongoDbClassMappings
    {

        public static void InitializeGuidRepresentation()
        {
            // by default, avoid legacy UUID representation: use Binary 0x04 subtype.
            //MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            //BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterIdGenerator(typeof(Guid), CombGuidGenerator.Instance);
        }
        public static void MapDomainEntities()
        {
            BsonClassMap.RegisterClassMap<AuditableEntity<int>>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });
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
