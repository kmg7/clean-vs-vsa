﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

public abstract class ApiControllerBase : ControllerBase
{
    protected IMediator Mediator =>
        _mediator ??=
            HttpContext.RequestServices.GetService<IMediator>()
            ?? throw new InvalidOperationException("IMediator cannot be retrieved from request services.");

    private IMediator? _mediator;
}
