using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public class Error
    {

        // private => no one can create an instance(Object) of Error directly
        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public string Code { get;}
        public string Description { get;}
        public ErrorType Type { get;}


        // Static Factory Methods To Create Error 
        #region Static Factory Methods
        public static Error Failure(string code = "General.Failure", string description = "A General Failure Has Occurred")
        {
            return new Error(code, description, ErrorType.Failure);
        }

        public static Error Validation(string code = "General.Validation", string description = "Validation Error Has Occurred")
        {
            return new Error(code, description, ErrorType.Validation);
        }

        public static Error NotFound(string code = "General.NotFound", string description = "The Request Resource Was Not Found")
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error Unauthorized(string code = "General.Unauthorized", string description = "You Are Not Authorized To Access This Resource")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }

        public static Error Forbidden(string code = "General.Forbidden", string description = "You Do Not Have Permission Access This Resource")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }

        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "The Provided Credentials Are Invalid")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        } 
        #endregion
    }
}
