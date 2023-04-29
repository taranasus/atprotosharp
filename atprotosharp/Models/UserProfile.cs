namespace atprotosharp;

public class UserProfile
{
    public string Did { get; set; }
    public string Handle { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
    public string Banner { get; set; }
    public int FollowsCount { get; set; }
    public int FollowersCount { get; set; }
    public int PostsCount { get; set; }
    public string IndexedAt { get; set; }
    public ViewerModel Viewer { get; set; }
    public List<object> Labels { get; set; }

    public class ViewerModel
    {
        public bool Muted { get; set; }
    }
}

