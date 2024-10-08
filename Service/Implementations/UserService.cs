/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the UserService class,
 * which provides functionality to manage and retrieve user information from the database.
 * This includes methods for creating, updating, activating, deleting, and approving users.
 * Date Created: 2024/09/28
 */

using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.Helpers;
using ECommerceBackend.Models;
using ECommerceBackend.Utilities;
using MongoDB.Driver;

namespace ECommerceBackend.Data.Repository.Implementations
{
    public class UserService : IUserServices
    {
        private readonly MongoDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for the UserService class, initializes the MongoDB context and configuration.
        /// </summary>
        /// <param name="context">The MongoDB context</param>
        /// <param name="configuration">The application configuration</param>
        public UserService(MongoDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Retrieves a list of all users from the database and maps them to UserResponseDTO objects.
        /// </summary>
        public List<UserResponseDTO> GetUserList()
        {
            // Retrieve all users from the MongoDB collection and map them to DTOs
            var users = _context.Users.Find(FilterDefinition<User>.Empty).ToList();
            return users.Select(DtoMapper.ToUserResponseDTO).ToList();
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>UserResponseDTO for the retrieved user or null if not found</returns>
        public UserResponseDTO GetUserById(string id)
        {
            // Find the user by ID in the MongoDB collection
            var user = _context.Users.Find(u => u.Id == id).FirstOrDefault();
            return user == null ? null : DtoMapper.ToUserResponseDTO(user);
        }

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="userRegisterRequest">The user registration details</param>
        /// <returns>The newly created UserResponseDTO</returns>
        public UserResponseDTO CreateUser(UserRegisterRequest userRegisterRequest)
        {
            // Hash the user's password and map the registration request to the user model
            var user = new User
            {
                Username = userRegisterRequest.Username,
                Email = userRegisterRequest.Email,
                PasswordHash = PasswordHasher.HashPassword(userRegisterRequest.Password),
                FirstName = userRegisterRequest.FirstName,
                LastName = userRegisterRequest.LastName,
                PhoneNumber = userRegisterRequest.PhoneNumber,
                Address = new Address
                {
                    Street = userRegisterRequest.Address.Street,
                    City = userRegisterRequest.Address.City,
                    State = userRegisterRequest.Address.State,
                    PostalCode = userRegisterRequest.Address.PostalCode,
                    Country = userRegisterRequest.Address.Country,
                    IsDeleted = false,
                },
                Role = userRegisterRequest.Role,
                ProfilePic =
                    userRegisterRequest.Profile_pic
                    ?? "https://shopilystorage.blob.core.windows.net/mycontainer/cd4e13a7-7ce1-4c63-beb4-4e44cdfa10f0.png",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsActive = false, // Initially set the user as inactive
            };

            // Insert the new user into the MongoDB collection
            _context.Users.InsertOne(user);
            return DtoMapper.ToUserResponseDTO(user);
        }

        /// <summary>
        /// Updates the details of an existing user by their ID.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="userUpdateRequest">The updated user details</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        public bool UpdateUser(string id, UserUpdateRequest userUpdateRequest)
        {
            // Build the update definition for the user
            var updateDefinition = Builders<User>
                .Update.Set(u => u.FirstName, userUpdateRequest.FirstName)
                .Set(u => u.LastName, userUpdateRequest.LastName)
                .Set(u => u.Email, userUpdateRequest.Email)
                .Set(u => u.PhoneNumber, userUpdateRequest.PhoneNumber)
                .Set(u => u.Address.Street, userUpdateRequest.Address.Street)
                .Set(u => u.Address.City, userUpdateRequest.Address.City)
                .Set(u => u.Address.State, userUpdateRequest.Address.State)
                .Set(u => u.Address.PostalCode, userUpdateRequest.Address.PostalCode)
                .Set(u => u.Address.Country, userUpdateRequest.Address.Country)
                .Set(u => u.UpdatedAt, DateTime.UtcNow); // Update the timestamp

            // Execute the update in the MongoDB collection
            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);

            return result.ModifiedCount > 0; // Return true if the update was successful
        }

        /// <summary>
        /// Soft-deletes a user by setting the IsDeleted flag to true.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>True if the deletion was successful, false otherwise</returns>
        public bool DeleteUser(string id)
        {
            // Soft-delete the user by setting IsDeleted to true
            var updateDefinition = Builders<User>.Update.Set(u => u.IsDeleted, true);
            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);
            return result.ModifiedCount > 0; // Return true if the deletion was successful
        }

        /// <summary>
        /// Activates a user by setting the IsDeleted flag to false.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>True if the activation was successful, false otherwise</returns>
        public bool ActivateUser(string id)
        {
            // Activate the user by setting IsDeleted to false
            var updateDefinition = Builders<User>.Update.Set(u => u.IsDeleted, false);
            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);
            return result.ModifiedCount > 0; // Return true if the activation was successful
        }

        /// <summary>
        /// Approves a user by setting the IsActive flag to true.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>True if the approval was successful, false otherwise</returns>
        public bool ApproveUser(string id)
        {
            // Approve the user by setting IsActive to true
            var updateDefinition = Builders<User>.Update.Set(u => u.IsActive, true);
            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);
            return result.ModifiedCount > 0; // Return true if the approval was successful
        }
    }
}
