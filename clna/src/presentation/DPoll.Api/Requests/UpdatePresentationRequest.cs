﻿namespace Dpoll.Api.Requests;

public class UpdatePresentationRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
}
