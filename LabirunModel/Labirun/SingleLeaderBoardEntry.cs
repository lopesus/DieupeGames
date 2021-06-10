using System;
using System.Collections.Generic;

namespace LabirunModel.Labirun
{
    public class SingleLeaderBoardEntry : IEquatable<SingleLeaderBoardEntry>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public long Score { get; set; }
        public long Rank { get; set; }


        public sealed class SingleLeaderBoardEntryComparer : IComparer<SingleLeaderBoardEntry>
        {
            public int Compare(SingleLeaderBoardEntry x, SingleLeaderBoardEntry y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return -x.Score.CompareTo(y.Score);
            }
        }

        public override string ToString()
        {
            return $"{Rank} - {UserName} - {Score}";
        }

        public bool Equals(SingleLeaderBoardEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SingleLeaderBoardEntry)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}