using ErrorOr;

namespace Unisphere.Explorer.Api;

internal static class CustomResults
{
    public static IResult Problem(IList<Error> errors)
    {
        var error = errors.FirstOrDefault();

        return Results.Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: GetStatusCode(error.Type));

        //var dictionary = new Dictionary<string, string[]>
        //{
        //    [error.Code] = [error.Description]
        //};

        //return Results.ValidationProblem(dictionary, statusCode: GetStatusCode(error.Type));
    }

    private static IResult CreateProblem(List<Error> errors)
    {
        var statusCode = errors.First().Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.ValidationProblem(errors.ToDictionary(k => k.Code, v => new[] { v.Description }),
            statusCode: statusCode);
    }

    static int GetStatusCode(ErrorType errorType) =>
          errorType switch
          {
              ErrorType.Validation => StatusCodes.Status400BadRequest,
              ErrorType.NotFound => StatusCodes.Status404NotFound,
              ErrorType.Conflict => StatusCodes.Status409Conflict,
              ErrorType.Forbidden => StatusCodes.Status403Forbidden,
              ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
              _ => StatusCodes.Status500InternalServerError
          };
}
