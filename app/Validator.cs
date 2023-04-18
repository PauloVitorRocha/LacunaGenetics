using System;
using System.Text.RegularExpressions;


class ValidationApi
{

    public int usernameValidator(string username)
    {
        // Define a regular expression for .
        Regex rx = new Regex(@"\b[0-9a-zA-Z]+\b",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Find matches.
        MatchCollection matches = rx.Matches(username);
        if (matches.Count > 1)
        {
            return -1;
        }
        if (matches.Count == 0)
        {

            return -2;
        }
        if(username.Length < 4 || username.Length > 32){
            return -3;
        }
        return 0;
    }

    public int emailValidator(string email)
    {
        // Define a regular expression for .
        Regex rx = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Find matches.
        MatchCollection matches = rx.Matches(email);

        if (matches.Count == 0)
        {
            return -1;
        }
        return 0;
    }

    public bool passwordValidator(string password)
    {
        return password.Length >= 8;
    }

}