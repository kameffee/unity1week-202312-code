using System;

namespace Unity1week202312.Stage
{
    public readonly struct StageId : IEquatable<StageId>
    {
        public static StageId First => new(0);

        public int Value { get; }

        public StageId(int value)
        {
            if (value < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value), value, "0以上の値を指定してください");
            }

            Value = value;
        }

        public StageId Next() => new(Value + 1);
        public StageId Previous() => new(Value - 1); 
        public bool IsFirst() => Value == 0;

        public bool Equals(StageId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is StageId other && Equals(other);
        }

        public static bool operator ==(StageId left, StageId right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(StageId left, StageId right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}