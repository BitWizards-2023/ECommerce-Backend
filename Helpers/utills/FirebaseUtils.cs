using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace ECommerceBackend.Helpers.utills
{
    public class FirebaseUtils
    {
        private readonly string _firebaseServiceAccountPath;

        public FirebaseUtils(IConfiguration configuration)
        {
            _firebaseServiceAccountPath = configuration["FirebaseSettings:ServiceAccountKeyPath"];

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(
                    new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(_firebaseServiceAccountPath),
                    }
                );
            }
        }

        public async Task<string> SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification { Title = title, Body = body },
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return response; // Firebase response
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error sending notification", ex);
            }
        }
    }
}
