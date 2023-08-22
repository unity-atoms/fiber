using System;

namespace Signals
{
    // A struct that wraps either a value or a signal
    public struct SignalProp<T>
    {
        public T Value;
        public BaseSignal<T> Signal;

        private enum SignalPropType { Empty = 0, Value = 1, Signal = 2 }
        private SignalPropType Type;
        public bool IsEmpty { get => Type == SignalPropType.Empty; }
        public bool IsValue { get => Type == SignalPropType.Value; }
        public bool IsSignal { get => Type == SignalPropType.Signal; }

        public SignalProp(T value)
        {
            Value = value;
            Signal = null;
            Type = SignalPropType.Value;
        }

        public SignalProp(BaseSignal<T> signal)
        {
            Value = default;
            Signal = signal;
            Type = SignalPropType.Signal;
        }

        public static implicit operator SignalProp<T>(T value)
        {
            return new SignalProp<T>(value);
        }

        public static implicit operator SignalProp<T>(BaseSignal<T> signal)
        {
            return new SignalProp<T>(signal);
        }

        public T Get()
        {
            if (Type == SignalPropType.Signal)
            {
                return Signal.Get();
            }
            else if (Type == SignalPropType.Value)
            {
                return Value;
            }
            else
            {
                throw new Exception($"SignalProp<{typeof(T)}> is empty");
            }
        }
    }

    // Struct used by nodes in work loop when updating properties via signals
    public struct WorkLoopSignalProp<T>
    {
        private SignalProp<T> _signalProp;
        private byte _dirtyBit;
        public bool IsEmpty { get => _signalProp.IsEmpty; }
        public bool IsValue { get => _signalProp.IsValue; }
        public bool IsSignal { get => _signalProp.IsSignal; }
        public SignalProp<T> SignalProp { get => _signalProp; }

        public WorkLoopSignalProp(SignalProp<T> signalProp)
        {
            _signalProp = signalProp;
            _dirtyBit = signalProp.IsSignal ? signalProp.Signal.DirtyBit : default;
        }

        public bool Check()
        {
            if (_signalProp.IsSignal)
            {
                _signalProp.Signal.Get(); // Needs to be called to update dirty bit
                if (_dirtyBit != _signalProp.Signal.DirtyBit)
                {
                    _dirtyBit = _signalProp.Signal.DirtyBit;
                    return true;
                }
            }
            return false;
        }

        public T Get()
        {
            return _signalProp.Get();
        }
    }
}