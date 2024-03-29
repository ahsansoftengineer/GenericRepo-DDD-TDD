﻿using Donation.Application.Simple;
using Donation.Contracts.Simple;
using Donation.Domain.HierarchyAggregate;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donation.Api.Controllers
{
  [Route("hierarchy/[controller]")]
  public class SUController : ApiController
  {
    private readonly IMapper mapper;
    private readonly ISender mediator;
    public SUController(IMapper mapper, ISender mediator)
    {
      this.mapper = mapper;
      this.mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CommandRequestCreateSimpleChild request)
    {
      var command = mapper.Map<SimpleCommandChildCreate<SU>>(request);
      var createResult = await mediator.Send(command);
      return createResult.Match(
        entity => Ok(mapper.Map<SimpleResponseChildCreate>(entity)),
        errors => Problem(errors)
      );
    }

    //[HttpGet]
    //public async Task<IActionResult> Get(SimpleCreateRequest request)
    //{
    //  var command = mapper.Map<CreateSystemzCommand>(request);
    //  var createResult = await mediator.Send(command);
    //  return createResult.Match(
    //    entity => Ok(mapper.Map<SimpleChildCreateResponse>(entity)),
    //    errors => Problem(errors)
    //  );
    //}
  }
}
