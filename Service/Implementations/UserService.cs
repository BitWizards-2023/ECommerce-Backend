using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.DTOs.Response;
using ECommerceBackend.Helpers;
using ECommerceBackend.Models;
using MongoDB.Driver;

namespace ECommerceBackend.Data.Repository.Implementations;

public class UserService : IUserServices
{
    private readonly MongoDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(MongoDbContext context, IConfiguration configuration)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public List<UserResponseDTO> GetUserList()
    {
        var users = _context.Users.Find(FilterDefinition<User>.Empty).ToList();
        return users.Select(DtoMapper.ToUserResponseDTO).ToList();
    }
}
