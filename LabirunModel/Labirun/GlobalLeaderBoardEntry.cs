using System;
using System.Collections.Generic;
using LabirunModel.Labirun.Request;

namespace LabirunModel.Labirun
{
    public sealed class TotalPointComparer : IComparer<GlobalLeaderBoardEntry>
    {
        public int Compare(GlobalLeaderBoardEntry x, GlobalLeaderBoardEntry y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            var val = x.TotalPoint.CompareTo(y.TotalPoint);
            if (val==0)
            {
                return x.Id.CompareTo(y.Id);
            }

            return val;
        }
    }

    public class GlobalLeaderBoardEntry :  IEquatable<GlobalLeaderBoardEntry>
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public int TotalPoint { get; set; }
        public int Combo4Ghost { get; set; }
        public int SuperCombo4Ghost { get; set; }
        public DateTime UpDateTime { get; set; }
        public long Rank { get; set; }

        public GlobalLeaderBoardEntry():base()
        {
            
        }

        public bool Equals(GlobalLeaderBoardEntry other)
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
            return Equals((GlobalLeaderBoardEntry)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public void UpdateAllScores(LeaderBoardsScoreUpdate scoreUpdate)
        {
            if (scoreUpdate == null) return;

            TotalPoint += scoreUpdate.TotalPoint;
            Combo4Ghost += scoreUpdate.Combo4Ghost;
            SuperCombo4Ghost += scoreUpdate.SuperCombo4Ghost;
        }


        public void AddToScore(LeaderBoardRequest request)
        {
            if (request == null) return;

            switch (request.LeaderBoardId)
            {
                case LeaderBoardId.TotalPoint:
                    TotalPoint += request.Score;
                    break;
                case LeaderBoardId.Combo4Ghost:
                    Combo4Ghost += request.Score;
                    break;
                case LeaderBoardId.SuperCombo4Ghost:
                    SuperCombo4Ghost += request.Score;
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return $"{Rank} {UserName}";
        }
    }

}