using System;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


public class Test
{
    public static async Task Main()
    {

        Console.WriteLine("\nJá possui conta? (s/n)");

        string? hasAccount = Console.ReadLine();
        User user = new User();
        if (hasAccount == "n")
        {
            await user.register();
        }
        await user.login();
        while (user.AccessToken == "")
        {
            await user.login();
        }

        AsyncFunctions req = new AsyncFunctions();
        JSONModel.ResponseObject jobData = await req.asyncGet("https://gene.lacuna.cc/api/dna/jobs", user.AccessToken);
        Job jobObject = new Job();
        jobObject = jobData.job;

        Console.WriteLine("Job Type = {0}", jobObject.type);

        Job jobInstance = new Job();

        if (jobObject.type == "DecodeStrand")
        {
            string decodedStrand = jobInstance.decodeStrand(jobObject.strandEncoded);

            //prepara o request
            var body = new Dictionary<string, string>{
                {"strand", decodedStrand}
            };
            AsyncFunctions request = new AsyncFunctions();
            string url = "https://gene.lacuna.cc/api/dna/jobs/" + jobObject.id + "/decode";

            var responseDict = await request.makeAsyncRequest(url, body, "application/json", user.AccessToken);
            Console.WriteLine("DECODE RESPONSE = {0}", responseDict.code);

        }
        else if (jobObject.type == "EncodeStrand")
        {
            await jobInstance.encodeStrand(jobObject.strand, jobObject.id, user.AccessToken);
        }
        else if (jobObject.type == "CheckGene")
        {
            await jobInstance.checkGene(jobObject.strandEncoded, jobObject.geneEncoded, jobObject.id, user.AccessToken);
        }
    }
}