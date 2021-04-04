using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Contract.AppValues
{
    public interface IServiceSecurity
    {
        string User { get; }
        string Password { get; }
        string Url { get; }
    }
}
