using DevStore.Core.Data.EventSourcing;
using DevStore.Core.Messages;
using KurrentDB.Client;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace EventSourcing;

public class EventSourcingRepository : IEventSourcingRepository
{
    private readonly KurrentDBClient _client;

    public EventSourcingRepository(IConfiguration configuration)
    {
        var settings = KurrentDBClientSettings.Create(configuration.GetConnectionString("")!);
        _client = new KurrentDBClient(settings);
    }

    public async Task SalvarEvento<TEvent>(TEvent evento) where TEvent : Event
    {
        await _client.AppendToStreamAsync(
            evento.AggregateId.ToString(),
            StreamState.Any,
            FormatarEvento(evento));
    }

    public async Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId)
    {
        var eventos = _client.ReadStreamAsync(
            Direction.Forwards,
            aggregateId.ToString(),
            StreamPosition.Start,
            maxCount: 500,
            false);

        var listaEventos = new List<StoredEvent>();

        await foreach (var resolvedEvent in eventos)
        {
            var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span);
            var eventData = JsonSerializer.Deserialize<BaseEvent>(resolvedEvent.Event.Data.Span);

            var evento = new StoredEvent(
                resolvedEvent.Event.EventId.ToGuid(),
                resolvedEvent.Event.EventType,
                eventData.Timestamp,
                dataEncoded);

            listaEventos.Add(evento);
        }

        return listaEventos.OrderBy(x => x.DataOcorrencia);
    }

    private static IEnumerable<EventData> FormatarEvento<TEvent>(TEvent evento) where TEvent : Event
    {
        yield return new EventData(
            Uuid.NewUuid(),
            evento.MessageType,
            JsonSerializer.SerializeToUtf8Bytes(evento),
            Array.Empty<byte>());
    }
}

internal class BaseEvent
{
    public DateTime Timestamp { get; set; }
}


