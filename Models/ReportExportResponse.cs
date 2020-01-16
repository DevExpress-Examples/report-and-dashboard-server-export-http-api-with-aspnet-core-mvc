using Newtonsoft.Json;

namespace ExportApiDemo.Models {
    public class ReportExportResponse {
        [JsonProperty("exportId")]
        public string ExportId { get; set; }
    }
}