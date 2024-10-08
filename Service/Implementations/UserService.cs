/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the UserService class,
 * which provides functionality to manage and retrieve user information from the database.
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

        // Constructor for the UserService class, initializes the database context and configuration
        public UserService(MongoDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // Retrieves a list of all users from the database and maps them to UserResponseDTO objects
        public List<UserResponseDTO> GetUserList()
        {
            var users = _context.Users.Find(FilterDefinition<User>.Empty).ToList();
            return users.Select(DtoMapper.ToUserResponseDTO).ToList();
        }

        public UserResponseDTO GetUserById(string id)
        {
            var user = _context.Users.Find(u => u.Id == id).FirstOrDefault();
            return user == null ? null : DtoMapper.ToUserResponseDTO(user);
        }

        public UserResponseDTO CreateUser(UserRegisterRequest userRegisterRequest)
        {
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
                IsActive = false,
            };

            _context.Users.InsertOne(user);
            return DtoMapper.ToUserResponseDTO(user);
        }

        public bool UpdateUser(string id, UserUpdateRequest userUpdateRequest)
        {
            Console.WriteLine(userUpdateRequest.Role);
            Console.WriteLine(userUpdateRequest.PhoneNumber);
            
            var updateDefinition = Builders<User>
                .Update.Set(u => u.FirstName, userUpdateRequest.FirstName)
                .Set(u => u.LastName, userUpdateRequest.LastName)
                .Set(u => u.Email, userUpdateRequest.Email)
                .Set(u => u.Role, userUpdateRequest.Role)
                .Set(u => u.PhoneNumber, userUpdateRequest.PhoneNumber)
                .Set(u => u.Address.Street, userUpdateRequest.Address.Street)
                .Set(u => u.Address.City, userUpdateRequest.Address.City)
                .Set(u => u.Address.State, userUpdateRequest.Address.State)
                .Set(u => u.Address.PostalCode, userUpdateRequest.Address.PostalCode)
                .Set(u => u.Address.Country, userUpdateRequest.Address.Country)
                .Set(u => u.UpdatedAt, DateTime.UtcNow);

            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);

            return result.ModifiedCount > 0;
        }

        public bool DeleteUser(string id)
        {
            var updateDefinition = Builders<User>.Update.Set(u => u.IsDeleted, true);
            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);
            return result.ModifiedCount > 0;
        }

        public bool ActivateUser(string id)
        {
            var updateDefinition = Builders<User>.Update.Set(u => u.IsDeleted, false);
            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);
            return result.ModifiedCount > 0;
        }

        public bool ApproveUser(string id)
        {
            var updateDefinition = Builders<User>.Update.Set(u => u.IsActive, true);
            var result = _context.Users.UpdateOne(u => u.Id == id, updateDefinition);
            return result.ModifiedCount > 0;
        }
    }
}
