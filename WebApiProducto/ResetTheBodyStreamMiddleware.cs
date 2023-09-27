using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Serilog;
using Serilog.Context;
using WebApiProducto.Models;


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
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> _logger)
    {
        _next = next;
        logger = _logger;
    }

    public async Task Invoke(HttpContext context)
    {

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        ResponseDetails response = new();
        context.Response.ContentType = MediaTypeNames.Application.Json;
        switch (exception)
        {
            case HttpRequestException ex:
                logger.LogError(ex, " Error: {Message}, {StackTrace} ", ex.Message, ex.StackTrace);
                response.details = ex.Message;
                response.status = ex.StatusCode.HasValue ? (int)ex.StatusCode! : StatusCodes.Status500InternalServerError;
                response.title = ErrorDescription.NoControlado;
                context.Response.StatusCode = ex.StatusCode.HasValue ? (int)ex.StatusCode! : StatusCodes.Status500InternalServerError;
                break;
            case TaskCanceledException ex:
                logger.LogError(ex, " Error: {Message}, {StackTrace} ", ex.Message, ex.StackTrace);
                response.details = ex.Message;
                response.status = StatusCodes.Status504GatewayTimeout;
                response.title = ErrorDescription.NoControlado;
                context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
                break;
            case Exception ex:
                logger.LogError(ex, " Error: {Message}, {StackTrace} ", ex.Message, ex.StackTrace);
                response.details = ex.Message;
                response.status = StatusCodes.Status500InternalServerError;
                response.title = ErrorDescription.NoControlado;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;

        }
        await context.Response.WriteAsync(response.ToString());

    }
}