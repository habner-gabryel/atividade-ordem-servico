using System;

namespace OrdemServicoTecnico.Exceptions
{
    public class InvalidEntityException : DomainException
    {
        public InvalidEntityException(string message) : base(message) { }
    }
}
