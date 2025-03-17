﻿using Unisphere.Core.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed class DeleteHouseCommand : ICommand<bool>
{
    public Guid? HouseId { get; set; }
}
