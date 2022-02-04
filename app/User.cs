//IMPORTS

using System;
using System.Text.Json;
using System.Text.RegularExpressions;
class User
{
    ValidationApi validator = new ValidationApi();

    private string username = "", email = "", password = "", accessToken = "";

    public string Username
    {
        get { return username; }   // get method
        set { username = value; }  // set method
    }

    public string Email
    {
        get { return email; }   // get method
        set { email = value; }  // set method
    }

    public string Password
    {
        get { return password; }   // get method
        set { password = value; }  // set method
    }

    public string AccessToken
    {
        get { return accessToken; }   // get method
        set { accessToken = value; }  // set method
    }

    public int validateUsername()
    {
        while (true)
        {
            Console.WriteLine("username: ");
            username = Console.ReadLine();
            int usernameValid = validator.usernameValidator(username);
            if (usernameValid == -1)
            {
                Console.WriteLine("Nome de usuário não pode conter espaços\n");
                continue;
            }
            else if (usernameValid == -2)
            {
                Console.WriteLine("Nome possui caractere inválido\n");
                continue;
            }
            else if (usernameValid == -3)
            {
                Console.WriteLine("Nome tem tamanho inválido\n");
                continue;
            }

            break;
        }
        return 1;
    }
    public int validateEmail()
    {
        while (true)
        {
            Console.WriteLine("email: ");
            email = Console.ReadLine();
            int emailValid = validator.emailValidator(email);
            if (emailValid == -1)
            {
                Console.WriteLine("Email inválido, digite um email no formato exemplo@exemplo.abc\n");
                continue;
            }

            else
            {
                break;
            }
        }
        return 1;
    }
    public int validatePassword()
    {
        while (true)
        {
            Console.WriteLine("password: ");
            password = Console.ReadLine();
            bool passwordValid = validator.passwordValidator(password);
            if (!passwordValid)
            {
                Console.WriteLine("A senha tem que possuir pelo menos 8 caracteres\n");
                continue;
            }
            break;
        }
        return 1;
    }
    public async Task register()
    {
        validateUsername();
        validateEmail();
        validatePassword();

        var body = new Dictionary<string, string>{
                {"username", username},
                {"email", email},
                {"password", password}
            };
        Console.WriteLine("IMH ERE{0}", username);
        AsyncFunctions req = new AsyncFunctions();
        var responseDict = await req.makeAsyncRequest("https://gene.lacuna.cc/api/users/create", body, "application/json");
        if (responseDict.code != "Success")
        {
            Console.WriteLine("Erro ao criar conta");
        }
        Console.WriteLine("Conta criada com sucesso");
    }
    public async Task login()
    {

        validateUsername();
        validatePassword();

        var body = new Dictionary<string, string>{
                {"username", username},
                {"password", password}
            };
        AsyncFunctions req = new AsyncFunctions();
        var responseDict = await req.makeAsyncRequest("https://gene.lacuna.cc/api/users/login", body, "application/json");
        if (responseDict.code != "Success")
        {
            Console.WriteLine("Erro ao realizar login");
            await login();
            return;
        }
        this.accessToken = responseDict.accessToken;
        Console.WriteLine("Logado!");
    }
}