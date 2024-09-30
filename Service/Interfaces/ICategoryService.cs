using System;
using ECommerceBackend.DTOs.Request.Category;
using ECommerceBackend.DTOs.Response.Category;

namespace ECommerceBackend.Service.Interfaces;

public interface ICategoryService
{
    List<CategoryResponseDTO> GetCategoryTree();
    CategoryResponseDTO GetCategoryById(string id);
    CategoryResponseDTO CreateCategory(CategoryRequestDTO categoryRequestDTO);
    bool UpdateCategory(string id, CategoryRequestDTO categoryRequestDTO);
    bool DeleteCategory(string id);
    List<CategoryResponseDTO> GetChildCategories(string parentId);
}
