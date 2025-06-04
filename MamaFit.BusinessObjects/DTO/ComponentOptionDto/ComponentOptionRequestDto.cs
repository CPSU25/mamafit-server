﻿using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.ComponentOptionDto
{
    public class ComponentOptionRequestDto
    {
        public string? ComponentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public ComponentOptionType? ComponentOptionType { get; set; }
    }
}
