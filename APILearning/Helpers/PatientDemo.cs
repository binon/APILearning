using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APILearning.Helpers
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class PatientDemo
    {
        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("meta")]
        public PatientDemoMeta Meta { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("link")]
        public Link[] Link { get; set; }

        [JsonProperty("entry")]
        public Entry[] Entry { get; set; }
    }

    public partial class Entry
    {
        [JsonProperty("fullUrl")]
        public Uri FullUrl { get; set; }

        [JsonProperty("resource")]
        public Resource Resource { get; set; }

        [JsonProperty("search")]
        public Search Search { get; set; }
    }

    public partial class Resource
    {
        [JsonProperty("resourceType")]
        public ResourceType ResourceType { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("meta")]
        public ResourceMeta Meta { get; set; }

        [JsonProperty("text")]
        public Text Text { get; set; }

        [JsonProperty("identifier")]
        public Identifier[] Identifier { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("name")]
        public Name[] Name { get; set; }

        [JsonProperty("gender")]
        public Gender Gender { get; set; }

        [JsonProperty("birthDate")]
        public DateTimeOffset BirthDate { get; set; }

        [JsonProperty("address")]
        public Address[] Address { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("line")]
        public string[] Line { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("district")]
        public District District { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }
    }

    public partial class Identifier
    {
        [JsonProperty("system")]
        public SystemUnion System { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class ResourceMeta
    {
        [JsonProperty("versionId")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long VersionId { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTimeOffset LastUpdated { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("family")]
        public Family Family { get; set; }

        [JsonProperty("given")]
        public string[] Given { get; set; }

        [JsonProperty("prefix")]
        public Prefix[] Prefix { get; set; }
    }

    public partial class Text
    {
        [JsonProperty("div")]
        public string Div { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
    }

    public partial class Search
    {
        [JsonProperty("mode")]
        public Mode Mode { get; set; }
    }

    public partial class Link
    {
        [JsonProperty("relation")]
        public string Relation { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class PatientDemoMeta
    {
        [JsonProperty("lastUpdated")]
        public DateTimeOffset LastUpdated { get; set; }
    }

    public enum City { Carmarthenshire };

    public enum Country { Uk };

    public enum District { Carmarthen };

    public enum Gender { Female, Male };

    public enum SystemEnum { UrnOid216840111388321813888 };

    public enum Family { Davies };

    public enum Prefix { Mr, Ms };

    public enum ResourceType { Patient };

    public enum Mode { Match };

    public partial struct SystemUnion
    {
        public SystemEnum? Enum;
        public Uri PurpleUri;

        public static implicit operator SystemUnion(SystemEnum Enum) => new SystemUnion { Enum = Enum };
        public static implicit operator SystemUnion(Uri PurpleUri) => new SystemUnion { PurpleUri = PurpleUri };
    }

    public partial class PatientDemo
    {
        public static PatientDemo FromJson(string json) => JsonConvert.DeserializeObject<PatientDemo>(json, APILearning.Helpers.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PatientDemo self) => JsonConvert.SerializeObject(self, APILearning.Helpers.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                CityConverter.Singleton,
                CountryConverter.Singleton,
                DistrictConverter.Singleton,
                GenderConverter.Singleton,
                SystemUnionConverter.Singleton,
                SystemEnumConverter.Singleton,
                FamilyConverter.Singleton,
                PrefixConverter.Singleton,
                ResourceTypeConverter.Singleton,
                ModeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CityConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(City) || t == typeof(City?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Carmarthenshire")
            {
                return City.Carmarthenshire;
            }
            throw new Exception("Cannot unmarshal type City");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (City)untypedValue;
            if (value == City.Carmarthenshire)
            {
                serializer.Serialize(writer, "Carmarthenshire");
                return;
            }
            throw new Exception("Cannot marshal type City");
        }

        public static readonly CityConverter Singleton = new CityConverter();
    }

    internal class CountryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Country) || t == typeof(Country?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "UK")
            {
                return Country.Uk;
            }
            throw new Exception("Cannot unmarshal type Country");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Country)untypedValue;
            if (value == Country.Uk)
            {
                serializer.Serialize(writer, "UK");
                return;
            }
            throw new Exception("Cannot marshal type Country");
        }

        public static readonly CountryConverter Singleton = new CountryConverter();
    }

    internal class DistrictConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(District) || t == typeof(District?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Carmarthen")
            {
                return District.Carmarthen;
            }
            throw new Exception("Cannot unmarshal type District");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (District)untypedValue;
            if (value == District.Carmarthen)
            {
                serializer.Serialize(writer, "Carmarthen");
                return;
            }
            throw new Exception("Cannot marshal type District");
        }

        public static readonly DistrictConverter Singleton = new DistrictConverter();
    }

    internal class GenderConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Gender) || t == typeof(Gender?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "female":
                    return Gender.Female;
                case "male":
                    return Gender.Male;
            }
            throw new Exception("Cannot unmarshal type Gender");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Gender)untypedValue;
            switch (value)
            {
                case Gender.Female:
                    serializer.Serialize(writer, "female");
                    return;
                case Gender.Male:
                    serializer.Serialize(writer, "male");
                    return;
            }
            throw new Exception("Cannot marshal type Gender");
        }

        public static readonly GenderConverter Singleton = new GenderConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class SystemUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(SystemUnion) || t == typeof(SystemUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    if (stringValue == "urn:oid:2.16.840.1.113883.2.1.8.1.3.888")
                    {
                        return new SystemUnion { Enum = SystemEnum.UrnOid216840111388321813888 };
                    }
                    try
                    {
                        var uri = new Uri(stringValue);
                        return new SystemUnion { PurpleUri = uri };
                    }
                    catch (UriFormatException) { }
                    break;
            }
            throw new Exception("Cannot unmarshal type SystemUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (SystemUnion)untypedValue;
            if (value.Enum != null)
            {
                if (value.Enum == SystemEnum.UrnOid216840111388321813888)
                {
                    serializer.Serialize(writer, "urn:oid:2.16.840.1.113883.2.1.8.1.3.888");
                    return;
                }
            }
            if (value.PurpleUri != null)
            {
                serializer.Serialize(writer, value.PurpleUri.ToString());
                return;
            }
            throw new Exception("Cannot marshal type SystemUnion");
        }

        public static readonly SystemUnionConverter Singleton = new SystemUnionConverter();
    }

    internal class SystemEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(SystemEnum) || t == typeof(SystemEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "urn:oid:2.16.840.1.113883.2.1.8.1.3.888")
            {
                return SystemEnum.UrnOid216840111388321813888;
            }
            throw new Exception("Cannot unmarshal type SystemEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (SystemEnum)untypedValue;
            if (value == SystemEnum.UrnOid216840111388321813888)
            {
                serializer.Serialize(writer, "urn:oid:2.16.840.1.113883.2.1.8.1.3.888");
                return;
            }
            throw new Exception("Cannot marshal type SystemEnum");
        }

        public static readonly SystemEnumConverter Singleton = new SystemEnumConverter();
    }

    internal class FamilyConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Family) || t == typeof(Family?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Davies")
            {
                return Family.Davies;
            }
            throw new Exception("Cannot unmarshal type Family");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Family)untypedValue;
            if (value == Family.Davies)
            {
                serializer.Serialize(writer, "Davies");
                return;
            }
            throw new Exception("Cannot marshal type Family");
        }

        public static readonly FamilyConverter Singleton = new FamilyConverter();
    }

    internal class PrefixConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Prefix) || t == typeof(Prefix?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Mr":
                    return Prefix.Mr;
                case "Ms":
                    return Prefix.Ms;
            }
            throw new Exception("Cannot unmarshal type Prefix");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Prefix)untypedValue;
            switch (value)
            {
                case Prefix.Mr:
                    serializer.Serialize(writer, "Mr");
                    return;
                case Prefix.Ms:
                    serializer.Serialize(writer, "Ms");
                    return;
            }
            throw new Exception("Cannot marshal type Prefix");
        }

        public static readonly PrefixConverter Singleton = new PrefixConverter();
    }

    internal class ResourceTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ResourceType) || t == typeof(ResourceType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Patient")
            {
                return ResourceType.Patient;
            }
            throw new Exception("Cannot unmarshal type ResourceType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ResourceType)untypedValue;
            if (value == ResourceType.Patient)
            {
                serializer.Serialize(writer, "Patient");
                return;
            }
            throw new Exception("Cannot marshal type ResourceType");
        }

        public static readonly ResourceTypeConverter Singleton = new ResourceTypeConverter();
    }

    internal class ModeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Mode) || t == typeof(Mode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "match")
            {
                return Mode.Match;
            }
            throw new Exception("Cannot unmarshal type Mode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Mode)untypedValue;
            if (value == Mode.Match)
            {
                serializer.Serialize(writer, "match");
                return;
            }
            throw new Exception("Cannot marshal type Mode");
        }

        public static readonly ModeConverter Singleton = new ModeConverter();
    }
}