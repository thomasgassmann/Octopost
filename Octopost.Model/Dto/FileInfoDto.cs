namespace Octopost.Model.Dto
{
    using System;

    public class FileInfoDto
    {
        public long Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public DateTime Created { get; set; }

        public string Link { get; set; }
    }
}
