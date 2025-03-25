using System;

namespace Utilities.Exceptions
{

    /// Excepción base para todos los errores relacionados con la capa de datos.
    public class DataException : Exception
    {

        /// Inicializa una nueva instancia de <see cref="DataException"/> con un mensaje de error.
        public DataException(string message) : base(message)
        {
        }

  
        /// Inicializa una nueva instancia de <see cref="DataException"/> con un mensaje de error y una excepción interna.

  
        public DataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


    /// Excepción lanzada cuando ocurre un error de conexión con la base de datos.
    public class DatabaseConnectionException : DataException
    {
       
        /// Inicializa una nueva instancia de <see cref="DatabaseConnectionException"/> con un mensaje de error.
        public DatabaseConnectionException(string message) : base(message)
        {
        }

        
        /// Inicializa una nueva instancia de <see cref="DatabaseConnectionException"/> con un mensaje de error y una excepción interna.
        public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

   
    /// Excepción lanzada cuando ocurre un error al ejecutar una consulta en la base de datos.
    public class QueryExecutionException : DataException
    {

        /// Inicializa una nueva instancia de <see cref="QueryExecutionException"/> con un mensaje de error.
        public QueryExecutionException(string message) : base(message)
        {
        }

        
        /// Inicializa una nueva instancia de <see cref="QueryExecutionException"/> con un mensaje de error y una excepción interna.
        public QueryExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


    /// Excepción lanzada cuando ocurre un conflicto de concurrencia en la base de datos.
    public class ConcurrencyException : DataException
    {
        /// Inicializa una nueva instancia de <see cref="ConcurrencyException"/> con un mensaje de error.
        public ConcurrencyException(string message) : base(message)
        {
        }

       
        /// Inicializa una nueva instancia de <see cref="ConcurrencyException"/> con un mensaje de error y una excepción interna.
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    
    /// Excepción lanzada cuando se violan restricciones de integridad en la base de datos
    public class DataIntegrityException : DataException
    {
       
        /// Inicializa una nueva instancia de <see cref="DataIntegrityException"/> con un mensaje de error.

        public DataIntegrityException(string message) : base(message)
        {
        }

       
        /// Inicializa una nueva instancia de <see cref="DataIntegrityException"/> con un mensaje de error y una excepción interna.
   
        public DataIntegrityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

