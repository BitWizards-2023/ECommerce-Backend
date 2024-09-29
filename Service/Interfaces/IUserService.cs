using System;
using ECommerceBackend.DTOs.Response;
using ECommerceBackend.Models;

namespace ECommerceBackend.Data.Repository.Interfaces;

public interface IUserServices
{
    List<UserResponseDTO> GetUserList();
}
