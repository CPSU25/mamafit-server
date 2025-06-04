﻿using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.ComponentDto
{
    public class ComponentResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
