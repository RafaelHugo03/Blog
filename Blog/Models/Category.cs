
namespace Blog.Models
{
   public class Category
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public IList<Post> Posts { get; set; }

        public Category(string name, string slug)
        {
            Name = name;
            Slug = slug;
            Posts = new List<Post>();
        }
    }
}