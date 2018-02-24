namespace Octopost.Model.Dto
{
    using System;

    public class FileDto
    {
        public long Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        public DateTime Created { get; set; }

        public string Link { get; set; }
    }
}
