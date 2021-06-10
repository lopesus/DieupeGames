namespace LabirunModel.Labirun
{
    public class PlayerProfile
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public string PassWord { get; set; }
        public string Salt { get; set; }
        public bool IsAnonymousAccount { get; set; }
        public bool HasUniversalLogin { get; set; }
        public bool HasFacebookLogin { get; set; }
        public bool HasKartRidgeLogin { get; set; }
        public bool HasSteamLogin { get; set; }
    }
}