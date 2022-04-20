﻿using System;
using System.Security.Cryptography;
using OneClickSolutions.Infrastructure.Dependency;

namespace OneClickSolutions.Infrastructure.Cryptography
{
    public interface IRandomNumber
    {
        int Next();
        int Next(int max);
        int Next(int min, int max);
    }

    internal sealed class RandomNumber : IRandomNumber, ISingletonDependency
    {
        private readonly RandomNumberGenerator _rand = RandomNumberGenerator.Create();

        public int Next()
        {
            var randBytes = new byte[4];
            _rand.GetBytes(randBytes);
            var value = BitConverter.ToInt32(randBytes, 0);
            if (value < 0) value = -value;
            return value;
        }

        public int Next(int max)
        {
            var randBytes = new byte[4];
            _rand.GetBytes(randBytes);
            var value = BitConverter.ToInt32(randBytes, 0);
            value = value % (max + 1); // % calculates remainder
            if (value < 0) value = -value;
            return value;
        }

        public int Next(int min, int max)
        {
            var value = Next(max - min) + min;
            return value;
        }
    }
}