using FluentValidation.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Core.Messages;

public abstract class Command : Message, IRequest<ValidationResult>
{
    public DateTime Timestamp { get; private set; }
    public ValidationResult? ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }

    [MemberNotNull(nameof(ValidationResult))]
    public virtual bool Valido()
    {
        throw new NotImplementedException();
    }
}
