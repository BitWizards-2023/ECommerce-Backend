using System;
using ECommerceBackend.Helpers.utills;
using ECommerceBackend.Service.Interfaces;

namespace ECommerceBackend.Service.Implementations;

public class NotificationService : INotificationService
{
    private readonly FirebaseUtils _firebaseUtils;

    public NotificationService(FirebaseUtils firebaseUtils)
    {
        _firebaseUtils = firebaseUtils;
    }

    public async Task SendNotificationAsync(string token, string title, string body)
    {
        await _firebaseUtils.SendNotificationAsync(token, title, body);
    }
}
