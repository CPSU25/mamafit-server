namespace MamaFit.BusinessObjects.Entity
{
    public class ComponentOptionPreset
    {
        public string? ComponentOptionsId { get; set; }
        public string? PresetsId { get; set; }

        //Navigation object
        public ComponentOption? ComponentOption { get; set; }
        public Preset? Preset { get; set; }
    }
}
