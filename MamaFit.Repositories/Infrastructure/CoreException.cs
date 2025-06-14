using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MamaFit.Repositories.Infrastructure
{
    public class CoreException : Exception
    {
        public CoreException(string code, string message = "", int statusCode = StatusCodes.Status500InternalServerError)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }
        
        public string Code { get; }

        public int StatusCode { get; set; }

        [JsonExtensionData] public Dictionary<string, object> AdditionalData { get; set; }

    }

    public class ErrorException : Exception
    {
        public int StatusCode { get; }

        public ErrorDetail ErrorDetail { get; }

        public ErrorException(int statusCode, string errorCode, string message = null)
        {
            StatusCode = statusCode;
            ErrorDetail = new ErrorDetail
            {
                ErrorCode = errorCode,
                ErrorMessage = message
            };
        }

        public ErrorException(int statusCode, ErrorDetail errorDetail)
        {
            StatusCode = statusCode;
            ErrorDetail = errorDetail;
        }
    }

    public class ErrorDetail
    {
        [JsonPropertyName("errorCode")] public required string ErrorCode { get; set; }

        [JsonPropertyName("errorMessage")] public required string ErrorMessage { get; set; }
    }
    public static class ErrorCode
    {
        public const string Forbidden = "Forbidden";
        public const string BadRequest = "Bad Request";
        public const string InternalServerError = "Internal Server Error";
        public const string UnAuthenticated = "Un-Authenticate";
        public const string UnAuthorized = "Un-Authorized";
        public const string NotFound = "Not Found";
        public const string Unknown = "Oops! Something went wrong, please try again later.";
        public const string NotUnique = "The resource is already, please try another.";
        public const string TokenExpired = "The TokenDto is already expired.";
        public const string TokenInvalid = "The TokenDto is invalid.";
        public const string Validated = "Validated.";
        public const string Conflicted = "Conflicted.";
        public const string InvalidInput = "Invalid input!";
        public const string Duplicate = "Duplicate!";
    }
    public static class ResponseCodeConstants
    {
        public const string CREATED = "Created!";
        public const string NOT_FOUND = "Not found!";
        public const string SUCCESS = "Success!";
        public const string FAILED = "Failed!";
        public const string EXISTED = "Existed!";
        public const string DUPLICATE = "Duplicate!";
        public const string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
        public const string INVALID_INPUT = "Invalid input!";
        public const string UNAUTHORIZED = "Unauthorized!";
        public const string BADREQUEST = "Bad request!";
        public const string FORBIDDEN = "Forbidden!";
    }
    
    public static class ApiCodes
    {
        // ───────────────────────────────
        // ✅ SUCCESS CODES
        // ───────────────────────────────
        public const string SUCCESS = "SUCCESS";
        public const string CREATED = "CREATED";
        public const string VALIDATED = "VALIDATED";

        // ───────────────────────────────
        // ❌ CLIENT ERRORS (4xx)
        // ───────────────────────────────
        public const string BAD_REQUEST = "BAD_REQUEST";
        public const string UNAUTHORIZED = "UNAUTHORIZED";
        public const string UNAUTHENTICATED = "UNAUTHENTICATED";
        public const string FORBIDDEN = "FORBIDDEN";
        public const string NOT_FOUND = "NOT_FOUND";
        public const string CONFLICT = "CONFLICT";
        public const string INVALID_INPUT = "INVALID_INPUT";
        public const string NOT_UNIQUE = "NOT_UNIQUE";
        public const string DUPLICATE = "DUPLICATE";
        public const string EXISTED = "EXISTED";
        public const string VALIDATION_FAILED = "VALIDATION_FAILED";

        // ───────────────────────────────
        // 🔒 AUTH & TOKEN ERRORS
        // ───────────────────────────────
        public const string TOKEN_EXPIRED = "TOKEN_EXPIRED";
        public const string TOKEN_INVALID = "TOKEN_INVALID";

        // ───────────────────────────────
        // 💥 SERVER ERRORS (5xx)
        // ───────────────────────────────
        public const string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
        public const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
    }
}

