using FluentValidation;

namespace OneClickSolutions.Infrastructure.FluentValidation
{
    public abstract class FluentModelValidator<TModel> : AbstractValidator<TModel>
    {
    }
}