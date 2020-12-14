using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

namespace Ag3.Util.Suporte
{
    public static class ResultHelper
    {
        public static readonly object DefaultValue = new object();
        
        public static Error WithMetadata(this Error self, string metadataName)
        {
            return self.WithMetadata(metadataName, DefaultValue);
        }

        public static Result Fail(string metadataToken, string message)
        {
            var result = new Result();
            var error = new Error(message)
                .WithMetadata(metadataToken);
            return result.WithError(error);
        }

        public static Result Fail(
            Func<Error,Error> metadataAction
            )
        {
            var result = new Result();
            var error = metadataAction(new Error());
            return result.WithError(error);
        }
    }
}
