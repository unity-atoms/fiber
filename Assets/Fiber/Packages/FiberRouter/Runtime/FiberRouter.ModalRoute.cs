using System;

namespace Fiber.Router
{
    [Serializable]
    public struct ModalRoute : IEquatable<ModalRoute>
    {
        public string Id;
        public string StringValue;
        public int IntValue;
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