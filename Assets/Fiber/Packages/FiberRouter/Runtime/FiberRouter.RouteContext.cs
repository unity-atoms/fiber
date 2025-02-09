using System;

namespace Fiber.Router
{
    public struct RouteContext : IEquatable<RouteContext>
    {
        public string StringValue { get; private set; }
        public int IntValue { get; private set; }

        public RouteContext(string stringValue, int intValue)
        {
            StringValue = stringValue;
            IntValue = intValue;
        }

        public readonly bool Equals(RouteContext other)
        {
            return StringValue == other.StringValue && IntValue == other.IntValue;
        }
    }
}