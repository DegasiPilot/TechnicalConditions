namespace TechnicalConditions.Data
{
	public class Appeal
	{
		public int Id { get; set; }
		public int AppealerCategoryId { get; set; }
		public int AppealPurposeId { get; set; }
		public int AppealCaseId { get; set; }
		public required string UserId { get; set; }

		public AppealerCategory AppealerCategory { get; set; }
		public AppealPurpose AppealPurpose { get; set; }
		public AppealCase AppealCase { get; set; }
		public ApplicationUser User { get; set; }
		public List<Document> Documents { get; set; } = new();
	}
}