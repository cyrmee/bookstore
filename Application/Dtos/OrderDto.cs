﻿using Domain.Types;

namespace Application.Dtos;

public record OrderDto
(
    Guid Id,
    string UserName,
    DateTime OrderDate,
    double TotalAmount,
    OrderStatus Status
);

public record OrderWriteDto(string UserName, DateTime OrderDate, double TotalAmount, OrderStatus Status);