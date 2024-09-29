/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the UserService class,
 * which provides functionality to manage and retrieve user information from the database.
 * Date Created: 2024/09/28
 */

using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.DTOs.Response;
using ECommerceBackend.Helpers;
using ECommerceBackend.Models;
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
    }
}
