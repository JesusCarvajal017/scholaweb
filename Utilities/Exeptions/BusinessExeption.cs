using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exeptions
{

    /// Excepción base para todos los errores relacionados con la capa de negocio.
    public class BusinessException : Exception
    {
        /// Inicializa una nueva instancia de <see cref="BusinessException"/> con un mensaje de error.
        public BusinessException(string message) : base(message)
        {
        }
      
        /// Inicializa una nueva instancia de <see cref="BusinessException"/> con un mensaje de error y una excepción interna.
        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// Excepción lanzada cuando no se encuentra una entidad en el sistema.
    public class EntityNotFoundException : DataException
    {
        /// Tipo de entidad que no se encontró
        public string EntityType { get; }

        /// Identificador de la entidad buscada.
        public object EntityId { get; }


        /// Inicializa una nueva instancia de <see cref="EntityNotFoundException"/> con un mensaje personalizado.
        public EntityNotFoundException(string message) : base(message)
        {
        }

        /// Inicializa una nueva instancia de <see cref="EntityNotFoundException"/> con información de la entidad.
        public EntityNotFoundException(string entityType, object entityId)
            : base($"La entidad '{entityType}' con ID '{entityId}' no fue encontrada.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }


        /// Inicializa una nueva instancia de <see cref="EntityNotFoundException"/> con información detallada.
        public EntityNotFoundException(string entityType, object entityId, Exception innerException)
            : base($"La entidad '{entityType}' con ID '{entityId}' no fue encontrada.", innerException)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }


    /// Excepción lanzada cuando los datos proporcionados no cumplen con las reglas de validación.
    public class ValidationException : BusinessException
    {
        /// Campo que no cumple con la validación.
        public string PropertyName { get; }

   
        /// Inicializa una nueva instancia de <see cref="ValidationException"/> con un mensaje de error.
        public ValidationException(string message) : base(message)
        {
        }


        /// Inicializa una nueva instancia de <see cref="ValidationException"/> con información del campo inválido.
        public ValidationException(string propertyName, string message)
            : base($"Error de validación en '{propertyName}': {message}")
        {
            PropertyName = propertyName;
        }

        
        /// Inicializa una nueva instancia de <see cref="ValidationException"/> con información detallada.
   
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


    /// Excepción lanzada cuando se intenta realizar una operación que viola reglas de negocio.
    public class BusinessRuleViolationException : BusinessException
    {
       
        /// Código que identifica la regla de negocio violada.
        public string RuleCode { get; }

       
        /// Inicializa una nueva instancia de <see cref="BusinessRuleViolationException"/> con un mensaje de error.
        
        public BusinessRuleViolationException(string message) : base(message)
        {
        }

       
        /// Inicializa una nueva instancia de <see cref="BusinessRuleViolationException"/> con un código de regla.
        public BusinessRuleViolationException(string ruleCode, string message)
            : base($"Violación de regla de negocio [{ruleCode}]: {message}")
        {
            RuleCode = ruleCode;
        }


        /// Inicializa una nueva instancia de <see cref="BusinessRuleViolationException"/> con información detallada.
      
        public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    
    /// Excepción lanzada cuando se intenta acceder a un recurso sin los permisos adecuados.
    public class UnauthorizedAccessBusinessException : BusinessException
    {
        /// Recurso al que se intentó acceder.
        public string Resource { get; }

        /// Tipo de operación que se intentó realizar.
        public string Operation { get; }

        /// Inicializa una nueva instancia de <see cref="UnauthorizedAccessBusinessException"/> con un mensaje de error.
        public UnauthorizedAccessBusinessException(string message) : base(message)
        {
        }

        
        /// Inicializa una nueva instancia de <see cref="UnauthorizedAccessBusinessException"/> con información detallada.
        


        public UnauthorizedAccessBusinessException(string resource, string operation)
            : base($"Acceso no autorizado al recurso '{resource}' para la operación '{operation}'.")
        {
            Resource = resource;
            Operation = operation;
        }

      

        /// Inicializa una nueva instancia de <see cref="UnauthorizedAccessBusinessException"/> con información detallada.
        public UnauthorizedAccessBusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


    /// Excepción lanzada cuando ocurre un error al interactuar con recursos externos como bases de datos o servicios.

    public class ExternalServiceException : BusinessException
    {

        /// Nombre del servicio externo que generó el error.
        public string ServiceName { get; }


        /// Inicializa una nueva instancia de <see cref="ExternalServiceException"/> con un mensaje de error.

        public ExternalServiceException(string message) : base(message)
        {
        }

        /// Inicializa una nueva instancia de <see cref="ExternalServiceException"/> con información del servicio.
    
        public ExternalServiceException(string serviceName, string message)
            : base($"Error en el servicio externo '{serviceName}': {message}")
        {
            ServiceName = serviceName;
        }

        
        /// Inicializa una nueva instancia de <see cref="ExternalServiceException"/> con información detallada.
   
        public ExternalServiceException(string serviceName, string message, Exception innerException)
            : base($"Error en el servicio externo '{serviceName}': {message}", innerException)
        {
            ServiceName = serviceName;
        }
    }
}
