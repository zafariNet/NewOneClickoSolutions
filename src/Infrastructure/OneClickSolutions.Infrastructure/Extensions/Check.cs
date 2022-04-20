using JetBrains.Annotations;
using OneClickSolutions.Infrastructure.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneClickSolutions.Infrastructure.Extensions;
[DebuggerStepThrough]
public static class Check
{
    [ContractAnnotation("value:null => halt")]
    public static T NotNull<T>(T value, [InvokerParameterName][NotNull] string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static string NotNullOrEmpty(string value, [InvokerParameterName][NotNull] string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
        }

        return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static string NotNullOrWhiteSpace(string value, [InvokerParameterName][NotNull] string parameterName)
    {
        if (value.IsNullOrWhiteSpace())
        {
            throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
        }

        return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, [InvokerParameterName][NotNull] string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
        }

        return value;
    }
    [ContractAnnotation("value:null => halt")]
    public static bool CheckregEx(string value,string regEx, [InvokerParameterName][NotNull] string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
        }
        if(!value.CheckReg(regEx))
            throw new ArgumentException(parameterName + "The value doesn't match with given RegEx!", parameterName);
        return true;
    }
}
