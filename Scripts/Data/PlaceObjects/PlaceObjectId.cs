using System;

namespace Unity1week202312.PlaceObjects
{
    public readonly struct PlaceObjectId : IEquatable<PlaceObjectId>
    {
        public int Value { get; }
        public int GroupId { get; }
        public int ChildId { get; }

        public PlaceObjectId(int value)
        {
            if (value < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value), value, "0以上の値を指定してください");
            }

            Value = value;
            GroupId = value / 10;
            ChildId = Value - GroupId;
        }

        public static bool operator ==(PlaceObjectId a, PlaceObjectId b) => a.Value == b.Value;
        public static bool operator !=(PlaceObjectId a, PlaceObjectId b) => a.Value != b.Value;

        public override bool Equals(object obj)
        {
            return obj is PlaceObjectId other && Equals(other);
        }

        public bool Equals(PlaceObjectId other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public PlaceObjectId AddAge(int age)
        {
            return new PlaceObjectId(Value + age);
        }
    }
}