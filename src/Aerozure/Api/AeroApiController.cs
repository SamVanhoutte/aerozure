using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aerozure.Api;

public abstract class AeroApiController : ControllerBase
{
            protected static bool IsValidIdFormat(string id, out Guid parsedId)
        {
            return Guid.TryParse(id, out parsedId);
        }

        protected static bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Define a regular expression for a simple email pattern
                // This pattern will catch most common email formats but isn't 100% exhaustive for all possible valid email addresses.
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                return regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        protected IActionResult ErrorStatus(int statusCode, string title, string? detail = null)
        {
            return StatusCode(statusCode, new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail ?? title
            });
        }

        protected IActionResult Forbidden(string title, string? detail = null)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = title,
                Detail = detail ?? title
            });
        }

        protected IActionResult Conflict(string title, string? detail = null)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = title,
                Detail = detail ?? title
            });
        }

        // protected IActionResult Unauthorized(string title, string? detail = null)
        // {
        //     return base.Unauthorized(new ProblemDetails
        //     {
        //         Status = StatusCodes.Status401Unauthorized,
        //         Title = title,
        //         Detail = detail ?? title
        //     });
        // }

        protected IActionResult BadRequest(string title, string? detail = null)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = title,
                Detail = detail ?? title
            });
        }

        protected IActionResult NotFound(string title, string? detail = null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = title,
                Detail = detail ?? title
            });
        }

        private IActionResult InvalidIdFormat(string fieldName, string? fieldValue = null)
        {
            var detail = $"The format of {fieldName} is not valid. It has to be a valid Guid.";
            if (!string.IsNullOrEmpty(fieldValue))
            {
                detail = $"The format of {fieldName} '{fieldValue}' is not valid. It has to be a valid Guid.";
            }

            return BadRequest($"The format of the {fieldName} is not valid.", detail);
        }

        protected IActionResult InvalidEmailFormat(string emailInput, string? fieldName = null)
        {
            var detail = $"The format of {emailInput} is not valid. It has to be a valid e-mail address.";
            if (!string.IsNullOrEmpty(fieldName))
            {
                detail = $"The format of {fieldName} '{emailInput}' is not valid. It has to be a valid e-mail address.";
            }

            return BadRequest($"The e-mail address {emailInput} is not valid.", detail);
        }

        protected IActionResult EntityNotFound(string entityName, string entityId)
        {
            return NotFound($"The {entityName} was not found.",
                $"{entityName} with id {entityId} was not found.");
        }

}