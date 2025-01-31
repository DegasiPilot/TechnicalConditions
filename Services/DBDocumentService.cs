using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TechnicalConditions.Data;

namespace TechnicalConditions.Services
{
	public class DBDocumentService : IDocumentService
	{
		public async Task<Document> UploadDocumentAsync(IBrowserFile browserFile, Appeal appeal)
		{
			using (var memoryStream = new MemoryStream())
			{
				using var fileStream = browserFile.OpenReadStream();
				await fileStream.CopyToAsync(memoryStream);
				return new Document() {
					Name = browserFile.Name,
					ContentBytes = memoryStream.ToArray(),
					Appeal = appeal
				};
			}
		}

		public string GetPresignedUrl(Document document)
		{
			return $"/files/{document.Id}";
		}

		public IResult SuccesDownloadFileResponse(Document document)
		{
			return Results.File(document.ContentBytes, fileDownloadName: document.Name);
		}
	}
}
