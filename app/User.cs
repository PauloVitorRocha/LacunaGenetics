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
                Console.WriteLine("\nUsername cannot contain whitespaces\n");
                continue;
            }
            else if (usernameValid == -2)
            {
                Console.WriteLine("\nUsername contains invalid characters\n");
                continue;
            }
            else if (usernameValid == -3)
            {
                Console.WriteLine("\nUsername has wrong size\n");
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
                Console.WriteLine("\nInvalid email, accepted format is example@example.abc\n");
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
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            password = pass;
            bool passwordValid = validator.passwordValidator(pass);
            if (!passwordValid)
            {
                Console.WriteLine("\nThe password must be 8 characters long\n");
                continue;
            }
            break;
        }
        return 1;
    }
    public async Task register()
    {
        Console.WriteLine("\nRegistration\n");

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
            Console.WriteLine("\nError on creating account");
            Console.WriteLine("Message: {0}\n", responseDict.message);
            await register();

        }
        else
        {
            Console.Clear();
            Console.WriteLine("\nAccount created successfully");
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
            Console.Clear();
            Console.WriteLine("\nIncorrect username or password");
            await login();
            return;
        }
        this.accessToken = responseDict.accessToken;
        Console.Clear();
        Console.WriteLine("\nLogged in!");
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
        Console.WriteLine("Connection successfully established!");
    }
}