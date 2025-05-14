namespace TodoWeb.Appllication.Helper;

public static class HashHelper
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public static string GenerateRamdomString(int length)
    {
        string s = "";
        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            s += random.Next(1, 255);
        }
        return s.ToString();
    }
}