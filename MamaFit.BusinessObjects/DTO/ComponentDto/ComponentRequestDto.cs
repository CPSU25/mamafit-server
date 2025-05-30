﻿using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.ComponentDto
{
    public class ComponentRequestDto
    {
        public string? StyleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public List<ComponentOptionRequestDto>? Options { get; set; }
    }
}
