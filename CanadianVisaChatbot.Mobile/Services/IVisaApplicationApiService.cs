using CanadianVisaChatbot.Shared.Models;

namespace CanadianVisaChatbot.Mobile.Services;

public interface IVisaApplicationApiService
{
    Task<VisaApplication> CreateApplicationAsync(VisaType type);
    Task<VisaApplication> GetApplicationAsync(string id);
    Task<List<VisaApplication>> GetUserApplicationsAsync();
    Task<DocumentInfo> UploadDocumentAsync(string applicationId, string name, DocumentType type, string notes, Stream fileStream);
    Task<Stream> DownloadDocumentAsync(string applicationId, string documentId);
    Task<ApplicationProgress> GetProgressAsync(string applicationId);
}