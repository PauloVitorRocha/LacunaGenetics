//IMPORTS
class User
{
    ValidationApi validator = new ValidationApi();

    private string username = "", email = "", password = "", accessToken = "";

    public string Username
    {
        get { return username; }
        set { username = value; }
    }

    public string Email
    {
        get { return email; }
        set { email = value; }
    }

    public string Password
    {
        get { return password; }
        set { password = value; }
    }

    public string AccessToken
    {
        get { return accessToken; }
        set { accessToken = value; }
    }

    public int validateUsername()
    {
        while (true)
        {
            Console.WriteLine("username: ");
            string? usn = Console.ReadLine();
            if (usn == null)
            {
                usn = "";
            }
            username = usn;

            int usernameValid = validator.usernameValidator(username);
            if (usernameValid == -1)
            {
                Console.WriteLine("\nNome de usuário não pode conter espaços\n");
                continue;
            }
            else if (usernameValid == -2)
            {
                Console.WriteLine("\nNome possui caractere inválido\n");
                continue;
            }
            else if (usernameValid == -3)
            {
                Console.WriteLine("\nNome tem tamanho inválido\n");
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
            string? eml = Console.ReadLine();
            if (eml == null)
            {
                eml = "";
            }
            email = eml;
            int emailValid = validator.emailValidator(email);
            if (emailValid == -1)
            {
                Console.WriteLine("\nEmail inválido, digite um email no formato exemplo@exemplo.abc\n");
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
            string? psw = Console.ReadLine();
            if (psw == null)
            {
                psw = "";
            }
            password = psw;
            bool passwordValid = validator.passwordValidator(password);
            if (!passwordValid)
            {
                Console.WriteLine("\nA senha tem que possuir pelo menos 8 caracteres\n");
                continue;
            }
            break;
        }
        return 1;
    }
    public async Task register()
    {
        Console.WriteLine("\nNovo Cadastro\n");

        validateUsername();
        validateEmail();
        validatePassword();

        var body = new Dictionary<string, string>{
                {"username", username},
                {"email", email},
                {"password", password}
            };
        AsyncFunctions req = new AsyncFunctions();
        var responseDict = await req.makeAsyncRequest("https://gene.lacuna.cc/api/users/create", body, "application/json");
        if (responseDict.code != "Success")
        {
            Console.WriteLine("\nErro ao criar conta");
            Console.WriteLine("Mensagem: {0}\n", responseDict.message);
            await register();

        }
        else
        {
            Console.WriteLine("\nConta criada com sucesso");
        }
    }
    public async Task login()
    {
        Console.WriteLine("\nLogin\n");

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
            Console.WriteLine("\nUsuário ou senha incorreta");
            await login();
            return;
        }
        this.accessToken = responseDict.accessToken;
        Console.WriteLine("\nLogado com sucesso!");
    }

    public async Task relogin()
    {
        var body = new Dictionary<string, string>{
                {"username", username},
                {"password", password}
            };
        AsyncFunctions req = new AsyncFunctions();
        var responseDict = await req.makeAsyncRequest("https://gene.lacuna.cc/api/users/login", body, "application/json");
        this.accessToken = responseDict.accessToken;
        Console.WriteLine("Conexão estabelecida com sucesso!");
    }
}