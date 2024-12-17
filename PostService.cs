using MongoDB.Driver;
using snmDB.Models;

namespace snmDB.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService(MongoDbService mongoDbService)
        {
            _posts = mongoDbService.GetPostCollection();  // Повертає колекцію "Posts"
        }

        // Додаємо метод для створення поста
        public async Task CreatePostAsync(Post post)
        {
            await _posts.InsertOneAsync(post);  // Додаємо новий пост до колекції
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _posts.Find(post => true).ToListAsync();  // Отримуємо всі пости
        }
    }
}
