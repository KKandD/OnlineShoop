using System;
using System.Collections.Generic;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Exceptions
{

    public class BadRequestException: Exception
    {
        public BadRequestException(string message): base (message)
        {
                
        }
    }
}
