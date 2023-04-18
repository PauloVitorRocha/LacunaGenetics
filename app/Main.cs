public class Program
{
    public static async Task Main()
    {

        string? hasAccount = "";
        bool invalid = false;
        while (hasAccount.CompareTo("y") != 0 && hasAccount.CompareTo("n") != 0)
        {
            Console.Clear();
            if (invalid)
                Console.WriteLine("\nInvalid option, only 'y' or 'n' is accepted");
            invalid = true;
            Console.WriteLine("\n############ Menu ############");
            Console.WriteLine("\nAlready have an account? (y/n)");

            hasAccount = Console.ReadLine();
            if (hasAccount is null)
            {
                hasAccount = "";
            }

        }
        User user = new User();
        Console.Clear();
        if (hasAccount.CompareTo("n") == 0)
        {
            await user.register();
        }
        await user.login();
        while (user.AccessToken == "")
        {
            await user.login();
        }
        int optNumber;
        while (true)
        {
            Console.WriteLine("\n############ Menu ############");
            Console.WriteLine("1 - Get new job");
            Console.WriteLine("0 - Exit");

            string? option = Console.ReadLine();
            Int32.TryParse(option, out optNumber);
            switch (optNumber)
            {
                case 1:
                    Console.Clear();
                    AsyncFunctions req = new AsyncFunctions();
                    JSONModel.ResponseObject jobObject = await req.asyncGet("https://gene.lacuna.cc/api/dna/jobs", user.AccessToken);
                    int cntRetries = 0;
                    while (jobObject.code == "Unauthorized")
                    {
                        if (cntRetries == 3)
                        {
                            Console.WriteLine("Could't reconnect, shutting down application...");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Not authorized, trying to reconnect...");
                        await user.relogin();
                        cntRetries++;
                        jobObject = await req.asyncGet("https://gene.lacuna.cc/api/dna/jobs", user.AccessToken);
                    }

                    Job jobData = new Job();
                    jobData = jobObject.job;

                    Console.WriteLine("Job Type = {0}", jobData.type);

                    Job jobInstance = new Job();

                    if (jobData.type == "DecodeStrand")
                    {
                        string decodedStrand = jobInstance.decodeStrand(jobData.strandEncoded);

                        //prepara o request
                        var body = new Dictionary<string, string>{
                            {"strand", decodedStrand}
                        };
                        AsyncFunctions request = new AsyncFunctions();
                        string url = "https://gene.lacuna.cc/api/dna/jobs/" + jobData.id + "/decode";

                        var responseDict = await request.makeAsyncRequest(url, body, "application/json", user.AccessToken);

                        Console.WriteLine("Decode Response = {0}", responseDict.code);
                    }

                    else if (jobData.type == "EncodeStrand")
                    {
                        string strandEncoded = jobInstance.encodeStrand(jobData.strand);
                        var body = new Dictionary<string, string>{
                            {"strandEncoded", strandEncoded}
                        };
                        AsyncFunctions request = new AsyncFunctions();
                        string url = "https://gene.lacuna.cc/api/dna/jobs/" + jobData.id + "/encode";

                        var responseDict = await request.makeAsyncRequest(url, body, "application/json", user.AccessToken);
                        Console.WriteLine("Encode Response = {0}", responseDict.code);
                    }

                    else if (jobData.type == "CheckGene")
                    {
                        bool isActive = jobInstance.checkGene(jobData.strandEncoded, jobData.geneEncoded);
                        var body = new Dictionary<string, bool>{
                            {"isActivated", isActive}
                        };
                        AsyncFunctions request = new AsyncFunctions();
                        string url = "https://gene.lacuna.cc/api/dna/jobs/" + jobData.id + "/gene";

                        var responseDict = await request.makeAsyncRequestBool(url, body, "application/json", user.AccessToken);

                        Console.WriteLine("Check Gene Response = {0}", responseDict.code);
                    }

                    break;
                case 0:
                    Console.WriteLine("\nShutting down...\n");
                    Environment.Exit(0);
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("\nInvalid option");
                    break;

            }
        }

    }
}