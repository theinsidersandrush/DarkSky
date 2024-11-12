using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace DarkSky.Helpers
{
	// Temporary helper to store credentials
	// ASSUMES ONLY A SINGLE CREDENTIAL
	public class CredentialHelper
	{
		public static PasswordVault vault = new PasswordVault();
		// ASSUMES ONLY ONE CREDENTIAL STORED
		public static PasswordCredential RetrieveCredential()
		{
			var credentialList = vault.RetrieveAll();
			var credential = credentialList[0];
			credential.RetrievePassword();
			return credential;
		}

		public static int Count()
		{	
			var credentialList = vault.RetrieveAll();
			return credentialList.Count;
		}

		public static void SaveCredential(string username, string password, string token)
		{
			// Store the credentials with token as the resource, username as the user name, and password as the password
			var credential = new PasswordCredential(token, username, password);

			// Add the credential to the vault
			vault.Add(credential);
		}

		public static void DeleteAllCredentials()
		{
			// Retrieve all credentials stored in the vault
			var credentialList = vault.RetrieveAll();

			// Delete each credential
			foreach (var credential in credentialList)
			{
				vault.Remove(credential);
			}
		}
	}
}
