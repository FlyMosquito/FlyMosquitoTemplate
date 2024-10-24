using System.Net;

namespace FlyMosquito.Domain
{
    /// <summary>
    /// 通用 API 返回结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class ApiResult<T>
    {
        /// <summary>
        /// 状态码（如 200 表示成功，其他值表示错误）
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// 返回的信息（通常在错误时使用）
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 具体的数据内容
        /// </summary>
        public T Data { get; }

        private ApiResult(int statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <param name="message">返回的信息</param>
        /// <returns>封装的成功 ApiResult 对象</returns>
        public static ApiResult<T> Success(T data = default, string message = "Success")
        {
            return new ApiResult<T>((int)HttpStatusCode.OK, message, data);
        }

        /// <summary>
        /// 失败返回
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="statusCode">状态码</param>
        /// <returns>封装的失败 ApiResult 对象</returns>
        public static ApiResult<T> Fail(string message = "Failed", HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new ApiResult<T>((int)statusCode, message, default);
        }

        /// <summary>
        /// 400 Bad Request
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 BadRequest ApiResult 对象</returns>
        public static ApiResult<T> BadRequest(string message = "Bad Request")
        {
            return Fail(message, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// 404 Not Found
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 NotFound ApiResult 对象</returns>
        public static ApiResult<T> NotFound(string message = "Not Found")
        {
            return Fail(message, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// 401 Unauthorized
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 Unauthorized ApiResult 对象</returns>
        public static ApiResult<T> Unauthorized(string message = "Unauthorized")
        {
            return Fail(message, HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 403 Forbidden
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 Forbidden ApiResult 对象</returns>
        public static ApiResult<T> Forbidden(string message = "Forbidden")
        {
            return Fail(message, HttpStatusCode.Forbidden);
        }
    }

    /// <summary>
    /// 非泛型的 ApiResult，用于不需要返回数据的场景
    /// </summary>
    public sealed class ApiResult
    {
        /// <summary>
        /// 状态码（如 200 表示成功，其他值表示错误）
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// 返回的信息（通常在错误时使用）
        /// </summary>
        public string Message { get; }

        private ApiResult(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="message">返回的信息</param>
        /// <returns>封装的成功 ApiResult 对象</returns>
        public static ApiResult Success(string message = "Success")
        {
            return new ApiResult((int)HttpStatusCode.OK, message);
        }

        /// <summary>
        /// 失败返回
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="statusCode">状态码</param>
        /// <returns>封装的失败 ApiResult 对象</returns>
        public static ApiResult Fail(string message = "Failed", HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new ApiResult((int)statusCode, message);
        }

        /// <summary>
        /// 400 Bad Request
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 BadRequest ApiResult 对象</returns>
        public static ApiResult BadRequest(string message = "Bad Request")
        {
            return Fail(message, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// 404 Not Found
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 NotFound ApiResult 对象</returns>
        public static ApiResult NotFound(string message = "Not Found")
        {
            return Fail(message, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// 401 Unauthorized
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 Unauthorized ApiResult 对象</returns>
        public static ApiResult Unauthorized(string message = "Unauthorized")
        {
            return Fail(message, HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// 403 Forbidden
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>封装的 Forbidden ApiResult 对象</returns>
        public static ApiResult Forbidden(string message = "Forbidden")
        {
            return Fail(message, HttpStatusCode.Forbidden);
        }
    }
}
