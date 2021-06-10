using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;
using LabirunModel.Labirun.Response;

namespace LabirunModel.Tools
{
    public static class CommonTools
    {
        public static string ToPlayerName(this string name)
        {
            return name.IsEmptyString() ? "####" : name;
        }
        public static long ToSavedScore(this double time)
        {
            return (long)Math.Truncate(time * 100);
        }


        public static string FromSavedScore(this long time)
        {
            return (time / 100f).ToString("N");
        }




        /// <summary>
        /// get the remainder and the quotient of an integer division
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static (long remainder, long quot) ModuloDivision(this long a, long b)
        {
            var remainder = a % b;
            var quot = a / b;
            return (remainder, quot);
        }
        public static bool IsEmptyDate(this DateTime dateTime)
        {
            return dateTime == default(DateTime);
        }

        public static List<IEnumerable<T>> CreateBatch<T>(List<T> list, int size)
        {
            List<IEnumerable<T>> result = new List<IEnumerable<T>>();
            var batchSize = size;
            var batchCount = Math.Ceiling(list.Count() / (decimal)batchSize);
            for (var i = 0; i < batchCount; i++)
            {
                var batch = list
                    .Skip(i * batchSize)
                    .Take(batchSize);
                result.Add(batch);
            }

            return result;
        }
        public static bool IsMonthlyFactoryBoost(this ItemId itemId)
        {
            switch (itemId)
            {
                case ItemId.FactoryMonthlyBoost20:
                case ItemId.FactoryMonthlyBoost50:
                case ItemId.FactoryMonthlyBoost100:
                case ItemId.FactoryMonthlyBoost200:
                    return true;
            }

            return false;
        }
        public static bool IsDailyFactoryBoost(this ItemId itemId)
        {
            switch (itemId)
            {
                case ItemId.FactoryDailyBoost10:
                case ItemId.FactoryDailyBoost20:
                case ItemId.FactoryDailyBoost50:
                case ItemId.FactoryDailyBoost100:
                    return true;
            }

            return false;
        }
        public static List<SingleLeaderBoardEntry> ParseResult(this LeaderBoardResult result)
        {
            if (result == null) return new List<SingleLeaderBoardEntry>();

            var list = result.First10;


            list.MergeLeaderBoardEntry(result.BeforePlayer);
            if (result.PlayerEntry != null) list.MergeLeaderBoardEntry(result.PlayerEntry);
            list.MergeLeaderBoardEntry(result.AfterPlayer);

            List<SingleLeaderBoardEntry> entries = new List<SingleLeaderBoardEntry>();
            switch (result.LeaderBoardId)
            {
                case LeaderBoardId.Combo4Ghost:
                    entries = list.Select(en => new SingleLeaderBoardEntry()
                    {
                        Id = en.Id,
                        Score = en.Combo4Ghost,
                        UserName = en.UserName,
                        Rank = en.Rank
                    }).OrderByDescending(en => en.Score).ToList();
                    break;

                case LeaderBoardId.SuperCombo4Ghost:
                    entries = list.Select(en => new SingleLeaderBoardEntry()
                    {
                        Id = en.Id,
                        Score = en.SuperCombo4Ghost,
                        UserName = en.UserName,
                        Rank = en.Rank
                    }).OrderByDescending(en => en.Score).ToList();
                    break;

                default:
                    entries = list.Select(en => new SingleLeaderBoardEntry()
                    {
                        Id = en.Id,
                        Score = en.TotalPoint,
                        UserName = en.UserName,
                        Rank = en.Rank
                    }).OrderByDescending(en => en.Score).ToList();
                    break;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Rank = i + 1;
            }
            return entries;
        }

        public static void MergeLeaderBoardEntry(this List<GlobalLeaderBoardEntry> list, List<GlobalLeaderBoardEntry> around)
        {
            if (around == null) return;

            foreach (var entry in around)
            {
                if (entry != null)
                {
                    var index = list.FirstOrDefault(it => it.Id == entry.Id);
                    if (index == null)
                    {
                        list.Add(entry);
                    }
                }

            }
        }
        public static void MergeLeaderBoardEntry(this List<GlobalLeaderBoardEntry> list, GlobalLeaderBoardEntry entry)
        {
            if (entry == null) return;

            if (entry != null)
            {
                var index = list.FirstOrDefault(it => it.Id == entry.Id);
                if (index == null)
                {
                    list.Add(entry);
                }
            }

        }



        /// Randomize array element order in-place.
        /// Using Durstenfeld shuffle algorithm.
        public static void ShuffleArray<T>(this List<T> array)
        {
            var random = new Random();
            for (var i = array.Count - 1; i > 0; i--)
            {
                //j ← random integer such that 0 ≤ j ≤ i
                var j = random.Next(0, i + 1);
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }



        public static ItemId OpenChest(List<GenericViewItem> list)
        {
            var items = new List<ItemId>();
            if (list == null || list.Count == 0) return ItemId.None;

            foreach (GenericViewItem item in list)
            {
                for (int i = 0; i < item.Probability; i++)
                {
                    items.Add(item.ItemId);
                }
            }

            items.ShuffleArray();

            var idx = new Random().Next(0, items.Count);
            if (idx >= 0 && items.Count > idx) return items[idx];
            return ItemId.None;
        }

        //public static ItemId OpenChest(List<ItemProbability> list)
        //{
        //    var items = new List<ItemId>();
        //    if (list == null || list.Count == 0) return ItemId.None;

        //    foreach (ItemProbability itemProbability in list)
        //    {
        //        for (int i = 0; i < itemProbability.Probability; i++)
        //        {
        //            items.Add(itemProbability.Id);
        //        }
        //    }

        //    items.ShuffleArray();

        //    var idx = new Random().Next(0, items.Count);
        //    if (idx >= 0 && items.Count > idx) return items[idx];
        //    return ItemId.None;
        //}



        public static int RoundUpToNearestMultiOf(this double number, int round = 5)
        {
            return (int)(Math.Ceiling(number / round) * round);
        }

        public static int RoundUpToNearestMultiOf(this int number, int round = 5)
        {
            return (int)(Math.Ceiling((double)(number / round)) * round);
        }

        public static bool IsExpired(this DateTime dateTime)
        {
            var subtract = DateTime.UtcNow.Subtract(dateTime);
            return subtract.TotalSeconds >= 0;
        }

        public static bool IsActive(this DateTime dateTime)
        {
            var subtract = DateTime.UtcNow.Subtract(dateTime);
            return subtract.TotalSeconds < 0;
        }

        public static bool IsEmptyString(this string s)
        {
            return String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s);
        }

        public static bool IsNotEmptyString(this string s)
        {
            return !(String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s));
        }


        public static string GenerateRandomHash()
        {
            //https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //var stringSalt = ConvertToString(salt);
            var stringSalt = salt.ToBase64();
            return stringSalt;
        }



        public static string ToBase64(this byte[] imageBytes)
        {
            // Convert byte[] to Base64 String
            if (imageBytes != null) return Convert.ToBase64String(imageBytes);
            return null;
        }

        public static byte[] Base64ToByte(this string base64String)
        {
            // Convert Base64 String to byte[]
            if (base64String != null)
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                return imageBytes;
            }

            return null;
        }



        
    }
}