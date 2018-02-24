namespace Octopost.Model.Data
{
    using Octopost.Model.Interfaces;
    using System;

    public class Vote : IIdentifiable, ICreated
    {
        public long Id { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public VoteState State { get; set; }

        public DateTime Created { get; set; }
    }
}
