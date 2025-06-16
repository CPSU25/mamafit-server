using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.Repositories.Infrastructure
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        //public object? AdditionalData { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public string Code { get; set; }

        public ResponseModel(int statusCode, string code, T? data, string? message = null)
        {
            this.StatusCode = statusCode;
            this.Code = code;
            this.Data = data;
            //this.AdditionalData = additionalData;
            this.Message = message;
        }

        public ResponseModel(int statusCode, string code, string? message)
        {
            this.StatusCode = statusCode;
            this.Code = code;
            this.Message = message;
        }

        public static ResponseModel<T> OkResponseModel(T data, string code = ApiCodes.SUCCESS)
        {
            return new ResponseModel<T>(StatusCodes.Status200OK, code, data);
        }

        public static ResponseModel<T> NotFoundResponseModel(T? data, string code = ApiCodes.NOT_FOUND)
        {
            return new ResponseModel<T>(StatusCodes.Status404NotFound, code, data);
        }

        public static ResponseModel<T> BadRequestResponseModel(T? data, object? additionalData = null, string code = ApiCodes.BAD_REQUEST)
        {
            return new ResponseModel<T>(StatusCodes.Status400BadRequest, code, data);
        }

        public static ResponseModel<T> InternalErrorResponseModel(T? data, object? additionalData = null, string code = ApiCodes.INTERNAL_SERVER_ERROR)
        {
            return new ResponseModel<T>(StatusCodes.Status500InternalServerError, code, data);
        }

        public static ResponseModel<T> CreatedResponseModel<T>(T data, object? additionalData = null, string code = ApiCodes.CREATED)
        {
            return new ResponseModel<T>(StatusCodes.Status201Created, code, data);
        }

        public static ResponseModel<T> NoContentResponseModel<T>(string? message = null, string code = ApiCodes.SUCCESS)
        {
            return new ResponseModel<T>(StatusCodes.Status204NoContent, code, default, message);
        }

        public static ResponseModel<T> UnauthorizedResponseModel<T>(T? data = default, object? additionalData = null, string code = ApiCodes.UNAUTHORIZED)
        {
            return new ResponseModel<T>(StatusCodes.Status401Unauthorized, code, data);
        }

        public static ResponseModel<T> ForbiddenResponseModel<T>(T? data = default, object? additionalData = null, string code = ApiCodes.FORBIDDEN)
        {
            return new ResponseModel<T>(StatusCodes.Status403Forbidden, code, data);
        }
    }

    public class ResponseModel : ResponseModel<object>
    {
        public ResponseModel(int statusCode, string code, object? data, object? additionalData = null, string? message = null) : base(statusCode, code, data, message)
        {
        }
        public ResponseModel(int statusCode, string code, string? message) : base(statusCode, code, message)
        {
        }
    }
}
