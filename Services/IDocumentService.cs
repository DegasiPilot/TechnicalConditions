using Microsoft.AspNetCore.Components.Forms;
using TechnicalConditions.Data;

namespace TechnicalConditions.Services
{
	public interface IDocumentService
	{
		public Task<Document> UploadDocumentAsync(IBrowserFile browserFile, Appeal appeal);
		public string GetPresignedUrl(Document document);
		public IResult SuccesDownloadFileResponse(Document document);
	}
}
