using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrightNight.Models
{
    public class Common
    {
        public class ApiRes<T>
        {
            public bool? Result { get; set; } = null;
            public string? Msg { get; set; }
            public T? Data { get; set; }
            public ApiRes<T> HandleError(Exception ex)
            {
                this.Result = false;
                var e = GetException(ex);
                if (e is ApplicationException)
                {
                    this.Msg = e.Message;
                }
                else
                {
                    this.Msg = $"Error:{ex.Message}\n{ex.StackTrace}";
                }
                return this;
            }
            private Exception GetException(Exception ex)
            {
                return ex.InnerException != null ? GetException(ex.InnerException) : ex;
            }
        }
    }
}