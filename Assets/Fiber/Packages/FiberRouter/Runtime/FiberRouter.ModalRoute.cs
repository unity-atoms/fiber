using System;

namespace Fiber.Router
{
    public struct ModalRoute : IEquatable<ModalRoute>
    {
        public string Id { get; private set; }
        public string StringValue { get; private set; }
        public int IntValue { get; private set; }
        public ModalRoute(string id, string stringValue = default, int intValue = default)
        {
            Id = id;
            StringValue = stringValue;
            IntValue = intValue;
        }

        public readonly bool Equals(ModalRoute other)
        {
            return Id == other.Id && StringValue == other.StringValue && IntValue == other.IntValue;
        }

        public readonly bool IsEmpty()
        {
            return Id == default && StringValue == default && IntValue == default;
        }

        public static ModalRoute Empty() => new(default);
    }
}