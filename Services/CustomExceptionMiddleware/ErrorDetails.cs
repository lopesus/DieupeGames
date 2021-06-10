using DieupeGames.Helpers;

namespace DieupeGames.Services.CustomExceptionMiddleware
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            var json = this.ToJsonDotNetCore();

            return json;
        }
    }
}