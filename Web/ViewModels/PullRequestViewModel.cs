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
        public List<string> Description { get; set; }
        public string Date { get; set; }
        public AuthorWrapper Author { get; set; }
        public List<AuthorWrapper> Reviewers { get; set; }

        public PullRequests(PullRequest pullRequest)
        {
            Id = pullRequest.Id;
            Title = pullRequest.Title;
            Date = Convert.ToInt64(pullRequest.CreatedDate).FromTimestamp(); // convert date
            Author = pullRequest.Author;
            Author.Avatar = pullRequest.Author.GetUserAvatar();
            Reviewers = GetReviewers(pullRequest.Reviewers);
            Description = GetDescription(pullRequest.Description);
        }
        
        private static List<AuthorWrapper> GetReviewers(AuthorWrapper[] reviewers) //photo's get wrappers
        {
            foreach (var wrapper in reviewers.ToList())
                wrapper.Avatar = wrapper.GetUserAvatar();

            return reviewers.ToList();
        }

        private List<string> GetDescription(string description) //hyphenations
        {
            return description?.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>{ "Описание отсутствует" };
        }
    }
}