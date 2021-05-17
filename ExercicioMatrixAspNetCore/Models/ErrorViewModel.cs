namespace ExercicioMatrixAspNetCore.Models
{
    public class ErrorViewModel
    {
        public int status { get; set; }

        public string error { get; set; }

        public ErrorViewModel(int statusCode, string serverMessage)
        {
            this.status = statusCode;
            this.error = serverMessage;
        }
    }
}
