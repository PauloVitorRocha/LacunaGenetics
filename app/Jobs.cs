using System.Text;

public class Job
{
    public string id { get; set; } = "";
    public string type { get; set; } = "";
    public string strand { get; set; } = "";
    public string strandEncoded { get; set; } = "";
    public string geneEncoded { get; set; } = "";


    public async Task encodeStrand(string jobStrand, string jobId, string userAccessToken)
    {
        string bits = Converter.stringToBits(jobStrand);
        // Console.WriteLine("Bits = {0}", bits);

        var byteArray = Enumerable.Range(0, int.MaxValue / 8)
                          .Select(i => i * 8)
                          .TakeWhile(i => i < bits.Length)
                          .Select(i => bits.Substring(i, 8))
                          .Select(s => Convert.ToByte(s, 2))
                          .ToArray();
        // Console.WriteLine("BYtes = {0}", byteArray[0]);

        string strandEncoded = System.Convert.ToBase64String(byteArray);

        var body = new Dictionary<string, string>{
                {"strandEncoded", strandEncoded}
            };
        AsyncFunctions request = new AsyncFunctions();
        string url = "https://gene.lacuna.cc/api/dna/jobs/" + jobId + "/encode";

        var responseDict = await request.makeAsyncRequest(url, body, "application/json", userAccessToken);
        Console.WriteLine("ENCODE RESPONSE = {0}", responseDict.code);
    }


    public string decodeStrand(string jobEncodedStrand)
    {
        // Console.WriteLine("Base64 = {0}", jobEncodedStrand);
        byte[] byteArray = System.Convert.FromBase64String(jobEncodedStrand);
        // Console.WriteLine("BYTE ARRAY = {0}", byteArray);

        string[] byteString = byteArray.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray();
        StringBuilder bits = new StringBuilder();
        foreach (var bytes in byteString)
        {
            bits.Append(bytes);
        }
        // Console.WriteLine("bits = {0}", bits);

        string decodedStrand = Converter.bitsToString(bits.ToString());

        return decodedStrand;
    }


    public async Task checkGene(string jobEncodedStrand, string geneEncodedStrand, string jobId, string userAccessToken)
    {
        string decodedJobStrand = decodeStrand(jobEncodedStrand);
        if (decodedJobStrand[0] == 'G')
        {
            decodedJobStrand = Converter.getTemplateStrand(decodedJobStrand);
        }
        string decodedGeneStrand = decodeStrand(geneEncodedStrand);

        int geneSize = decodedGeneStrand.Length;
        int halfGeneSize = geneSize / 2;
        if (geneSize % 2 != 0)
        {
            halfGeneSize += 1;
        }
        //monta string de metade do tamanho
        StringBuilder stringToBeChecked = new StringBuilder();
        for (int i = 0; i < halfGeneSize; i++)
        {
            stringToBeChecked.Append(decodedGeneStrand[i]);
        }

        bool isActive = false;
        for (int i = 0; i < halfGeneSize; i++)
        {
            isActive = decodedJobStrand.Contains(stringToBeChecked.ToString());

            if (isActive)
                break;
            if (geneSize == (halfGeneSize + i))
                break;

            stringToBeChecked.Remove(0, 1);
            stringToBeChecked.Append(decodedGeneStrand[halfGeneSize + i]);
        }
        Console.WriteLine("ATIVO = {0}", isActive);

        var body = new Dictionary<string, bool>{
            {"isActivated", isActive}
        };
        AsyncFunctions request = new AsyncFunctions();
        string url = "https://gene.lacuna.cc/api/dna/jobs/" + jobId + "/gene";

        var responseDict = await request.makeAsyncRequestBool(url, body, "application/json", userAccessToken);
        Console.WriteLine("CHECK GENE RESPONSE = {0}", responseDict.code);
    }
}