﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebApplication6
{
    internal sealed class TokenFactory : ITokenFactory
    {
        public string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }

    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
