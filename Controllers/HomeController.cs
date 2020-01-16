using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExportApiDemo.Models;
using ExportApiDemo.Services;

namespace ExportApiDemo.Controllers {
    public class HomeController : Controller {
        readonly IExportService demoExportService;

        const int DemoReportId = 1113;
        const int DemoDashboardId = 2455;
        const int DemoGeneratedDocumentId = 21579;

        public HomeController(IExportService exportService) {
            this.demoExportService = exportService;
        }

        public async Task<IActionResult> ExportReport() {
            ExportedDocumentContent content = null;
            try {
                content = await demoExportService.ExportReport(DemoReportId);
            } catch(DemoExportServiceException e) {
                return Ok(e.Message);
            }
            return File(content.Content, content.ContentType, content.FileName);
        }

        public async Task<FileStreamResult> ExportDashboard() {
            ExportedDocumentContent content = await demoExportService.ExportDashboard(DemoDashboardId);
            return File(content.Content, content.ContentType, content.FileName);
        }

        public async Task<FileStreamResult> GetScheduledJobGeneratedDocument() {
            ExportedDocumentContent content = await demoExportService.GetScheduledJobGeneratedDocument(DemoGeneratedDocumentId);
            return File(content.Content, content.ContentType, content.FileName);
        }

        public IActionResult Index() {
            return View();
        }
    }
}