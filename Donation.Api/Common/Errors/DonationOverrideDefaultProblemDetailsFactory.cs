﻿using System.Diagnostics;
using Donation.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Donation.Api.Common.Errors;

// This File is a copy of DefaultProblemDetailsFactory from AddController > AddMvcCore > 
// Here only change the class name and add a property
// We cannot extend this Class because it is Sealed

public sealed class DonationOverrideDefaultProblemDetailsFactory : ProblemDetailsFactory
{
  private readonly ApiBehaviorOptions _options;
  private readonly Action<ProblemDetailsContext>? _configure;

  public DonationOverrideDefaultProblemDetailsFactory(
      IOptions<ApiBehaviorOptions> options,
      IOptions<ProblemDetailsOptions>? problemDetailsOptions = null)
  {
    _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    _configure = problemDetailsOptions?.Value?.CustomizeProblemDetails;
  }

  public override ProblemDetails CreateProblemDetails(
      HttpContext httpContext,
      int? statusCode = null,
      string? title = null,
      string? type = null,
      string? detail = null,
      string? instance = null)
  {
    statusCode ??= 500;

    var problemDetails = new ProblemDetails
    {
      Status = statusCode,
      Title = title,
      Type = type,
      Detail = detail,
      Instance = instance,
    };

    ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

    return problemDetails;
  }

  public override ValidationProblemDetails CreateValidationProblemDetails(
      HttpContext httpContext,
      ModelStateDictionary modelStateDictionary,
      int? statusCode = null,
      string? title = null,
      string? type = null,
      string? detail = null,
      string? instance = null)
  {
    if (modelStateDictionary == null)
    {
      throw new ArgumentNullException(nameof(modelStateDictionary));
    }

    statusCode ??= 400;

    var problemDetails = new ValidationProblemDetails(modelStateDictionary)
    {
      Status = statusCode,
      Type = type,
      Detail = detail,
      Instance = instance,
    };

    if (title != null)
    {
      // For validation problem details, don't overwrite the default title with null.
      problemDetails.Title = title;
    }

    ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

    return problemDetails;
  }

  private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
  {
    problemDetails.Status ??= statusCode;

    if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
    {
      problemDetails.Title ??= clientErrorData.Title;
      problemDetails.Type ??= clientErrorData.Link;
    }

    var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
    if (traceId != null)
    {
      problemDetails.Extensions["traceId"] = traceId;
    }
    // HttpContextItemKeys.Errors it is our Constant
    // Items["errors"] is assigned in our ApiController Class
    var errors = httpContext.Items[HttpContextItemKeys.Errors] as List<Error>;
    if(errors is not null)
    {
      problemDetails.Extensions.Add(
        "errorCodes", errors.Select(err => err.Code)
      );
    }


    //problemDetails.Extensions.Add("customeProperty", "Custom Value added by DonationOverrideDefaultProblemDetailsFactory"); // <=


    //_configure?.Invoke(new() { HttpContext = httpContext!, ProblemDetails = problemDetails });
  }
}
//{
//    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
//    "title": "User with given email already exists",
//    "status": 400,
//    "traceId": "00-cae291986b7f2b7f9dca05d8eb904584-ed76646d75ac46fb-00",
//    "customeProperty": "Custom Value" // <= To Achieve this in result set
//}