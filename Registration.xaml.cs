using System;
using System.Collections.Generic;
using System.Windows;
using snmDB.Models;
using snmDB.Services;

namespace snmDB
{
    public partial class RegistrationView : Window
    {
        private readonly UserService _userService;

        public RegistrationView(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string interestsInput = InterestsTextBox.Text.Trim();


            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

    
            var interests = new List<string>();
            if (!string.IsNullOrEmpty(interestsInput))
            {
                interests.AddRange(interestsInput.Split(','));
            }


            var existingUser = await _userService.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                MessageBox.Show("A user with this email already exists.");
                return;
            }


            var newUser = new User
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Interests = interests
            };

            await _userService.CreateUserAsync(newUser);

            MessageBox.Show("Registration successful!");
            Close();
        }
    }
}
