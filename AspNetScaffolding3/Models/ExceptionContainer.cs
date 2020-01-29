using System;

namespace AspNetScaffolding.Models
{
    public class ExceptionContainer
    {
        public Exception Exception { get; set; }

        public ExceptionContainer(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
