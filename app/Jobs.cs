using System.Text;

public class Job
{
    public string id { get; set; } = "";
    public string type { get; set; } = "";
    public string strand { get; set; } = "";
    public string strandEncoded { get; set; } = "";
    public string geneEncoded { get; set; } = "";


    public string encodeStrand(string jobStrand)
    {
        string bits = Converter.stringToBits(jobStrand);

        var byteArray = Enumerable.Range(0, int.MaxValue / 8)
                          .Select(i => i * 8)
                          .TakeWhile(i => i < bits.Length)
                          .Select(i => bits.Substring(i, 8))
                          .Select(s => Convert.ToByte(s, 2))
                          .ToArray();

        string strandEncoded = System.Convert.ToBase64String(byteArray);
        return strandEncoded;

    }


    public string decodeStrand(string jobEncodedStrand)
    {
        byte[] byteArray = System.Convert.FromBase64String(jobEncodedStrand);

        string[] byteString = byteArray.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray();
        StringBuilder bits = new StringBuilder();
        foreach (var bytes in byteString)
        {
            bits.Append(bytes);
        }

        string decodedStrand = Converter.bitsToString(bits.ToString());

        return decodedStrand;
    }


    public bool checkGene(string jobEncodedStrand, string geneEncodedStrand)
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
        Console.WriteLine("Ativo = {0}", isActive);
        return isActive;

        // var body = new Dictionary<string, bool>{
        //     {"isActivated", isActive}
        // };
        // AsyncFunctions request = new AsyncFunctions();
        // string url = "https://gene.lacuna.cc/api/dna/jobs/" + jobId + "/gene";

        // var responseDict = await request.makeAsyncRequestBool(url, body, "application/json", userAccessToken);
        // Console.WriteLine("Check Gene Response = {0}", responseDict.code);
    }
}