/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the IUserServices interface, which provides functionality
 * for managing user information in the e-commerce system. It includes methods for retrieving
 * user lists, creating, updating, activating, approving, and deleting users.
 * Date Created: 2024/09/28
 */

using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.Data.Repository.Interfaces
{
    public interface IUserServices
    {
        /// <summary>
        /// Retrieves a list of all users in the system.
        /// </summary>
        /// <returns>A list of UserResponseDTO containing user details</returns>
        List<UserResponseDTO> GetUserList();

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>A UserResponseDTO containing the user's details</returns>
        UserResponseDTO GetUserById(string id);

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="userRegisterRequest">The details of the user to be registered</param>
        /// <returns>A UserResponseDTO containing the newly created user details</returns>
        UserResponseDTO CreateUser(UserRegisterRequest userRegisterRequest);

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="id">The ID of the user to be updated</param>
        /// <param name="userUpdateDTO">The updated user details</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        bool UpdateUser(string id, UserUpdateRequest userUpdateDTO);

        /// <summary>
        /// Soft-deletes a user, marking them as inactive or deleted.
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>True if the user was deleted successfully, false otherwise</returns>
        bool DeleteUser(string id);

        /// <summary>
        /// Activates a previously deactivated user, marking them as active again.
        /// </summary>
        /// <param name="id">The ID of the user to activate</param>
        /// <returns>True if the activation was successful, false otherwise</returns>
        bool ActivateUser(string id);

        /// <summary>
        /// Approves a user, marking them as an active user in the system.
        /// </summary>
        /// <param name="id">The ID of the user to approve</param>
        /// <returns>True if the approval was successful, false otherwise</returns>
        bool ApproveUser(string id);
    }
}
