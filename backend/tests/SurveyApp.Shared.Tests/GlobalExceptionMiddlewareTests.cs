using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using SurveyApp.Shared.Helpers;

namespace SurveyApp.Shared.Tests
{
    public class GlobalExceptionMiddlewareTests
    {
        private static DefaultHttpContext CreateContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        [Fact]
        public async Task InvokeAsync_NoException_CallsNext()
        {
            var nextCalled = false;
            RequestDelegate next = _ => { nextCalled = true; return Task.CompletedTask; };
            var middleware = new GlobalExceptionMiddleware(next, NullLogger<GlobalExceptionMiddleware>.Instance);

            await middleware.InvokeAsync(CreateContext());

            Assert.True(nextCalled);
        }

        [Fact]
        public async Task InvokeAsync_NoException_Returns200()
        {
            RequestDelegate next = ctx => { ctx.Response.StatusCode = 200; return Task.CompletedTask; };
            var middleware = new GlobalExceptionMiddleware(next, NullLogger<GlobalExceptionMiddleware>.Instance);
            var context = CreateContext();

            await middleware.InvokeAsync(context);

            Assert.Equal(200, context.Response.StatusCode);
        }

        [Theory]
        [InlineData(typeof(KeyNotFoundException), 404)]
        [InlineData(typeof(UnauthorizedAccessException), 401)]
        [InlineData(typeof(ArgumentException), 400)]
        [InlineData(typeof(Exception), 500)]
        public async Task InvokeAsync_ThrowsException_ReturnsCorrectStatusCode(Type exceptionType, int expectedStatus)
        {
            var ex = (Exception)Activator.CreateInstance(exceptionType)!;
            RequestDelegate next = _ => throw ex;
            var middleware = new GlobalExceptionMiddleware(next, NullLogger<GlobalExceptionMiddleware>.Instance);
            var context = CreateContext();

            await middleware.InvokeAsync(context);

            Assert.Equal(expectedStatus, context.Response.StatusCode);
        }
    }

}