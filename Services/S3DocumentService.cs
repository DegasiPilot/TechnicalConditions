using TechnicalConditions.Data;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Components.Forms;
using Amazon;
using System.Text;
using System.Security.Cryptography;

namespace TechnicalConditions.Services
{
	public class S3DocumentService : IDocumentService
	{
		private const string _accessKey = "O79K7G04F4JBG3CPB864";
		private const string _secretKey = "Cn3vV6yeL6gwLvIdQ29vabZRufhlLH5bQrv4tYbI";
		private const string _bucketName = "803e1a41-793ea7ec-6fb2-4bb2-84dd-1f48208d719d";
		private const string _endpointUrl = "https://s3.timeweb.cloud";
		private static readonly AmazonS3Config _config = new AmazonS3Config()
		{
			ServiceURL = _endpointUrl,
			ForcePathStyle = true,
			AuthenticationRegion = "ru-1",
		};

		private AmazonS3Client _s3Client;

		public S3DocumentService()
		{
			_s3Client = new AmazonS3Client(_accessKey, _secretKey, _config);
		}

		public string GetPresignedUrl(Document document)
		{
			var request = new GetPreSignedUrlRequest
			{
				BucketName = _bucketName,
				Key = document.AppealId + "/" + document.Name,
				Expires = DateTime.UtcNow.AddMinutes(15)  //Временная ссылка действует 15 минут
			};

			return _s3Client.GetPreSignedURL(request);
		}

		public async Task<Document> UploadDocumentAsync(IBrowserFile browserFile, Appeal appeal)
		{
			string objectKey = $"{appeal.UserId}/{browserFile.Name}"; // Уникальное имя

			using var memoryStream = new MemoryStream();
			await browserFile.OpenReadStream().CopyToAsync(memoryStream);
			memoryStream.Position = 0;

			var request = new PutObjectRequest
			{
				BucketName = _bucketName,
				Key = objectKey,
				InputStream = memoryStream,
				ContentType = "application/octet-stream",
				AutoCloseStream = false, // Оставляем поток открытым
				UseChunkEncoding = false
			};

			await _s3Client.PutObjectAsync(request);

			return new Document()
			{
				Name = browserFile.Name,
				ContentBytes = Array.Empty<byte>(),
				Appeal = appeal
			};
		}

		public IResult SuccesDownloadFileResponse(Document document)
		{
			return Results.Redirect(GetPresignedUrl(document));
		}
	}
}
