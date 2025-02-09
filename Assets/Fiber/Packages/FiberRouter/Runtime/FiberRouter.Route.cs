using System;
using System.Collections.Generic;
using Signals;

namespace Fiber.Router
{
    public struct Route : IEquatable<Route>, ISignal<Route>
    {
        // Copied from BaseSignal.cs
        #region Signal
        public byte DirtyBit { get; set; }
        public readonly bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
        public readonly Route Get() => this;
        private object _dependents;
        public readonly void NotifySignalUpdate()
        {
            // Not needed, since no one is going to listen to this signal directly.
            // If someone, like a computed signal, is listening for what route is 
            // the current one, it should listen to the route stack.
        }
        public void RegisterDependent(ISignal dependant)
        {
            if (_dependents is List<ISignal> listOfDependentSignals)
            {
                listOfDependentSignals.Add(dependant);
            }
            else if (_dependents is ISignal currentSignal)
            {
                var list = Signals.Pooling.ISignalListPool.Get();
                list.Add(currentSignal);
                list.Add(dependant);
            }
            else
            {
                _dependents = dependant;
            }
        }

        public void UnregisterDependent(ISignal dependant)
        {
            if (_dependents is List<ISignal> listOfDependentSignals)
            {
                listOfDependentSignals.Remove(dependant);
            }
            else
            {
                _dependents = null;
            }
        }
        #endregion


        public string Id;
        public bool IsLayoutRoute { get; private set; }
        public string StringValue { get; private set; }
        public int IntValue { get; private set; }
        public List<ModalRoute> Modals { get; private set; }

        public Route(string id, bool isLayoutRoute, List<ModalRoute> modals, string stringValue = default, int intValue = default)
        {
            Id = id;
            IsLayoutRoute = isLayoutRoute;
            Modals = modals;
            StringValue = stringValue;
            IntValue = intValue;

            DirtyBit = 0;
            _dependents = null;
        }

        public void PushModal(string id, string stringValue = default, int intValue = default)
        {
            if (IsModalPushed(id))
            {
                throw new Exception($"Modal with id {id} is already pushed");
            }
            Modals.Add(new ModalRoute(id, stringValue, intValue));
            ++DirtyBit;
        }

        public void PopModal()
        {
            Modals.RemoveAt(Modals.Count - 1);
            ++DirtyBit;
        }

        public void PopModal(string id)
        {
            if (!IsModalPushed(id))
            {
                throw new Exception($"Modal with id {id} is not pushed");
            }

            for (var i = 0; i < Modals.Count; i++)
            {
                if (Modals[i].Id == id)
                {
                    Modals.RemoveAt(i);
                    break;
                }
            }

            ++DirtyBit;
        }

        public readonly ModalRoute PeekModal()
        {
            return Modals.Count == 0 ? ModalRoute.Empty() : Modals[^1];
        }

        public readonly bool IsModalPushed(string id)
        {
            for (var i = 0; Modals != null && i < Modals.Count; i++)
            {
                if (Modals[i].Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public readonly bool Equals(Route other)
        {
            return Id == other.Id && IsLayoutRoute == other.IsLayoutRoute && StringValue == other.StringValue && IntValue == other.IntValue;
        }

        public readonly bool IsEmpty()
        {
            return Id == default && IsLayoutRoute == default && Modals == null && StringValue == default && IntValue == default;
        }

        public static Route Empty() => new(default, default, default);
    }
}