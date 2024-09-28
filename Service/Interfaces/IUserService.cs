using System;
using ECommerceBackend.Models;

namespace ECommerceBackend.Data.Repository.Interfaces;

public interface IUserServices
{
    
        List<User> GetUserList();
}
