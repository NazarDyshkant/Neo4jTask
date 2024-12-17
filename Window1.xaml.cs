using System.Windows;
using snmDB.Services;

namespace snmDB
{
    public partial class Window1 : Window
    {
        private readonly Neo4jService _neo4jService;

        public Window1()
        {
            InitializeComponent();
        }
        public Window1(Neo4jService neo4jService)
        {
            InitializeComponent();
            _neo4jService = neo4jService;
        }

        private async void CreateNodeButton_Click(object sender, RoutedEventArgs e)
        {
            string userId = UserIdTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                await _neo4jService.CreateUserNodeAsync(userId, firstName, lastName);
                MessageBox.Show("User node created successfully.");
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private async void DeleteNodeButton_Click(object sender, RoutedEventArgs e)
        {
            string userId = UserIdTextBox.Text;

            if (!string.IsNullOrEmpty(userId))
            {
                await _neo4jService.DeleteUserNodeAsync(userId);
                MessageBox.Show("User node deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please provide a user ID.");
            }
        }

        private async void CreateRelationshipButton_Click(object sender, RoutedEventArgs e)
        {
            string fromUserId = FromUserIdTextBox.Text;
            string toUserId = ToUserIdTextBox.Text;
            string relationshipType = RelationshipTypeTextBox.Text;

            if (!string.IsNullOrEmpty(fromUserId) && !string.IsNullOrEmpty(toUserId) && !string.IsNullOrEmpty(relationshipType))
            {
                await _neo4jService.CreateRelationshipAsync(fromUserId, toUserId, relationshipType);
                MessageBox.Show("Relationship created successfully.");
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private async void DeleteRelationshipButton_Click(object sender, RoutedEventArgs e)
        {
            string fromUserId = FromUserIdTextBox.Text;
            string toUserId = ToUserIdTextBox.Text;
            string relationshipType = RelationshipTypeTextBox.Text;

            if (!string.IsNullOrEmpty(fromUserId) && !string.IsNullOrEmpty(toUserId) && !string.IsNullOrEmpty(relationshipType))
            {
                await _neo4jService.DeleteRelationshipAsync(fromUserId, toUserId, relationshipType);
                MessageBox.Show("Relationship deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private async void CheckConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            string fromUserId = FromUserIdTextBox.Text;
            string toUserId = ToUserIdTextBox.Text;

            if (!string.IsNullOrEmpty(fromUserId) && !string.IsNullOrEmpty(toUserId))
            {
                bool isConnected = await _neo4jService.CheckConnectionAsync(fromUserId, toUserId);
                MessageBox.Show(isConnected ? "Users are connected." : "Users are not connected.");
            }
            else
            {
                MessageBox.Show("Please provide both user IDs.");
            }
        }

        private async void GetDistanceButton_Click(object sender, RoutedEventArgs e)
        {
            string fromUserId = FromUserIdTextBox.Text;
            string toUserId = ToUserIdTextBox.Text;

            if (!string.IsNullOrEmpty(fromUserId) && !string.IsNullOrEmpty(toUserId))
            {
                int distance = await _neo4jService.GetDistanceAsync(fromUserId, toUserId);
                MessageBox.Show(distance >= 0 ? $"Distance: {distance}" : "No connection found.");
            }
            else
            {
                MessageBox.Show("Please provide both user IDs.");
            }
        }
    }
}
