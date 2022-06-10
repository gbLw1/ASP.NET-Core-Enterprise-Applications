using System.ComponentModel.DataAnnotations;
using Core.DomainObjects;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace MVC.Extensions;

public class CpfAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string cpf && Cpf.Validar(cpf))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("CPF inválido");
    }
}

public class CpfAttributeAdapter : AttributeAdapterBase<CpfAttribute>
{

    public CpfAttributeAdapter(CpfAttribute attribute, IStringLocalizer stringLocalizer)
        : base(attribute, stringLocalizer) { }

    public override void AddValidation(ClientModelValidationContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-cpf", GetErrorMessage(context));
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
        => "CPF em formato inválido";
}

public class CpfValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
    {
        if (attribute is CpfAttribute CpfAttribute)
        {
            return new CpfAttributeAdapter(CpfAttribute, stringLocalizer!);
        }

        return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}
