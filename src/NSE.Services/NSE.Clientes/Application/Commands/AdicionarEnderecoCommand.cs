using Core.Messages;
using FluentValidation;

namespace NSE.Clientes.Application.Commands;

public class AdicionarEnderecoCommand : Command
{
    public Guid ClienteId { get; set; }
    public string? Logradouro { get; private set; }
    public string? Numero { get; private set; }
    public string? Complemento { get; private set; }
    public string? Bairro { get; private set; }
    public string? Cep { get; private set; }
    public string? Cidade { get; private set; }
    public string? Estado { get; private set; }

    //public AdicionarEnderecoCommand() { }

    public AdicionarEnderecoCommand(Guid clienteId, string logradouro, string numero, string complemento,
        string bairro, string cep, string cidade, string estado)
    {
        AggregateId = clienteId;
        ClienteId = clienteId;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cep = cep;
        Cidade = cidade;
        Estado = estado;
    }

    public override bool Valido()
    {
        ValidationResult = new EnderecoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class EnderecoValidation : AbstractValidator<AdicionarEnderecoCommand>
    {
        public EnderecoValidation()
        {
            RuleFor(e => e.Logradouro)
                .NotEmpty()
                .WithMessage("Informe o logradouro");

            RuleFor(e => e.Numero)
                .NotEmpty()
                .WithMessage("Informe o número");

            RuleFor(e => e.Cep)
                .NotEmpty()
                .WithMessage("Informe o CEP");

            RuleFor(e => e.Bairro)
                .NotEmpty()
                .WithMessage("Informe o bairro");

            RuleFor(e => e.Cidade)
                .NotEmpty()
                .WithMessage("Informe a cidade");

            RuleFor(e => e.Estado)
                .NotEmpty()
                .WithMessage("Informe o estado");
        }
    }
}
