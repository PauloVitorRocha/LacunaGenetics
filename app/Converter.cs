using System.Text;

public static class Converter
{
    public static string stringToBits(string strand)
    {
        StringBuilder baseBits = new StringBuilder();
        foreach (char letter in strand)
        {
            if (letter == 'A')
            {
                baseBits.Append("00");
            }
            else if (letter == 'C')
            {
                baseBits.Append("01");
            }
            else if (letter == 'G')
            {
                baseBits.Append("10");
            }
            else if (letter == 'T')
            {
                baseBits.Append("11");
            }
        }
        return baseBits.ToString();
    }

    public static string bitsToString(string bits)
    {
        StringBuilder decodedStrand = new StringBuilder();
        for (int i = 0; i < bits.Length - 1; i+=2)
        {
            if (bits[i] == '0')
            {
                if (bits[i + 1] == '0')
                    decodedStrand.Append("A");
                else
                    decodedStrand.Append("C");
            }
            else if (bits[i] == '1')
            {
                if (bits[i + 1] == '0')
                    decodedStrand.Append("G");
                else
                    decodedStrand.Append("T");
            }

        }
        return decodedStrand.ToString();
    }


    public static string getTemplateStrand(string bits)
    {
        StringBuilder templateStrand = new StringBuilder();
        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i] == 'A')
            {
                templateStrand.Append('T');
            }
            else if (bits[i] == 'T')
            {
                templateStrand.Append('A');
            }
            else if (bits[i] == 'C')
            {
                templateStrand.Append('G');
            }
            else if (bits[i] == 'G')
            {
                templateStrand.Append('C');
            }

        }
        return templateStrand.ToString();
    }
}