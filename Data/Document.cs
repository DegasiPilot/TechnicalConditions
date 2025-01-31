namespace TechnicalConditions.Data
{
	public class Document
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required byte[] ContentBytes { get; set; }
		public int AppealId { get; set; }

		public Appeal Appeal { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
