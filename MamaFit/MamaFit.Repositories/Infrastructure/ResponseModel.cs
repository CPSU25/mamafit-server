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
        public object? AdditionalData { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public string Code { get; set; }

        public ResponseModel(int statusCode, string code, T? data, object? additionalData = null, string? message = null)
        {
            this.StatusCode = statusCode;
            this.Code = code;
            this.Data = data;
            this.AdditionalData = additionalData;
            this.Message = message;
        }

        public ResponseModel(int statusCode, string code, string? message)
        {
            this.StatusCode = statusCode;
            this.Code = code;
            this.Message = message;
        }

        public static ResponseModel<T> OkResponseModel(T data, object? additionalData = null, string code = ResponseCodeConstants.SUCCESS)
        {
            return new ResponseModel<T>(StatusCodes.Status200OK, code, data, additionalData);
        }

        public static ResponseModel<T> NotFoundResponseModel(T? data, object? additionalData = null, string code = ResponseCodeConstants.NOT_FOUND)
        {
            return new ResponseModel<T>(StatusCodes.Status404NotFound, code, data, additionalData);
        }

        public static ResponseModel<T> BadRequestResponseModel(T? data, object? additionalData = null, string code = ResponseCodeConstants.FAILED)
        {
            return new ResponseModel<T>(StatusCodes.Status400BadRequest, code, data, additionalData);
        }

        public static ResponseModel<T> InternalErrorResponseModel(T? data, object? additionalData = null, string code = ResponseCodeConstants.FAILED)
        {
            return new ResponseModel<T>(StatusCodes.Status500InternalServerError, code, data, additionalData);
        }
    }

    public class ResponseModel : ResponseModel<object>
    {
        public ResponseModel(int statusCode, string code, object? data, object? additionalData = null, string? message = null) : base(statusCode, code, data, additionalData, message)
        {
        }
        public ResponseModel(int statusCode, string code, string? message) : base(statusCode, code, message)
        {
        }
    }
}
