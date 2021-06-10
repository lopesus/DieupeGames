using System;
using System.Collections.Generic;

namespace LabirunModel.Labirun
{
    public class ScoreRank : IComparable<ScoreRank>, IComparable
    {

        public long Score { get; set; }
        public long Rank { get; set; }
        public int CompareTo(ScoreRank other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return -Score.CompareTo(other.Score);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is ScoreRank other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ScoreRank)}");
        }

        public override string ToString()
        {
            return $"{nameof(Score)}: {Score}, {nameof(Rank)}: {Rank}";
        }

        public  string ToString2()
        {
            return $"{nameof(Rank)}: {Rank}, {nameof(Score)}: {Score}";
        }
    }
}