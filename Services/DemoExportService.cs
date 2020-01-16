using System.Threading.Tasks;
using System.Net.Http;
using ExportApiDemo.Models;
using TaskStatus = ExportApiDemo.Models.TaskStatus;
using System.Threading;
using Newtonsoft.Json;
using System.Text;

namespace ExportApiDemo.Services {
    public interface IExportService {
        Task<ExportedDocumentContent> ExportReport(int reportId);
        Task<ExportedDocumentContent> ExportDashboard(int dashboardId);
        Task<ExportedDocumentContent> GetScheduledJobGeneratedDocument(int generatedDocumentId);
    }

    public class DemoExportService : IExportService {
        readonly IHttpRequestService httpRequestService;

        const int ExportStatusPollingDelay = 1000;
        string ExportDashboardPath(int dashboardId) => $"api/documents/export/dashboard/{dashboardId}";
        string ExportGeneratedDocumentPath(int generatedDocumentId) => $"api/jobs/generateddocuments/{generatedDocumentId}/export";
        string ExportReportPath(int reportId) => $"api/documents/export/report/{reportId}";
        string DocumentExportStatusPath(string exportId) => $"api/documents/export/status/{exportId}";
        string DownloadReportPath(string exportId) => $"api/documents/export/result/{exportId}";

        public DemoExportService(IHttpRequestService httpRequestService) {
            this.httpRequestService = httpRequestService;
        }

        public async Task<ExportedDocumentContent> ExportReport(int reportId) {
            // Start report export task
            var documentParameters = new DocumentParameter[] { new DocumentParameter() { Name = "CustomerID", Value = "CACTU" } };
            string exportId = await ExportReport(reportId, GetPdfExportModel(documentParameters));

            // Check report export task status
            TaskStatus status = TaskStatus.InProgress;
            while (status != TaskStatus.Complete) {
                if((status = await GetReportExportTaskStatus(exportId)) == TaskStatus.Fault)
                    throw new DemoExportServiceException("task failed");
                Thread.Sleep(ExportStatusPollingDelay);
            }

            // Download exported report
            HttpContent exportContent = await GetReportExportContent(exportId);
            return new ExportedDocumentContent(await exportContent.ReadAsStreamAsync(), "application/pdf", "report.pdf");
        }

        public async Task<ExportedDocumentContent> ExportDashboard(int dashboardId) {
            HttpContent exportContent = await ExportDashboard(dashboardId, GetPdfExportModel());
            return new ExportedDocumentContent(await exportContent.ReadAsStreamAsync(), "application/pdf", "dashboard.pdf");
        }

        public async Task<ExportedDocumentContent> GetScheduledJobGeneratedDocument(int generatedDocumentId) {
            HttpContent exportContent = await ExportScheduledJobDocument(generatedDocumentId, new ExportOptions() { ExportFormat = "pdf" });
            return new ExportedDocumentContent(await exportContent.ReadAsStreamAsync(), "application/pdf", "jobResult.pdf");
        }


        async Task<string> ExportReport(int reportId, ExportRequestModel requestModel) {
            HttpResponseMessage startExportResponse = await GetExportResponse(ExportReportPath(reportId), requestModel);
            return JsonConvert.DeserializeObject<ReportExportResponse>(await startExportResponse.Content.ReadAsStringAsync()).ExportId;
        }

        async Task<TaskStatus> GetReportExportTaskStatus(string exportId) {
            HttpResponseMessage exportStatusResponse = await httpRequestService.GetAsync($"{DocumentExportStatusPath(exportId)}");
            return JsonConvert.DeserializeObject<ReportExportTaskStatusResponse>(await exportStatusResponse.Content.ReadAsStringAsync()).TaskStatus;
        }

        async Task<HttpContent> GetReportExportContent(string exportId) {
            return (await httpRequestService.GetAsync($"{DownloadReportPath(exportId)}")).Content;
        }

        async Task<HttpContent> ExportDashboard(int dashboardId, ExportRequestModel requestModel) {
            return (await GetExportResponse(ExportDashboardPath(dashboardId), requestModel)).Content;
        }

        async Task<HttpContent> ExportScheduledJobDocument(int generatedDocumentId, ExportOptions requestModel) {
            return (await GetExportResponse(ExportGeneratedDocumentPath(generatedDocumentId), requestModel)).Content;
        }

        async Task<HttpResponseMessage> GetExportResponse(string path, object requestModel) {
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
            return await this.httpRequestService.PostAsync(path, requestContent);
        }

        static ExportRequestModel GetPdfExportModel(DocumentParameter[] parameters = null) {
            return new ExportRequestModel {
                ExportOptions = new ExportOptions() { ExportFormat = "pdf" },
                DocumentParameters = parameters
            };
        }
    }
}
