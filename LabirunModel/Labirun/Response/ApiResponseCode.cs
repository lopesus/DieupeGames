namespace LabirunModel.Labirun.Response
{
    public enum ApiResponseCode
    {
        Ok = 0,
        Error = 1,
        EmptyRequestData,
        UserNameTaken,
        PlayerNotFound,
        UserNameNotSet,
        BadCredentials,
        PlayerAlreadyExist,
        PlayerAndPromoIdAreSame,
        NotFound,
        MaxCreatedMazePerPlayerReached,
    }
}