﻿using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressTask
{
    public class MaternityDressTaskResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DressTaskType? Type { get; set; }
    }
}
