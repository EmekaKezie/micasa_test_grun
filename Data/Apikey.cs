namespace TheEstate.Data
{
    public class Apikey
    {
        private readonly RequestDelegate _next;

        const string APIKEY = "APP_SECRET";
        const string KeyValue = "emeka_kezie";

        public Apikey(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out var extractApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key missing");
                return;
            }

            if (!KeyValue.Equals(extractApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Api Key");
                return;
            }

            await _next(context);
        }
    }
}
