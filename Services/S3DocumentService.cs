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
		private const string _accessKey = "test";
		private const string _secretKey = "test";
		private const string _bucketName = "test";
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
