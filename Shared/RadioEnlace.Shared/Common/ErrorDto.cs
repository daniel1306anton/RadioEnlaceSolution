using System.Collections.Generic;
using System.Linq;

namespace RadioEnlace.Shared.Common
{
    public class ErrorDto
    {
        /// <summary>
        /// Error por código.
        /// </summary>
        private const ushort InternalError = 1;
        /// <summary>
        /// Error por datos de usuario.
        /// </summary>
        private const ushort ExternalError = 2;
        /// <summary>
        /// Mensaje del error.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Lista de errores para usuario
        /// </summary>
        public IEnumerable<string> MessageList { get; set; }
        /// <summary>
        /// Codigo del error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tipo del error.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// indica si es bloqueante o es solo de advertencia; por default es bloqueante.
        /// </summary>
        public bool IsWarning { get; set; }

        public ErrorDto()
        { }

        /// <summary>
        /// Sobrecarga del método.
        /// </summary>
        public ErrorDto(string code, string message, ushort type, bool isWarning = false)
        {
            Code = code;
            Message = message;
            Type = type;
            IsWarning = isWarning;
        }
        /// <summary>
        /// Construcción de un error producido internamente (por código).
        /// </summary>
        public static ErrorDto BuildTechnical(string message)
        {
            return new ErrorDto("TE001", message, InternalError);

        }
        public static ErrorDto BuildTechnical(string message, string code)
        {
            return new ErrorDto(code, message, InternalError);

        }
        /// <summary>
        /// Construcción de un error producido por datos de usuario.
        /// </summary>
        public static ErrorDto BuildUser(IEnumerable<string> messageList)
        {
            return new ErrorDto()
            {
                Code = "BE001",
                Message = messageList != null && messageList.Any() ? messageList.Aggregate((current, next) => { return current + ", " + next; }) : "",
                MessageList = messageList,
                Type = ExternalError
            };
        }

        public static ErrorDto BuildUser(string message, string code = null, List<string> messageList = null)
        {
            return new ErrorDto()
            {
                Code = code == null ? "BE001" : code,
                Message = message == null ? "Error User" : message,
                MessageList = messageList == null ? new List<string>() : messageList,
                Type = ExternalError
            };
        }

        /// <summary>
        /// Construcción de un error producido internamente (por servicio).
        /// </summary>
        public static ErrorDto BuildService(string message, string code)
        {
            return new ErrorDto(code, message, InternalError);

        }

        /// <summary>
        /// Construcción de un error producido por Proceso de Validación de solicitudes.
        /// </summary>
        public static ErrorDto BuildUser(string message, bool isWarning, string code = null, List<string> messageList = null)
        {
            return new ErrorDto()
            {
                Code = code ?? "BE001",
                Message = message,
                MessageList = messageList == null ? new List<string>() : messageList,
                Type = ExternalError,
                IsWarning = isWarning
            };
        }
    }
}
