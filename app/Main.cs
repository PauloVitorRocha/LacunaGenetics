public class Program
{
    public static async Task Main()
    {

        string? hasAccount = "";
        bool invalid = false;
        while (hasAccount.CompareTo("s") != 0 && hasAccount.CompareTo("n") != 0)
        {
            if (invalid)
                Console.WriteLine("\nOpção inválida, apenas 's' ou 'n' são aceitas");
            invalid = true;
            Console.WriteLine("\nJá possui conta? (s/n)");
            hasAccount = Console.ReadLine();
            if (hasAccount is null)
            {
                hasAccount = "";
            }

        }
        User user = new User();
        if (hasAccount.CompareTo("n") == 0)
        {
            await user.register();
        }
        await user.login();
        while (user.AccessToken == "")
        {
            await user.login();
        }
        Console.WriteLine("\n############ Menu ############");
        Console.WriteLine("1 - Pega um novo Job");
        Console.WriteLine("0 - Sair");
        int optNumber;
        while (true)
        {
            string? option = Console.ReadLine();
            Int32.TryParse(option, out optNumber);
            switch (optNumber)
            {
                case 1:
                    AsyncFunctions req = new AsyncFunctions();
                    JSONModel.ResponseObject jobObject = await req.asyncGet("https://gene.lacuna.cc/api/dna/jobs", user.AccessToken);
                    int cntRetries=0;
                    while (jobObject.code == "Unauthorized")
                    {
                        if(cntRetries == 3){
                            Console.WriteLine("Não foi possivel reconectar, encerrando aplicação...");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Não autorizado, tentando reconexão...");
                        await user.relogin();
                        cntRetries ++;
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
                    Console.WriteLine("\nEncerrando...\n");
                    Environment.Exit(0);
                    break;
            }
        }

    }
}