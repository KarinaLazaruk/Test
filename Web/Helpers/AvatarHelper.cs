using Web.Data;
using Web.Models;

namespace Web.Helpers
{
    public static class AvatarHelper
    {
        public static string GetUserAvatar(this AuthorWrapper author)
        {
            return $"{author.User.Links.Self[0].Href.AbsoluteUri}{ApiConstans.Avatar}";
        }

        public static string GetProjectAvatar(this Project project)
        {
            return $"{project.Links.Self[0].Href.AbsoluteUri}{ApiConstans.Avatar}";
        }
    }
}