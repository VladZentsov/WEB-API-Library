using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public class LibraryException:Exception
    {
        public LibraryException()
        {

        }
        public LibraryException(string message): base(message)
        {

        }
    }
}
