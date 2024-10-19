using System.Threading.Tasks;
using ECommerceBackend.Helpers.utills;

namespace ECommerceBackend.Service.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(string token, string title, string body);
}
