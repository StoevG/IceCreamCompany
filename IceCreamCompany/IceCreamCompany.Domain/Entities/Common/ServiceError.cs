using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamCompany.Domain.Entities.Common
{
    [Serializable]
    public class ServiceError
    {
        public ServiceError(string message, int code)
        {
            this.Message = message;
            this.Code = code;
        }

        public ServiceError() { }

        public string Message { get; }

        public int Code { get; }

        public static ServiceError DefaultError => new ServiceError("An exception occured.", 999);

        public static ServiceError ModelStateError(string validationError)
        {
            return new ServiceError(validationError, 998);
        }

        public static ServiceError ForbiddenError => new ServiceError("You are not authorized to call this action.", 998);

        public static ServiceError CustomMessage(string errorMessage)
        {
            return new ServiceError(errorMessage, 997);
        }

        public static ServiceError UserFailedToUpdate => new ServiceError("Failed to update User.", 997);

        public static ServiceError UserNotFound => new ServiceError("User with this id does not exist", 996);

        public static ServiceError UserFailedToCreate => new ServiceError("Failed to create User.", 995);

        public static ServiceError Canceled => new ServiceError("The request canceled successfully!", 994);

        public static ServiceError NotFound => new ServiceError("The specified resource was not found.", 990);

        public static ServiceError ValidationFormat => new ServiceError("Request object format is not true.", 901);

        public static ServiceError Validation => new ServiceError("One or more validation errors occurred.", 900);

        public static ServiceError SearchAtLeastOneCharacter => new ServiceError("Search parameter must have at least one character!", 898);

        public static ServiceError ServiceProviderNotFound => new ServiceError("Service Provider with this name does not exist.", 700);

        public static ServiceError ServiceProvider => new ServiceError("Service Provider failed to return as expected.", 600);

        public static ServiceError DateTimeFormatError => new ServiceError("Date format is not true. Date format must be like yyyy-MM-dd (2019-07-19)", 500);

        public override bool Equals(object obj)
        {
            var error = obj as ServiceError;

            return Code == error?.Code;
        }

        public bool Equals(ServiceError error)
        {
            return Code == error?.Code;
        }

        public override int GetHashCode()
        {
            return Code;
        }

        public static bool operator ==(ServiceError a, ServiceError b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ServiceError a, ServiceError b)
        {
            return !(a == b);
        }
    }
}
