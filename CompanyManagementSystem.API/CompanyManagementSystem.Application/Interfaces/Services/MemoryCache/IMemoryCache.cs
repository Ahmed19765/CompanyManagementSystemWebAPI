using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Services.MemoryCache
{
    public interface IMemoryCache<T>
    {
        // Using Example : cache.Save("ahmed@gmail.com", "847291", TypeOfValue.OTP, TimeSpan.FromMinutes(5));
        void Save (string key, T value, TypeOfValue TypeV, TimeSpan expiration);
        bool Validate(string key, T value, TypeOfValue TypeV); // This method will check if the key exists and if the value matches the stored value for the given type
        void Remove(string key, TypeOfValue TypeV);
        void Cleaner(); // This method will be called in the background to clean expired cache entries
    }

    public enum TypeOfValue
    {
        OTP,
        EmailVerificationOtp,
        PasswordResetOtp
    }
}
