using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public class LoginRequest
	{
		public string email { get; set; }
		public string password { get; set; }

	}


	public class RefreshTokenRequest
	{
		public string accessToken { get; set; }
		public string refreshToken { get; set; }

	}

	public class AccessTokenResponse
	{
		public string token { get; }
		public int expiresIn { get; }

		public AccessTokenResponse(string token, int expiresIn)
		{
			this.token = token;
			this.expiresIn = expiresIn;
		}
	}

	public class AuthResponse
	{
		public AccessTokenResponse accessToken { get; set; }
		public string refreshToken { get; set; }

	}
}
