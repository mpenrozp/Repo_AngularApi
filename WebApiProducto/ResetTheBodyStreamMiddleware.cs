using System.Text;
using Serilog;
using Serilog.Context;

public class SerilogRequestLogger
{
    readonly RequestDelegate _next;

    public SerilogRequestLogger(RequestDelegate next)
    {
        if (next == null) throw new ArgumentNullException(nameof(next));
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

        // Push the user name into the log context so that it is included in all log entries
        // LogContext.PushProperty("UserName", httpContext.User.Identity.Name);

        // Getting the request body is a little tricky because it's a stream
        // So, we need to read the stream and then rewind it back to the beginning
        string requestBody = "";
        HttpRequestRewindExtensions.EnableBuffering(httpContext.Request);
        Stream body = httpContext.Request.Body;
        byte[] buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
        await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
        requestBody = Encoding.UTF8.GetString(buffer);
        body.Seek(0, SeekOrigin.Begin);
        httpContext.Request.Body = body;

        /*Log.ForContext("RequestHeaders", httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
           .ForContext("RequestBody", requestBody)
           .Debug("Request information {RequestMethod} {RequestPath} information", httpContext.Request.Method, httpContext.Request.Path);
        */

        Log.Information("{RequestMethod} {RequestPath} {@requestBody} ", httpContext.Request.Method, httpContext.Request.Path, requestBody);
        // The reponse body is also a stream so we need to:
        // - hold a reference to the original response body stream
        // - re-point the response body to a new memory stream
        // - read the response body after the request is handled into our memory stream
        // - copy the response in the memory stream out to the original response stream
        using (var responseBodyMemoryStream = new MemoryStream())
        {
            var originalResponseBodyReference = httpContext.Response.Body;
            httpContext.Response.Body = responseBodyMemoryStream;

            await _next(httpContext);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            /*
                        Log.ForContext("RequestBody", requestBody)
                           .ForContext("ResponseBody", responseBody)
                           .Debug("Response information {RequestMethod} {RequestPath} {statusCode}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode);
            */
            Log.Information("{RequestMethod} {RequestPath} {@responseBody} {status}", httpContext.Request.Method, httpContext.Request.Path, responseBody, httpContext.Response.StatusCode);
            await responseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);
        }
    }
}
public class ResetTheBodyStreamMiddleware
{
    private readonly RequestDelegate _next;

    public ResetTheBodyStreamMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Still enable buffering before anything reads
        context.Request.EnableBuffering();

        // Call the next delegate/middleware in the pipeline
        await _next(context);

        // Reset the request body stream position to the start so we can read it
        context.Request.Body.Position = 0;


    }
}
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        //First, get the incoming request
        var request = await FormatRequest(context.Request);

        //Copy a pointer to the original response body stream
        var originalBodyStream = context.Response.Body;

        //Create a new memory stream...
        using var responseBody = new MemoryStream();
        //...and use that for the temporary response body
        context.Response.Body = responseBody;

        //Continue down the Middleware pipeline, eventually returning to this class
        await _next(context);

        //Format the response from the server
        var response = await FormatResponse(context.Response);

        //TODO: Save log to chosen datastore
        Log.Information(request + " " + response);
        //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static async Task<string> FormatRequest(HttpRequest request)
    {
        IEnumerable<string> keyValues = request.Headers.Keys.Select(key => key + ": " + string.Join(",", request.Headers[key]!));
        string requestHeaders = string.Join(System.Environment.NewLine, keyValues);
        var body = request.Body;

        //This line allows us to set the reader for the request back at the beginning of its stream.
        request.EnableBuffering();

        //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];

        //...Then we copy the entire request stream into the new buffer.
        await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);

        //We convert the byte[] into a string using UTF8 encoding...
        var bodyAsText = Encoding.UTF8.GetString(buffer);

        // reset the stream position to 0, which is allowed because of EnableBuffering()
        request.Body.Seek(0, SeekOrigin.Begin);

        return $"Request: Headers: {requestHeaders} {request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
    }

    private static async Task<string> FormatResponse(HttpResponse response)
    {
        //We need to read the response stream from the beginning...
        response.Body.Seek(0, SeekOrigin.Begin);

        //...and copy it into a string
        string text = await new StreamReader(response.Body).ReadToEndAsync();

        //We need to reset the reader for the response so that the client can read it.
        response.Body.Seek(0, SeekOrigin.Begin);

        //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
        return $"Response: {response.StatusCode}: {text}";
    }
}