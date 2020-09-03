using System;

namespace Shutdown.GUI.Data.Unit
{
    public abstract class UnitBase<TImpl> : IEquatable<TImpl>
        where TImpl : UnitBase<TImpl>
    {
        public string Name { get; }

        protected UnitBase(string name)
        {
            Name = name;
        }
        
        public bool Equals(TImpl other) => other != null && Name == other.Name;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TImpl) obj);
        }

        public override int GetHashCode() => (Name != null ? Name.GetHashCode() : 0);

        public override string ToString() => Name;
    }

    public abstract class PriorityUnitBase<TImpl> : UnitBase<TImpl>
        where TImpl : PriorityUnitBase<TImpl>
    {
        public int Priority { get; }
        
        protected PriorityUnitBase(string name, int priority) 
            : base(name)
        {
            Priority = priority;
        }
    }
}