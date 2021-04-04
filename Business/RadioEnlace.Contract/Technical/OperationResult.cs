using RadioEnlace.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Contract.Technical
{
    public class OperationResult
    {
        public bool Failure { get; }
        public bool Sucess { get; }
        public List<ErrorDto> ErrorList { get; set; }
        public OperationResult(bool isSuccess)
        {
            Sucess = isSuccess;
            Failure = !isSuccess;
        }
        public OperationResult(List<ErrorDto> errorList)
        {
            Sucess = (errorList == null || !errorList.Any());
            Failure = (errorList != null && errorList.Any());
            ErrorList = errorList;
        }
        public OperationResult(ErrorDto error)
        {
            var errorList = (error == null) ? null : new List<ErrorDto>() { error };
            Sucess = (errorList == null || !errorList.Any());
            Failure = (errorList != null && errorList.Any());
            ErrorList = errorList;
        }
        public OperationResult()
        {
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult(T result) : base(result != null)
        {
            Result = result;
        }

        public OperationResult(T result, bool isSuccess) : base(isSuccess)
        {
            Result = result;
        }

        public OperationResult(T result, ErrorDto error)
            : base(error)
        {
            Result = result;
        }

        public OperationResult(List<ErrorDto> errorList)
            : base(errorList)
        {
        }

        public OperationResult(ErrorDto error)
           : base(error)
        {
        }

        public OperationResult()
        {
        }

        public T Result { get; }
    }    

    public class OperationResultService<T> : OperationResult
    {
        public T Result { get; }
        public ulong? LoadId { get; }
        public IEnumerable<ErrorDto> ErrorListService { get; set; }
        public OperationResultService(T result) : base(result != null)
        {
            Result = result;
        }

        public OperationResultService(T result, bool isSuccess = true) : base(isSuccess)
        {
            Result = result;
        }
        public OperationResultService(T result, ulong? loadId, bool isSuccess = true) : base(isSuccess)
        {
            Result = result;
            LoadId = loadId;
        }

        public OperationResultService(T result, bool isSuccess, List<ErrorDto> errorListService) : base(isSuccess)
        {
            ErrorListService = errorListService;
        }


        public OperationResultService(ErrorDto error) : base(error)
        {
            ErrorListService = new List<ErrorDto>();
        }
        public OperationResultService(IEnumerable<ErrorDto> errorList, bool isSuccess = false) : base(isSuccess)
        {
            ErrorListService = errorList;

        }
        public OperationResultService(IEnumerable<ErrorDto> errorList, ErrorDto error) : base(error)
        {
            ErrorListService = errorList;

        }
    }
}
