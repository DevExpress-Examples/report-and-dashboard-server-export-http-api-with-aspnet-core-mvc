using System;

namespace ExportApiDemo.Services {
    [Serializable]
    public class DemoExportServiceException : Exception {
        public DemoExportServiceException(string message) : base(message) {
        }
    }
}