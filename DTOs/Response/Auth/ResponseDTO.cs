/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the generic ResponseDTO, which is used to standardize
 * API responses by including a success flag, message, and data of type T.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Response.Auth
{
    /// <summary>
    /// Represents a standardized response DTO that contains a success flag, message, and data of type T.
    /// </summary>
    /// <typeparam name="T">The type of the data included in the response.</typeparam>
    public class ResponseDTO<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the request was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message providing additional information about the request.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the data returned in the response.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseDTO{T}"/> class.
        /// </summary>
        public ResponseDTO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseDTO{T}"/> class with the specified success flag, message, and data.
        /// </summary>
        /// <param name="success">Indicates whether the request was successful.</param>
        /// <param name="message">Provides additional information about the request.</param>
        /// <param name="data">The data returned in the response.</param>
        public ResponseDTO(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
