using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace PokerShark.Interfaces.PyPoker
{
    internal partial class GameInfoMessage
    {
        [JsonProperty("player_num", Required = Required.Always)]
        public int PlayerNum { get; set; }

        [JsonProperty("rule", Required = Required.Always)]
        public Rule Rule { get; set; }

        [JsonProperty("seats", Required = Required.Always)]
        public Seat[] Seats { get; set; }
    }

    internal partial class Rule
    {
        [JsonProperty("ante", Required = Required.Always)]
        public double Ante { get; set; }

        [JsonProperty("blind_structure", Required = Required.Always)]
        public BlindStructure BlindStructure { get; set; }

        [JsonProperty("max_round", Required = Required.Always)]
        public int MaxRound { get; set; }

        [JsonProperty("initial_stack", Required = Required.Always)]
        public double InitialStack { get; set; }

        [JsonProperty("small_blind_amount", Required = Required.Always)]
        public double SmallBlindAmount { get; set; }
    }

    internal partial class BlindStructure
    {
    }

    internal partial class Seat
    {
        [JsonProperty("stack", Required = Required.Always)]
        public double Stack { get; set; }

        [JsonProperty("state", Required = Required.Always)]
        public string State { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("uuid", Required = Required.Always)]
        public string Uuid { get; set; }
    }

    internal partial class RoundStartMessage
    {
        [JsonProperty("round_count", Required = Required.Always)]
        public int RoundCount { get; set; }

        [JsonProperty("hole_card", Required = Required.Always)]
        public string[] HoleCard { get; set; }

        [JsonProperty("seats", Required = Required.Always)]
        public Seat[] Seats { get; set; }
    }


    internal partial class RoundStateMessage
    {
        [JsonProperty("round_state", Required = Required.Always)]
        public RoundState RoundState { get; set; }
    }

    internal partial class RoundState
    {
        [JsonProperty("round_count", Required = Required.Always)]
        public int RoundCount { get; set; }

        [JsonProperty("dealer_btn", Required = Required.Always)]
        public int DealerBtn { get; set; }

        [JsonProperty("small_blind_pos", Required = Required.Always)]
        public int SmallBlindPos { get; set; }

        [JsonProperty("big_blind_pos", Required = Required.Always)]
        public int BigBlindPos { get; set; }


        [JsonProperty("small_blind_amount", Required = Required.Always)]
        public double SmallBlindAmount { get; set; }

        [JsonProperty("street", Required = Required.Always)]
        public string Street { get; set; }

        [JsonProperty("formated_community_card", Required = Required.Always)]
        public string[] CommunityCard { get; set; }

        [JsonProperty("seats", Required = Required.Always)]
        public Seat[] Seats { get; set; }

        [JsonProperty("pot", Required = Required.Always)]
        public Pot Pot { get; set; }

        [JsonProperty("action_histories", Required = Required.Always)]
        public ActionHistories ActionHistories { get; set; }
    }

    internal partial class ActionHistories
    {

    }

    internal partial class Pot
    {
        [JsonProperty("main", Required = Required.Always)]
        public Main Main { get; set; }

        [JsonProperty("side", Required = Required.Always)]
        public Side[] Side { get; set; }
    }

    internal partial class Main
    {
        [JsonProperty("amount", Required = Required.Always)]
        public double Amount { get; set; }
    }

    internal partial class Side
    {
        [JsonProperty("amount", Required = Required.Always)]
        public double Amount { get; set; }

        [JsonProperty("eligibles", Required = Required.Always)]
        public string[] Eligibles { get; set; }
    }

    internal partial class NewActionMessage
    {
        [JsonProperty("new_action", Required = Required.Always)]
        public NewAction NewAction { get; set; }

        [JsonProperty("round_state", Required = Required.Always)]
        public RoundState RoundState { get; set; }
    }

    internal partial class NewAction
    {
        [JsonProperty("player_uuid", Required = Required.Always)]
        public string PlayerUuid { get; set; }

        [JsonProperty("action", Required = Required.Always)]
        public string Action { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public long Amount { get; set; }
    }

    internal partial class RoundResultMessage
    {
        [JsonProperty("winners", Required = Required.Always)]
        public Seat[] Winners { get; set; }

        [JsonProperty("round_state", Required = Required.Always)]
        public RoundState RoundState { get; set; }
    }


    internal partial class ValidActionsMessage
    {
        [JsonProperty("valid_actions", Required = Required.Always)]
        public ValidAction[] ValidActions { get; set; }
    }

    internal partial class ValidAction
    {
        [JsonProperty("action", Required = Required.Always)]
        public string Action { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public AmountUnion Amount { get; set; }
    }

    internal partial class AmountClass
    {
        [JsonProperty("max", Required = Required.Always)]
        public long Max { get; set; }

        [JsonProperty("min", Required = Required.Always)]
        public long Min { get; set; }
    }

    [JsonConverter(typeof(AmountUnionConverter))]
    internal partial struct AmountUnion
    {
        public AmountClass AmountClass;
        public long? Integer;

        public static implicit operator AmountUnion(AmountClass AmountClass) => new AmountUnion { AmountClass = AmountClass };
        public static implicit operator AmountUnion(long Integer) => new AmountUnion { Integer = Integer };
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AmountUnionConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AmountUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AmountUnion) || t == typeof(AmountUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new AmountUnion { Integer = integerValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<AmountClass>(reader);
                    return new AmountUnion { AmountClass = objectValue };
            }
            throw new Exception("Cannot unmarshal type AmountUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (AmountUnion)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.AmountClass != null)
            {
                serializer.Serialize(writer, value.AmountClass);
                return;
            }
            throw new Exception("Cannot marshal type AmountUnion");
        }

        public static readonly AmountUnionConverter Singleton = new AmountUnionConverter();
    }

}
