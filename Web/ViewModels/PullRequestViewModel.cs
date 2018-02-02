using System;
using System.Collections.Generic;
using System.Linq;
using Web.Helpers;
using Web.Models;

namespace Web.ViewModels
{
    public class PullRequestViewModel
    {
        public string Name { get; set; }
        public List<PullRequests> PullRequests { get; set; }

        public PullRequestViewModel(string name, List<PullRequests> pullRequests)
        {
            Name = name;
            PullRequests = pullRequests;
        }
    }

    public class PullRequests
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string[] Description { get; set; }
        public string Date { get; set; }
        public AuthorWrapper Author { get; set; }
        public AuthorWrapper[] Reviewers { get; set; }
        public string Deeplink { get; set; }

        public PullRequests(PullRequest pullRequest)
        {
            Id = pullRequest.Id;
            Title = pullRequest.Title;
            Date = Convert.ToInt64(pullRequest.CreatedDate).FromTimestamp(); // convert date
            Author = pullRequest.Author;
            Author.Avatar = pullRequest.Author.GetUserAvatar();
            Reviewers = GetAvatarReviewers(pullRequest.Reviewers);
            Description = GetDescription(pullRequest.Description);
            Deeplink = pullRequest.Links.Self.First().Href.AbsoluteUri;
        }

        //photo's get wrappers
        private static AuthorWrapper[] GetAvatarReviewers(AuthorWrapper[] reviewers)
        {
            foreach (var wrapper in reviewers)
                wrapper.Avatar = wrapper.GetUserAvatar();

            return reviewers;
        }

        // break the description into lines
        private string[] GetDescription(string description)
        {
            return description?.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries) ?? new[] { "Описание отсутствует" };
        }
    }
}