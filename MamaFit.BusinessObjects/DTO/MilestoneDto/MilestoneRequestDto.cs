using MamaFit.BusinessObjects.Enum;
using System.ComponentModel.DataAnnotations;

namespace MamaFit.BusinessObjects.DTO.MilestoneDto
{
    public class MilestoneRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<ItemType>? ApplyFor { get; set; }
        public int SequenceOrder { get; set; }
    }
}
