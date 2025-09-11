using System;

namespace EpersBackend.Services.Users
{
	public interface IPasswordService
	{
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        string CreateRandomToken();
    }
}

