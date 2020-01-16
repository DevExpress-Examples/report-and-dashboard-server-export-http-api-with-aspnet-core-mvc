using System.IO;

namespace ExportApiDemo.Models {
    public class ExportedDocumentContent {
        readonly Stream content;
        readonly string contentType;
        readonly string fileName;

        public ExportedDocumentContent(Stream content, string contentType, string fileName) {
            this.content = content;
            this.contentType = contentType;
            this.fileName = fileName;
        }

        public Stream Content { get { return content; } }
        public string ContentType { get { return contentType; } }
        public string FileName { get { return fileName; } }
    }
}