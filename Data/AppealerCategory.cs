namespace TechnicalConditions.Data
{
	public class AppealerCategory
	{
		public int Id { get; set; }
		public required string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
