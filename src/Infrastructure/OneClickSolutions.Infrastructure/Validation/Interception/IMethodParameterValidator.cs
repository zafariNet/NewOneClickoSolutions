using System.Collections.Generic;
using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Validation.Interception
{
    /// <summary>
    /// This interface is used to validate method parameters.
    /// </summary>
    public interface IMethodParameterValidator : ITransientDependency
    {
        IEnumerable<ValidationFailure> Validate(object validatingObject);
    }
}