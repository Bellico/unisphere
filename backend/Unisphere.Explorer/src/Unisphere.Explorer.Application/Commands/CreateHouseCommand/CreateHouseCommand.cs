﻿using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed class CreateHouseCommand : ICommand<Guid>
{
    public string Description { get; set; }
}
