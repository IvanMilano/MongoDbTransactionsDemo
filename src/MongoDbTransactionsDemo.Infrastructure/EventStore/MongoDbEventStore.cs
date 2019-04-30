using CQRSlite.Events;

using MongoDB.Driver;

using MongoDbTransactionsDemo.Infrastructure.Contracts;
using MongoDbTransactionsDemo.Infrastructure.DataAccess;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Infrastructure.EventStore
{
    public class MongoDbEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;

        private readonly IMongoDbContext _mongoDbContext;

        private IMongoCollection<EventData> _events;

        private const string EventsCollection = "events";

        public MongoDbEventStore(IEventPublisher publisher, IMongoDbContext mongoDbContext)
        {
            _publisher = publisher;
            _mongoDbContext = mongoDbContext;
            _events = _mongoDbContext.Database.GetCollection<EventData>(EventsCollection);
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default)
        {
            var filterBuilder = Builders<EventData>.Filter;
            var filter = filterBuilder.Eq(EventData.StreamIdFieldName, aggregateId) &
                        filterBuilder.Gte(EventData.VersionFieldName, fromVersion);

            var result = await _events.FindAsync(filter, cancellationToken: cancellationToken);
            return (await result.ToListAsync(cancellationToken)).Select(x => x.PayLoad);
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {
            if (!events.Any())
            {
                return;
            }
            using (var session = await _mongoDbContext.StartSession(cancellationToken))
            {
                var transactionOptions = new TransactionOptions(ReadConcern.Snapshot, ReadPreference.Primary, WriteConcern.WMajority);
                session.StartTransaction(transactionOptions);
                try
                {
                    var bulkOps = new List<WriteModel<EventData>>();
                    foreach (var @event in events)
                    { 
                        var eventData = new EventData
                        {
                            Id = Guid.NewGuid(),
                            StreamId = @event.Id,
                            TimeStamp = @event.TimeStamp,
                            AssemblyQualifiedName = @event.GetType().AssemblyQualifiedName,
                            PayLoad = @event
                        };
                        bulkOps.Add(new InsertOneModel<EventData>(eventData));
                        await _publisher.Publish(@event, cancellationToken);
                    }
                    await _events.BulkWriteAsync(session, bulkOps).ConfigureAwait(false);

                    await session.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception exp)
                {
                    session.AbortTransaction(cancellationToken);
                    throw exp;
                }
            }
        }
    }
}
