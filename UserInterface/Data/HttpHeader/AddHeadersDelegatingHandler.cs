namespace UserInterface.Data.HttpHeader;

public class AddHeadersDelegatingHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    public AddHeadersDelegatingHandler(IConfiguration configuration) : base(new HttpClientHandler())
    {
        _configuration = configuration;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("XAPIKEY", _configuration.GetSection("XApiKey").Value!);  

        return base.SendAsync(request, cancellationToken);
    }
}