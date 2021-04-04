using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Contract.Technical
{
    public interface ISerialize
    {
        OperationResult<string> Execute<T>(T objectDto);
    }
}
