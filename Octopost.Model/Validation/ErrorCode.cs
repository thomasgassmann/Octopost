namespace Octopost.Model.Validation
{
    using System;

    public class ErrorCode : IEquatable<ErrorCode>
    {
        internal ErrorCode(string code) =>
            this.Code = code;

        public string Code { get; }

        public static ErrorCode Parse(string str) =>
            new ErrorCode(str.ToUpper());

        public static ErrorCode Parse(
            ErrorCodeType errorType,
            OctopostEntityName containingEntity,
            PropertyName propertyName,
            OctopostEntityName? referenceEntity = null)
        {
            var strReferenceEntity = referenceEntity.ToString().ToUpper();
            var strEntity = containingEntity.ToString().ToUpper();
            var errorCode = string.Empty;
            switch (errorType)
            {
                case ErrorCodeType.InvalidReferenceId:
                    if (referenceEntity == null)
                    {
                        throw new ArgumentException($"'{nameof(referenceEntity)}' cannot be null");
                    }

                    errorCode = $"INVALID_{strReferenceEntity}_ID";
                    break;
                case ErrorCodeType.PropertyDataNullOrEmpty:
                    errorCode = $"{propertyName.Name}_EMPTY";
                    break;
                case ErrorCodeType.PropertyInvalidData:
                    errorCode = $"{propertyName.Name}_INVALID_DATA";
                    break;
                case ErrorCodeType.Duplicate:
                    errorCode = $"DUPLICATE_{propertyName.Name}";
                    break;
                case ErrorCodeType.Recursion:
                    errorCode = $"RECURSIVE_{containingEntity.ToString()}";
                    break;
                case ErrorCodeType.TooShort:
                    errorCode = $"{propertyName.Name}_TOO_SHORT";
                    break;
                case ErrorCodeType.OutOfRange:
                    errorCode = $"{propertyName.Name}_OUT_OF_RANGE";
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(errorCode))
            {
                throw new ArgumentException($"Entry for enum value '{errorType}' not found");
            }

            return new ErrorCode(errorCode);
        }

        public static bool operator ==(ErrorCode left, ErrorCode right) =>
            left.Code == right.Code;

        public static bool operator !=(ErrorCode left, ErrorCode right) =>
            left.Code != right.Code;

        public bool Equals(ErrorCode other) =>
            this.Code == other.Code;

        public override bool Equals(object obj) =>
            obj is ErrorCode ec && ec.Code == this.Code;

        public override int GetHashCode() =>
            this.Code.GetHashCode();
    }
}
