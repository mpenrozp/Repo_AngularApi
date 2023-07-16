using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProducto.Interfaces
{
    public interface IToken
    {
        string GenerateToken(string Usuario, string Password);
    }
}