namespace EvensonFamilyTreeAppsDev.Models
{
    public class Partnership
    {
        public int Id { get; set; }

        public int Person1Id { get; set; }
        public Person Person1 { get; set; } = null!;

        public int Person2Id { get; set; }
        public Person Person2 { get; set; } = null!;

        public int? RelationshipTypeId { get; set; }
        public RelationshipType? RelationshipType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}
