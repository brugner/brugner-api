using System;
using System.Runtime.Serialization;

namespace Brugner.API.Core.Exceptions
{
    [Serializable]
    public class InvalidArgumentAPIException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:InvalidArgumentAPIException"/> class
        /// </summary>
        public InvalidArgumentAPIException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InvalidArgumentAPIException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public InvalidArgumentAPIException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InvalidArgumentAPIException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public InvalidArgumentAPIException(string message, System.Exception inner) : base(message, inner)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InvalidArgumentAPIException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected InvalidArgumentAPIException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
