﻿using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using ZiylanEtl.Abstraction.Exceptions;

namespace ZiylanEtl.Aop
{
    public class ZiylanExceptionDataCallHandler : ICallHandler
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (getNext == null)
                throw new ArgumentNullException(nameof(getNext));
            var methodReturn = getNext()(input, getNext);
            if (methodReturn.Exception == null) return methodReturn;
            var ziylanEtlException  = new ZiylanEtlException();
            for (var index = 0; index < input.Arguments.Count; ++index)
            {
                ziylanEtlException.Data.Add(input.Arguments.GetParameterInfo(index).Name, input.Arguments[index]);
                methodReturn.Exception = ziylanEtlException;
            }
            
            return methodReturn;
        }

        public int Order { get; set; }
    }
}
