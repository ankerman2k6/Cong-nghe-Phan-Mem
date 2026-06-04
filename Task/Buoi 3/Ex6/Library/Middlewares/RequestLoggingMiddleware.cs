namespace Library.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)

        {
            //Chức năng 3: Chặn URL
            var currentPath = context.Request.Path.Value;
            if (currentPath == "/Book/Detail/0" || currentPath == "/Book/Detail/-1")
            {
                context.Response.StatusCode = 400; // Thiết lập mã lỗi 400
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Book id không hợp lệ");
                
                return; // QUAN TRỌNG: Lệnh return này sẽ ngắt pipeline, không gọi _next, do đó request không thể tới được Controller.
            }

            //Chức năng 1: Ghi log thông tin request
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var method = context.Request.Method;
            var path = context.Request.Path + context.Request.QueryString;
            Console.WriteLine($"[{timestamp}] Method: {method} - Path: {path}");
            
            // Đẩy request đi tiếp vào pipeline (tới các middleware khác hoặc Controller)
            await _next(context);
            //Chức năng 2: Ghi log mã trạng thái sau khi response được tạo
            Console.WriteLine($"Status: {context.Response.StatusCode}");
        }
    }
}