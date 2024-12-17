using snmDB.Models;
using snmDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace snmDB
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly User _currentUser;

        public MainWindow(UserService userService, PostService postService, User user)
        {
            InitializeComponent();

            _userService = userService;
            _postService = postService;
            _currentUser = user;

            LoadPosts();
        }

        private async void LoadPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            PostsList.ItemsSource = posts.OrderByDescending(p => p.CreatedAt).ToList();
        }


        private async void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string friendEmail = FriendEmailTextBox.Text.Trim();
            if (string.IsNullOrEmpty(friendEmail))
            {
                MessageBox.Show("Please enter an email.");
                return;
            }

            var friend = await _userService.GetUserByEmailAsync(friendEmail);
            if (friend == null)
            {
                MessageBox.Show("User not found.");
                return;
            }

        }

        private async void RemoveFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFriendEmail = FriendsListBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedFriendEmail))
            {
                MessageBox.Show("Please select a friend to remove.");
                return;
            }


            _userService.DeleteUserAsync(selectedFriendEmail);
           
        }


    }
}
