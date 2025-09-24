﻿using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IUserService
{
    public Task<int> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber);
    public Task<User?> GetUserByEmailAsync(string email);
}