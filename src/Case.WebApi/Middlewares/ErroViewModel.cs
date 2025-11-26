namespace Case.WebApi.Middlewares
{
    internal class ErroViewModel
    {
        private string errorCode;
        private string message;

        public ErroViewModel(string errorCode, string message)
        {
            this.errorCode = errorCode;
            this.message = message;
        }
    }
}