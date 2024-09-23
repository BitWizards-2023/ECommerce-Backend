using System;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.Models;
using MongoDB.Driver;

namespace ECommerceBackend.Data.Repository.Implementations;

public class UserService:IUserServices 
{
    
    private readonly MongoDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(MongoDbContext context, IConfiguration configuration)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
      public List<User> GetUserList()
        {
            return _context.Users.Find(u => !u.IsDeleted).ToList();
        }
}
