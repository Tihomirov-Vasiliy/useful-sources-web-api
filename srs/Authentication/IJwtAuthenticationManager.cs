namespace Authentication
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string userLogin,string userPassword);
    }
}
