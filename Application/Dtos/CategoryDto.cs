﻿namespace Application.Dtos;

public record CategoryDto
(
    Guid Id,
    string Name
);

public record CategoryWriteDto(string Name);
