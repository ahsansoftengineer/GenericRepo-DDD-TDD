﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Donation.Api.Controllers
{
  public class ErrorsController : ControllerBase
  {
    [Route("/error")]
    public IActionResult Error()
    {
      // NOTE: The Default Impl by MVC Core which add Links with Status Code by default
      // "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1"
      // "traceId": "00-9e8f3f76d05d090fbfb308999b923d85-6d5ae68f347daf07-00"
      Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
      return Problem(title: exception?.Message, statusCode: 400);
    }
  }
}