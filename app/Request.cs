using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;


class AsyncFunctions
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<JSONModel.Response> makeAsyncRequest(string url, Dictionary<string, string> body, string contentType, string authorization = "")
    {

        string myJson = JsonSerializer.Serialize(body);

        var req = new HttpRequestMessage(HttpMethod.Post, url);

        req.Content = new StringContent(myJson, Encoding.UTF8, contentType);

        if (authorization != "")
        {
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
        }

        var response = await client.SendAsync(req);

        string responseString = await response.Content.ReadAsStringAsync();

        JSONModel.Response? responseDict = JsonSerializer.Deserialize<JSONModel.Response>(responseString);
        if (responseDict == null){
            responseDict = new JSONModel.Response();
        }

        return responseDict;
    }

    public async Task<JSONModel.ResponseObject> asyncGet(string url, string authorization)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
        var response = await client.SendAsync(req);

        string responseString = await response.Content.ReadAsStringAsync();
        JSONModel.ResponseObject? resp = JsonSerializer.Deserialize<JSONModel.ResponseObject>(responseString);
        if (resp == null)
        {
            resp = new JSONModel.ResponseObject();
        }
        return resp;

    }

    public async Task<JSONModel.Response> makeAsyncRequestBool(string url, Dictionary<string, bool> body, string contentType, string authorization = "")
    {

        string myJson = JsonSerializer.Serialize(body);

        var req = new HttpRequestMessage(HttpMethod.Post, url);

        req.Content = new StringContent(myJson, Encoding.UTF8, contentType);

        if (authorization != "")
        {
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
        }

        var response = await client.SendAsync(req);

        string responseString = await response.Content.ReadAsStringAsync();

        JSONModel.Response? responseDict = JsonSerializer.Deserialize<JSONModel.Response>(responseString);
        if (responseDict == null)
        {
            responseDict = new JSONModel.Response();
        }
        return responseDict;
    }
}