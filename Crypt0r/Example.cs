using System;
using Crypt0r;

namespace Example
{
	public class ExampleProgram
	{
		public static void Main(string[] args)
		{
			Crypt0rV3 c = new Crypt0rV3(); // This will generate random Key & IV
			c.SetInRegistry(); // If you want to enable decryption, DON'T FORGET THIS!
			// OTHERWISE YOUR FILE(s) ARE GONE!
			
			// If you want to use your own key & iv, do this:
			Crypt0rV3 c_ = new Crypt0rV3(yourKey, yourIV); // yourKey should be 32 characters long.
			// yourIV should be 16 characters long. If not, it'll be random.
			
			///<. Now to encrypt:
			
			string path = "C:\\YourPath\\YourFile.fileextension";
			byte[] fileBytes = File.ReadAllBytes(path);
			File.WriteAllBytes(path, c.EncodeBytes(fileBytes));
			
			///<. Decryption:
			
			byte[] fileBytesDecode = File.ReadAllBytes(path);
			File.WriteAllBytes(path, c.DecodeBytes(fileBytesDecode));
			
			// READ ME!
			/*
			
				After decryption make sure you execute the Destroy(); method. Example:
				Crypt0rV3 c = new Crypt0rV3();
				bla bla bla...
				c.Destroy();
				
				But only after decryption. If you call it before, then you won't be able
				to recover your file unless you stored the key before.
				If you don't call it, the GITGUD registry key will be on your HKEY_LOCAL_MACHINE
				and it can affect future uses.
				
				Things such as always remembering the key & iv is up to you, using the namespace
				Microsoft.Win32 and the class RegistryKey.
				
				Example:
				RegistryKey rk = Registry.CurrentUser.OpenSubKey("GITGUD");
				string key_ = rk.GetValue("Key");
				string iv_ = rk.GetValue("IV");
				
				You can modify the Crypt0r.cs anytime you want, but be sure to build it again and
				credit the developer (me) :)
				Have a good day, my friend.
				
			*/
		}
	}
}